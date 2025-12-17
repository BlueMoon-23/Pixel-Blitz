using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        characterList = new List<BaseCharacter> ();
        CharacterPositions = new List<Vector3>();
    }
    private List<BaseCharacter> characterList;
    // Mảng lưu vị trí character để kiểm tra tránh trùng lặp
    private List<Vector3> CharacterPositions;
    private int Character_LimitPlacement = 20;
    public TextMeshProUGUI CurrentCharacter;
    public TextMeshProUGUI Announcement;
    public enum CharacterName { Archer, Freezer, Minigunner, MinigunnerClone, Ranger, Rocketeer, Summoner, Accelerator, Wizard};
    private Dictionary<CharacterName, int> Limit_for_1_Character = new Dictionary<CharacterName, int> { 
        { CharacterName.Archer, 8 },
        { CharacterName.Freezer, 4 },
        { CharacterName.Minigunner, 4 },
        { CharacterName.MinigunnerClone, 4 },
        { CharacterName.Ranger, 5 },
        { CharacterName.Rocketeer, 5 },
        { CharacterName.Summoner, 3 },
        { CharacterName.Accelerator, 4 },
        { CharacterName.Wizard, 6 },
    };
    private Dictionary<CharacterName, int> CharacterQuantity = new Dictionary<CharacterName, int> {
        { CharacterName.Archer, 0 },
        { CharacterName.Freezer, 0 },
        { CharacterName.Minigunner, 0 },
        { CharacterName.MinigunnerClone, 0 },
        { CharacterName.Ranger, 0 },
        { CharacterName.Rocketeer, 0 },
        { CharacterName.Summoner, 0 },
        { CharacterName.Accelerator, 0 },
        { CharacterName.Wizard, 0 },
    };
    public TextMeshProUGUI Limit_for_1_Character_Text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetPopulation()
    {
        return characterList.Count;
    }
    public void AddCharacterWithPosition(BaseCharacter character, Vector3 position)
    {
        if (characterList.Count < 20)
        {
            characterList.Add(character);
            CharacterPositions.Add(position);
            Change_CurrentCharacter();
            switch (character.GetType().Name)
            {
                case "Archer":
                    {
                        if (CharacterQuantity[CharacterName.Archer] < Limit_for_1_Character[CharacterName.Archer])
                        {
                            CharacterQuantity[CharacterName.Archer]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.Archer, character);
                        }
                        break;
                    }
                case "Freezer":
                    {
                        if (CharacterQuantity[CharacterName.Freezer] < Limit_for_1_Character[CharacterName.Freezer])
                        {
                            CharacterQuantity[CharacterName.Freezer]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.Freezer, character);
                        }
                        break;
                    }
                case "Minigunner":
                    {
                        if (CharacterQuantity[CharacterName.Minigunner] < Limit_for_1_Character[CharacterName.Minigunner])
                        {
                            CharacterQuantity[CharacterName.Minigunner]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.Minigunner, character);
                        }
                        break;
                    }
                case "Ranger":
                    {
                        if (CharacterQuantity[CharacterName.Ranger] < Limit_for_1_Character[CharacterName.Ranger])
                        {
                            CharacterQuantity[CharacterName.Ranger]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.Ranger, character);
                        }
                        break;
                    }
                case "Rocketeer":
                    {
                        if (CharacterQuantity[CharacterName.Rocketeer] < Limit_for_1_Character[CharacterName.Rocketeer])
                        {
                            CharacterQuantity[CharacterName.Rocketeer]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.Rocketeer, character);
                        }
                        break;
                    }
                case "Summoner":
                    {
                        if (CharacterQuantity[CharacterName.Summoner] < Limit_for_1_Character[CharacterName.Summoner])
                        {
                            CharacterQuantity[CharacterName.Summoner]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.Summoner, character);
                        }
                        break;
                    }
                case "Accelerator":
                    {
                        if (CharacterQuantity[CharacterName.Accelerator] < Limit_for_1_Character[CharacterName.Accelerator])
                        {
                            CharacterQuantity[CharacterName.Accelerator]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.Accelerator, character);
                        }
                        break;
                    }
                case "Wizard":
                    {
                        if (CharacterQuantity[CharacterName.Wizard] < Limit_for_1_Character[CharacterName.Wizard])
                        {
                            CharacterQuantity[CharacterName.Wizard]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.Wizard, character);
                        }
                        break;
                    }
                case "MinigunnerClone":
                    {
                        if (CharacterQuantity[CharacterName.MinigunnerClone] < Limit_for_1_Character[CharacterName.MinigunnerClone])
                        {
                            CharacterQuantity[CharacterName.MinigunnerClone]++;
                        }
                        else
                        {
                            characterList.Remove(character);
                            CharacterPositions.Remove(position);
                            Destroy(character.gameObject);
                            Change_CurrentCharacter();
                            Show_Limit_for_1_Character_Text(CharacterName.MinigunnerClone, character);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        else
        {
            LimitPlacement_Announce();
        }
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
        switch (character.GetType().Name)
        {
            case "Archer":
                {
                    CharacterQuantity[CharacterName.Archer]--;
                    break;
                }
            case "Freezer":
                {
                    CharacterQuantity[CharacterName.Freezer]--;
                    break;
                }
            case "Minigunner":
                {
                    CharacterQuantity[CharacterName.Minigunner]--;
                    break;
                }
            case "MinigunnerClone":
                {
                    CharacterQuantity[CharacterName.MinigunnerClone]--;
                    break;
                }
            case "Ranger":
                {
                    CharacterQuantity[CharacterName.Ranger]--;
                    break;
                }
            case "Rocketeer":
                {
                    CharacterQuantity[CharacterName.Rocketeer]--;
                    break;
                }
            case "Summoner":
                {
                    CharacterQuantity[CharacterName.Summoner]--;
                    break;
                }
            case "Accelerator":
                {
                    CharacterQuantity[CharacterName.Accelerator]--;
                    break;
                }
            case "Wizard":
                {
                    CharacterQuantity[CharacterName.Wizard]--;
                    break;
                }
            default:
                {
                    break;
                }
        }
        characterList.Remove(character);
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
    public void DestroyAllCharacters()
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            Destroy(characterList[i].gameObject);
        }
        characterList.Clear();
        Change_CurrentCharacter();
        CharacterPositions.Clear();
        CharacterQuantity.Clear();
    }
}
