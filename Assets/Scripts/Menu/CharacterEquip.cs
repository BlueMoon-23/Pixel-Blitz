using System;
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
    public Image CharacterGlow;
    public TextMeshProUGUI RangeStat;
    public TextMeshProUGUI DamageStat;
    public TextMeshProUGUI CooldownStat;
    public TextMeshProUGUI CostStat;
    [field: SerializeField] public CharacterInfomation chosenCharacter { get; private set; }
    // Thay đổi image và tiền màu xanh của loadout
    public List<CharacterInfomation> characterLoadout = new List<CharacterInfomation>();
    public List<CharacterLoadoutUI> CharacterLoadoutUIs = new List<CharacterLoadoutUI>(4);
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
        MigrateLegacyLoadout();
        if (!string.IsNullOrEmpty(AccountSaveManager.CurrentAccount.userLoadoutKey))
        {
            string loadoutKey = AccountSaveManager.CurrentAccount.userLoadoutKey;
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
            List<CharacterName> characterKeys = new List<CharacterName>(oldLoadoutLength);
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
                        if (Enum.TryParse(res, out CharacterName result))
                        {
                            characterKeys.Add(result);
                            break;
                        }
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
                        CharacterLoadoutUIs[i].Setup(characterSource[j]);
                        CharacterLoadoutUIs[i].gameObject.SetActive(true);
                        characterLoadout.Add(characterSource[j]);
                        break;
                    }
                }
            }
        }
    }
    public void MigrateLegacyLoadout()
    {
        var account = AccountSaveManager.CurrentAccount;
        if (account == null || account.userLoadoutKey != null && account.userLoadoutKey.Length > 0)
            return; // Đã migrat rồi hoặc acc mới không cần
        // Lôi data cũ từ PlayerPrefs
        if (PlayerPrefs.HasKey("LoadoutKey"))
        {
            string legacyData = PlayerPrefs.GetString("LoadoutKey");
            // 3. Chuyển đổi từ định dạng cũ (string "Archer,Musketeer...") sang format mới (List<int>)
            account.userLoadoutKey = legacyData;
            // 4. Lưu lại vào file save JSON của Account
            AccountSaveManager.instance.SaveAccounts();
            // 5. Xóa key cũ sau khi đã chuyển thành công
            PlayerPrefs.DeleteKey("LoadoutKey");
            PlayerPrefs.Save();
            Debug.Log("Đã di trú thành công Loadout cũ cho account: " + account.Username);
        }
    }
    public void Close()
    {
        CharacterInfo.SetActive(false);  
        WeaponEquip.instance.Close();
        MapInfo.SetActive(true);
    }
    public void ChooseCharacter(CharacterInfomation character)
    {
        if (character == null) return;
        MapInfo.SetActive(false);
        CharacterInfo.SetActive(true);
        // Chỉnh thông tin
        chosenCharacter = character;
        CharacterName.text = chosenCharacter.characterData.characterProfile.CharacterName;
        CharacterName.color = chosenCharacter.characterData.characterProfile.CharacterColor;
        CharacterGlow.color = chosenCharacter.characterData.characterProfile.CharacterColor;
        CharacterImage.sprite = chosenCharacter.characterData.characterProfile.CharacterImage;
        // Hiện weapon của character ngay khi choose character
        foreach (CharacterData characterData in AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters)
        {
            if (characterData.characterID == chosenCharacter.characterData.characterID)
            {
                WeaponEquip.instance.characterWeaponBox.InitCharacterWeapon(characterData, character);
                break;
            }
        }
        // Gán current character cho weaponEquip, không để weaponEquip.Open() nữa
        WeaponEquip.instance.CurrentCharacter = chosenCharacter.characterData;
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
            if (WeaponEquip.instance != null)
            {
                WeaponEquip.instance.HideWeaponButton();
            }
            return;
        }
        else
        {
            if (WeaponEquip.instance != null)
            {
                WeaponEquip.instance.ShowWeaponButton();
            }
            CharacterImage.color = Color.white;
            Purchase_Button.gameObject.SetActive(false);
            var baseLevelData = chosenCharacter.characterData.characterProfile.characterLevelDatas[0];
            var weaponData = chosenCharacter.characterData.WeaponEquippedData;
            float oldRange = baseLevelData.RangeStat;
            float newRange = WeaponCalculator.CalculateRange(oldRange, weaponData);
            UpdateStatText(RangeStat, oldRange, newRange, isHigherBetter: true);
            float oldDamage = baseLevelData.DamageStat;
            float newDamage = WeaponCalculator.CalculateDamage(oldDamage, weaponData);
            UpdateStatText(DamageStat, oldDamage, newDamage, isHigherBetter: true);
            float oldCooldown = baseLevelData.CooldownStat;
            float newCooldown = WeaponCalculator.CalculateCooldown(oldCooldown, weaponData);
            UpdateStatText(CooldownStat, oldCooldown, newCooldown, isHigherBetter: false);
            float oldCost = chosenCharacter.characterData.characterProfile.CostStat;
            float newCost = WeaponCalculator.CalculateCost(oldCost, weaponData);
            UpdateStatText(CostStat, oldCost, newCost, isHigherBetter: false);
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
    private void UpdateStatText(TMP_Text textComponent, float oldVal, float newVal, bool isHigherBetter)
    {
        textComponent.text = newVal.ToString();
        if (Mathf.Approximately(newVal, oldVal))
        {
            textComponent.color = Color.white; // Hoặc màu mặc định của UI
        }
        else if (newVal > oldVal)
        {
            textComponent.color = isHigherBetter ? new Color32(165, 255, 107, 255) : new Color32(255, 100, 76, 255);
        }
        else // newVal < oldVal
        {
            textComponent.color = isHigherBetter ? new Color32(255, 100, 76, 255) : new Color32(165, 255, 107, 255); ;
        }
    }
    public void Equip()
    {
        if (CurrentIndex >= 4)
        {
            for (int i = 0; i < 3; i++)
            {
                CharacterLoadoutUIs[i].Setup(CharacterLoadoutUIs[i + 1].characterInfo);
            }
            characterLoadout.RemoveAt(0); // tự động dồn các phần tử lên luôn rồi
            CurrentIndex--;
        }
        // Chỉnh thông tin trên loadout
        CharacterLoadoutUIs[CurrentIndex].Setup(chosenCharacter);
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
                    CharacterLoadoutUIs[j].Setup(CharacterLoadoutUIs[j + 1].characterInfo);
                }
                // Tạo khoảng trống ngăn không cho lấy lộn value
                CharacterLoadoutUIs[3].ResetToNull();
                // Cài lại current index, sau đó tắt tại chỗ đó đi
                CurrentIndex = characterLoadout.Count;
                CharacterLoadoutUIs[CurrentIndex].CharacterLoadoutImage.gameObject.SetActive(false);
                CharacterLoadoutUIs[CurrentIndex].CharacterLoadoutCost.gameObject.SetActive(false);
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