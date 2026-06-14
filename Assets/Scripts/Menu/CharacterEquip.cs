using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject Purchase_Button;
    // Thông báo người chơi bắt buộc phải equip ít nhất 1 character để vào game
    public GameObject LoadoutGroup;
    public GameObject EquipAnnounce;
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
        if (PlayerPrefs.HasKey(UserDataKey.LOADOUTKEY)) 
        {
            string loadoutKey = PlayerPrefs.GetString(UserDataKey.LOADOUTKEY);
            // string có dạng: archer,freezer,minigunner,ranger, 
            // tách chuỗi ra từng cái 1
            int oldLoadoutLength = 0;
            List<int> commaIndex = new List<int>();
            for (int i = 0; i < loadoutKey.Length; i++)
            {
                if (loadoutKey[i] == ',')
                {
                    oldLoadoutLength++;
                    commaIndex.Add(i);
                }
            }
            CurrentIndex = oldLoadoutLength;
            List<string> characterKeys = new List<string>(oldLoadoutLength);
            for (int i = 0; i < oldLoadoutLength; i++)
            {
                int j = 0;
                if (i > 0)
                {
                    j = commaIndex[i - 1] + 1;
                }
                string res = "";
                for (; j < loadoutKey.Length; j++)
                {
                    if (loadoutKey[j] != ',') { res += loadoutKey[j]; }
                    else // nghĩa là đã chạm dấu phẩy, break vòng này
                    {
                        characterKeys.Add(res);
                        break;
                    }
                }
            }
            CharacterInfomation[] characterSource = FindObjectsOfType<CharacterInfomation>();
            for (int i = 0; i < characterKeys.Count; i++)
            {
                for (int j = 0; j < characterSource.Length; j++)
                {
                    if (characterKeys[i] == characterSource[j].characterData.characterID)
                    {
                        // Chỉnh thông tin trên loadout
                        CharacterLoadoutImages[i].gameObject.SetActive(true);
                        CharacterLoadoutCosts[i].gameObject.SetActive(true);
                        CharacterLoadoutImages[i].sprite = characterSource[j].characterData.characterProfile.CharacterImage;
                        CharacterLoadoutCosts[i].text = "$" + characterSource[j].characterData.characterProfile.CostStat.ToString();
                        characterLoadout.Add(characterSource[j]);
                        break;
                    }
                }
            }
        }
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
        CharacterName.text = chosenCharacter.characterData.characterProfile.CharacterName;
        CharacterImage.sprite = chosenCharacter.characterData.characterProfile.CharacterImage;
        // Kiểm tra người chơi có character này chưa
        if (!character.hasOwned)
        {
            Purchase_Button.gameObject.SetActive(true);
            CharacterImage.color = Color.black;
            Unequip_Button.gameObject.SetActive(false);
            Equip_Button.gameObject.SetActive(false);
            RangeStat.text = "?";
            DamageStat.text = "?";
            CooldownStat.text = "?";
            CostStat.text = "?";
            return;
        }
        else
        {
            CharacterImage.color = Color.white;
            Purchase_Button.gameObject.SetActive(false);
            RangeStat.text = chosenCharacter.characterData.characterProfile.characterLevelDatas[0].RangeStat.ToString();
            DamageStat.text = chosenCharacter.characterData.characterProfile.characterLevelDatas[0].DamageStat.ToString();
            CooldownStat.text = chosenCharacter.characterData.characterProfile.characterLevelDatas[0].CooldownStat.ToString();
            CostStat.text = chosenCharacter.characterData.characterProfile.CostStat.ToString();
        }
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
        CharacterLoadoutImages[CurrentIndex].sprite = chosenCharacter.characterData.characterProfile.CharacterImage;
        CharacterLoadoutCosts[CurrentIndex].text = "$" + chosenCharacter.characterData.characterProfile.CostStat.ToString();
        // Kéo chosenCharacter vào List<CharacterInfomation> CharacterLoadout. CharacterLoadout save luôn, để khi nhấn nút purchase xong quay lại sẽ tiện
        characterLoadout.Add(chosenCharacter);
        if (CharacterLoadout.instance != null)
        {
            CharacterLoadout.instance.SetCharacterLoadout();
        }
        CurrentIndex++;
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.OpenButton_Sound);
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
        if (CharacterLoadout.instance != null)
        {
            CharacterLoadout.instance.SetCharacterLoadout();
        }
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        Close();
    }
    // Đổi scene về lại ShopScene, đồng thời chỉ đúng về con character đó
    public void Purchase()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        if (CharacterSaveManager.instance != null)
        {
            for (int index = 0; index < CharacterSaveManager.instance.allCharacters.Count; index++)
            {
                if (CharacterSaveManager.instance.allCharacters[index].characterID == chosenCharacter.characterData.characterID)
                {
                    SceneKey.targetCharacterIndex = index;
                    break;
                }
            }
        }
        SceneKey.targetScene = SceneKey.ShopScene;
        SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
    }
}