using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEquip : MonoBehaviour
{
    public GameObject MapInfo;
    public GameObject CharacterInfo;
    // Chi tiết về cục character UI
    public TextMeshProUGUI CharacterName;
    public Image CharacterImage;
    public TextMeshProUGUI RangeStat;
    public TextMeshProUGUI DamageStat;
    public TextMeshProUGUI CooldownStat;
    public TextMeshProUGUI CostStat;
    public CharacterInfomation chosenCharacter;
    // Thay đổi image và tiền màu xanh của loadout
    public List<CharacterInfomation> characterLoadout = new List<CharacterInfomation>();
    public Image[] CharacterLoadoutImages = new Image[4];
    public TextMeshProUGUI[] CharacterLoadoutCosts = new TextMeshProUGUI[4];
    private int CurrentIndex = 0; // 0, 1, 2, 3
    public GameObject Equip_Button;
    public GameObject Unequip_Button;
    // Singleton để truyền dữ liệu vào game scene ở hàm awake => CharacterLoadout làm, nếu để ở đây thì khi load lại scene
    // thì các button sẽ mất link
    public static CharacterEquip instance;
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
        // Reset lại character loadout mỗi lần load lại scene mapchoose
        if (CharacterLoadout.instance != null)
        {
            foreach (Transform child in CharacterLoadout.instance.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Close()
    {
        CharacterInfo.SetActive(false);
        MapInfo.SetActive(true);
    }
    public void ChooseCharacter(CharacterInfomation character)
    {
        MapInfo.SetActive(false);
        CharacterInfo.SetActive(true);
        // Chỉnh thông tin
        chosenCharacter = character;
        CharacterName.text = chosenCharacter.characterData.CharacterName;
        CharacterImage.sprite = chosenCharacter.characterData.CharacterImage;
        RangeStat.text = chosenCharacter.characterData.RangeStat.ToString();
        DamageStat.text = chosenCharacter.characterData.DamageStat.ToString();
        CooldownStat.text = chosenCharacter.characterData.CooldownStat.ToString();
        CostStat.text = chosenCharacter.characterData.CostStat.ToString();
        // Kiểm tra đã được equip vào loadout chưa
        for (int i = 0; i < characterLoadout.Count; i++)
        {
            if (characterLoadout[i] == chosenCharacter)
            {
                Unequip_Button.gameObject.SetActive(true);
                Equip_Button.gameObject.SetActive(false);
                return;
            }
        }
        Unequip_Button.gameObject.SetActive(false);
        Equip_Button.gameObject.SetActive(true);
    }
    public void Equip()
    {
        if (CurrentIndex >= 4)
        {
            for (int i = 0; i < 3; i++)
            {
                CharacterLoadoutImages[i].sprite = CharacterLoadoutImages[i + 1].sprite;
                CharacterLoadoutCosts[i].text = CharacterLoadoutCosts[i + 1].text;
            }
            characterLoadout.RemoveAt(0); // tự động dồn các phần tử lên luôn rồi
            CurrentIndex--;
        }
        // Chỉnh thông tin trên loadout
        CharacterLoadoutImages[CurrentIndex].gameObject.SetActive(true);
        CharacterLoadoutCosts[CurrentIndex].gameObject.SetActive(true);
        CharacterLoadoutImages[CurrentIndex].sprite = chosenCharacter.characterData.CharacterImage;
        CharacterLoadoutCosts[CurrentIndex].text = "$" + chosenCharacter.characterData.CostStat.ToString();
        // Kéo chosenCharacter vào List<CharacterInfomation> CharacterLoadout
        characterLoadout.Add(chosenCharacter);
        CurrentIndex++;
        Close();
    }
    public void Unequip()
    {
        for (int i = 0; i < 4; i++)
        {
            if (characterLoadout[i] == chosenCharacter)
            {
                characterLoadout.RemoveAt(i);
                // Kéo các phần tử cuối lên lấp lại vị trí trống
                for (int j = i; j < 3; j++)
                {
                    CharacterLoadoutImages[j].sprite = CharacterLoadoutImages[j + 1].sprite;
                    CharacterLoadoutCosts[j].text = CharacterLoadoutCosts[j + 1].text;
                }
                // Tạo khoảng trống ngăn không cho lấy lộn value
                CharacterLoadoutImages[3].sprite = null;
                CharacterLoadoutCosts[3].text = null;
                // Cài lại current index, sau đó tắt tại chỗ đó đi
                CurrentIndex = characterLoadout.Count;
                CharacterLoadoutImages[CurrentIndex].gameObject.SetActive(false);
                CharacterLoadoutCosts[CurrentIndex].gameObject.SetActive(false);
                break;
            }
        }
        Close();
    }
}