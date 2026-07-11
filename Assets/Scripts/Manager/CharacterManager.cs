using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static CharacterManager;
public class CharacterManager : MonoBehaviour
{
    // Quản lý danh sách character
    public static CharacterManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        characterList = new List<BaseCharacter> ();
        CharacterPositions = new List<Vector3>();
    }
    private List<BaseCharacter> characterList; // danh sách các character đang active
    // Mảng lưu vị trí character để kiểm tra tránh trùng lặp
    private List<Vector3> CharacterPositions;
    private int Character_LimitPlacement = 20;
    public TextMeshProUGUI CurrentCharacter;
    public TextMeshProUGUI Announcement;
    public enum CharacterName { Archer, Freezer, Musketeer, Minigunner, MinigunnerClone, Ranger, Rocketeer, Summoner, Pulser, Wizard, Guardian};
    private Dictionary<CharacterName, int> Limit_for_1_Character = new Dictionary<CharacterName, int> { 
        { CharacterName.Archer, 8 },
        { CharacterName.Freezer, 4 },
        { CharacterName.Musketeer, 8 },
        { CharacterName.Minigunner, 4 },
        //{ CharacterName.MinigunnerClone, 4 },
        { CharacterName.Ranger, 5 },
        { CharacterName.Rocketeer, 5 },
        { CharacterName.Summoner, 3 },
        { CharacterName.Pulser, 4 },
        { CharacterName.Wizard, 6 },
        { CharacterName.Guardian, 2 },
    };
    private Dictionary<CharacterName, int> CharacterQuantity = new Dictionary<CharacterName, int> {
        { CharacterName.Archer, 0 },
        { CharacterName.Freezer, 0 },
        { CharacterName.Musketeer, 0 },
        { CharacterName.Minigunner, 0 },
        //{ CharacterName.MinigunnerClone, 0 },
        { CharacterName.Ranger, 0 },
        { CharacterName.Rocketeer, 0 },
        { CharacterName.Summoner, 0 },
        { CharacterName.Pulser, 0 },
        { CharacterName.Wizard, 0 },
        { CharacterName.Guardian, 0 },
    };
    public TextMeshProUGUI Limit_for_1_Character_Text;
    // Pooler
    public Transform poolParent;
    private Dictionary<CharacterName, Stack<BaseCharacter>> pools;
    void Start()
    {
        poolParent = this.transform;
        pools = new Dictionary<CharacterName, Stack<BaseCharacter>>();
        int index = 0;
        if (CharacterLoadout.instance != null)
        {
            foreach (Transform child in CharacterLoadout.instance.transform)
            {
                BaseCharacter newCharacter = child.GetComponent<BaseCharacter>();
                CharacterName characterName = GetCharacterEnumName(newCharacter);
                for (int i = 0; i < Limit_for_1_Character[characterName]; i++)
                {
                    CreateCharacter(newCharacter);
                }
                index++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private CharacterName GetCharacterEnumName(BaseCharacter character)
    {
        CharacterName characterName = CharacterName.Archer;
        if (Enum.TryParse(character.GetType().Name, out CharacterName result)) // Ép kiểu từ string xuống enum
        {
            characterName = result;
        }
        return characterName;
    }
    public BaseCharacter CreateCharacter(BaseCharacter baseCharacter)
    {
        BaseCharacter Character = Instantiate(baseCharacter, poolParent);
        Character.gameObject.SetActive(false);
        CharacterName characterName = GetCharacterEnumName(baseCharacter);
        if (pools.TryGetValue(characterName, out Stack<BaseCharacter> pool))
        {
            pool.Push(Character);
        }
        else
        {
            pool = new Stack<BaseCharacter>();
            pool.Push(Character);
            pools[characterName] = pool;
        }
        return Character;
    }
    public BaseCharacter GetCharacter(BaseCharacter baseCharacter)
    {
        Stack<BaseCharacter> pool = null;
        CharacterName characterName = GetCharacterEnumName(baseCharacter);
        if (pools.ContainsKey(characterName))
        {
            pool = pools[characterName];
        }
        else
        {
            CreateCharacter(baseCharacter);
            pool = pools[characterName];
        }
        if (pool.Count > 0)
        {
            BaseCharacter Character = pool.Pop();
            Character.gameObject.SetActive(true);
            return Character;
        }
        else
        {
            BaseCharacter Character = CreateCharacter(baseCharacter);
            Character.gameObject.SetActive(true);
            pool.Pop();
            return Character;
        }
    }
    public void ReturnCharacter(BaseCharacter baseCharacter)
    {
        baseCharacter.transform.SetParent(poolParent);
        baseCharacter.gameObject.SetActive(false);
        CharacterName characterName = GetCharacterEnumName(baseCharacter);
        if (pools.TryGetValue(characterName, out Stack<BaseCharacter> pool))
        {
            if (!pool.Contains(baseCharacter))
            {
                pool.Push(baseCharacter);
            }
        }
    }
    public int GetPopulation()
    {
        return characterList.Count;
    }
    public BaseCharacter GetCharacterByIndex(int index)
    {
        if (characterList[index] == null) return null;
        return characterList[index];
    }
    public void AddCharacterWithPosition(BaseCharacter character, Vector3 position)
    {
        if (characterList.Count < 20)
        {
            characterList.Add(character);
            CharacterPositions.Add(position);
            CharacterName characterName = GetCharacterEnumName(character);
            Change_CurrentCharacter();
            if (CharacterQuantity[characterName] < Limit_for_1_Character[characterName])
            {
                CharacterQuantity[characterName]++;
            }
            else
            {
                characterList.Remove(character);
                CharacterPositions.Remove(position);
                Destroy(character.gameObject);
                Change_CurrentCharacter();
                Show_Limit_for_1_Character_Text(characterName, character);
            }
        }
        else
        {
            LimitPlacement_Announce();
        }
    }
    public void AddPosition(Vector3 position)
    {
        CharacterPositions.Add(position);
    }
    public void RemovePosition(Vector3 position)
    {
        CharacterPositions.Remove(position);
    }
    public bool hasCharacterinPosition(Vector3 position)
    {
        for (int i = 0; i < CharacterPositions.Count; i++)
        {
            if (CharacterPositions[i] == position) return true;
        }
        return false;
    }
    public void RemoveCharacter(BaseCharacter character)
    {
        for (int i = 0;i < characterList.Count; i++)
        {
            if (characterList[i] == character) CharacterPositions.Remove(CharacterPositions[i]);
        }
        CharacterName characterName = GetCharacterEnumName(character);
        CharacterQuantity[characterName]--;
        characterList.Remove(character);
        ReturnCharacter(character);
        Change_CurrentCharacter();
    }
    private void Change_CurrentCharacter()
    {
        CurrentCharacter.text = characterList.Count.ToString() + " / " + Character_LimitPlacement.ToString();
    }
    public void LimitPlacement_Announce()
    {
        //DOTween.KillAll();
        Announcement.gameObject.SetActive(true);
        Vector3 original_position = Announcement.transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            Announcement.DOFade(1f, 0.25f).From(0f);
        }).Join(Announcement.transform.DOMove(new Vector3(Announcement.transform.position.x, Announcement.transform.position.y - 25f, Announcement.transform.position.z), 0.25f));
        sequence.AppendInterval(1f).Append(Announcement.transform.DOMove(new Vector3(Announcement.transform.position.x, Announcement.transform.position.y + 25f, Announcement.transform.position.z), 0.25f)).AppendInterval(0.25f).JoinCallback(() =>
        {
            Announcement.DOFade(0f, 0.25f).From(1f);
        });
        sequence.OnComplete(() =>
        {
            Announcement.transform.position = original_position;
            Announcement.gameObject.SetActive(false);
        });
    }
    private void Show_Limit_for_1_Character_Text(CharacterName name, BaseCharacter character)
    {
        Limit_for_1_Character_Text.text = "You can only place " + Limit_for_1_Character[name] + " " + character.GetType().Name + "s.";
        //DOTween.KillAll();
        Limit_for_1_Character_Text.gameObject.SetActive(true);
        Vector3 original_position = Limit_for_1_Character_Text.transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            Limit_for_1_Character_Text.DOFade(1f, 0.25f).From(0f);
        }).Join(Limit_for_1_Character_Text.transform.DOMove(new Vector3(Limit_for_1_Character_Text.transform.position.x, Limit_for_1_Character_Text.transform.position.y - 25f, Limit_for_1_Character_Text.transform.position.z), 0.25f));
        sequence.AppendInterval(1f).Append(Limit_for_1_Character_Text.transform.DOMove(new Vector3(Limit_for_1_Character_Text.transform.position.x, Limit_for_1_Character_Text.transform.position.y + 25f, Limit_for_1_Character_Text.transform.position.z), 0.25f)).AppendInterval(0.25f).JoinCallback(() =>
        {
            Limit_for_1_Character_Text.DOFade(0f, 0.25f).From(1f);
        });
        sequence.OnComplete(() =>
        {
            Limit_for_1_Character_Text.transform.position = original_position;
            Limit_for_1_Character_Text.gameObject.SetActive(false);
        });
    }
    public void AbilityOutOfRange_Announce()
    {
        Limit_for_1_Character_Text.text = "Grave must not out of summoner's range!";
        //DOTween.KillAll();
        Limit_for_1_Character_Text.gameObject.SetActive(true);
        Vector3 original_position = Limit_for_1_Character_Text.transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            Limit_for_1_Character_Text.DOFade(1f, 0.25f).From(0f);
        }).Join(Limit_for_1_Character_Text.transform.DOMove(new Vector3(Limit_for_1_Character_Text.transform.position.x, Limit_for_1_Character_Text.transform.position.y - 25f, Limit_for_1_Character_Text.transform.position.z), 0.25f));
        sequence.AppendInterval(1f).Append(Limit_for_1_Character_Text.transform.DOMove(new Vector3(Limit_for_1_Character_Text.transform.position.x, Limit_for_1_Character_Text.transform.position.y + 25f, Limit_for_1_Character_Text.transform.position.z), 0.25f)).AppendInterval(0.25f).JoinCallback(() =>
        {
            Limit_for_1_Character_Text.DOFade(0f, 0.25f).From(1f);
        });
        sequence.OnComplete(() =>
        {
            Limit_for_1_Character_Text.transform.position = original_position;
            Limit_for_1_Character_Text.gameObject.SetActive(false);
        });
    }
    public void DestroyAllCharacters()
    {
        foreach (var pool in pools.Values)
        {
            pool.Clear();
        }
        for (int i = poolParent.childCount - 1; i >= 0; i--)
        {
            if (poolParent.GetChild(i).gameObject != null)
            {
                Destroy(poolParent.GetChild(i).gameObject);
            }
        }
        pools.Clear();
        characterList.Clear();
        Change_CurrentCharacter();
        CharacterPositions.Clear();
        CharacterQuantity.Clear();
    }
}
