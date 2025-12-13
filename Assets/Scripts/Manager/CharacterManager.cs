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
        characterList.Remove(character);
        Change_CurrentCharacter();
    }
    public void Change_CurrentCharacter()
    {
        CurrentCharacter.text = characterList.Count.ToString() + " / " + Character_LimitPlacement.ToString();
    }
    public void LimitPlacement_Announce()
    {
        DOTween.KillAll();
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
}
