using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class WeaponBoxLocation
{
    public WeaponRarity Rarity;
    public GameObject Location;
}

public class WeaponEquip : MonoBehaviour
{
    public GameObject WeaponInventoryGroup;
    public GameObject WeaponEquipUI;
    public GameObject WeaponUnequipUI;
    public GameObject WeaponButton;
    public TextMeshProUGUI Gems_Text;
    public TextMeshProUGUI EquipSell;
    public TextMeshProUGUI UnequipSell;
    public WeaponBoxUI CurrentWeaponBox { get; set; }
    public CharacterData CurrentCharacter { get; set; }
    public CharacterWeaponBox characterWeaponBox;
    // vị trí instantiate tương ứng
    public List<WeaponBoxLocation> WeaponBoxLocation; 
    private Dictionary<WeaponRarity, GameObject> WeaponBoxLocationByRarity = new Dictionary<WeaponRarity, GameObject>();
    public static WeaponEquip instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (WeaponBoxLocation wb in WeaponBoxLocation)
        {
            WeaponBoxLocationByRarity.Add(wb.Rarity, wb.Location);
        }

    }
    private void Start()
    {
        if (AccountSaveManager.instance != null)
        {
            Gems_Text.text = AccountSaveManager.CurrentAccount.CurrencyData.UserGems.ToString();
        }
    }
    private void ShowWeapon(WeaponData weaponData)
    {

        if (weaponData.weaponProfile.WeaponOwner == CurrentCharacter.characterProfile)
        {
            WeaponBoxUI weaponBoxUI = WeaponBoxPooler.instance.GetWeaponBox(weaponData.weaponProfile.WeaponRarity.weaponRarity);
            if (weaponBoxUI != null)
            {
                weaponBoxUI.transform.position = WeaponBoxLocationByRarity[weaponBoxUI.Rarity].transform.position;
                weaponBoxUI.transform.SetParent(WeaponBoxLocationByRarity[weaponBoxUI.Rarity].transform);
                weaponBoxUI.transform.localScale = new Vector3(1, 1, 1);
                weaponBoxUI.transform.rotation = Quaternion.identity;
                weaponBoxUI.Weapon.weaponData = weaponData;
                weaponBoxUI.UpdateWeapon();
            }
        }
    }
    public void Open()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.ChooseMap_Sound);
        if (WeaponInventoryGroup.activeInHierarchy) return;
        WeaponInventoryGroup.SetActive(true);
        MapChoose.instance.AvailableMapInfo.SetActive(false);
        MapChoose.instance.InventoryInfo.SetActive(false);
        // Hiển thị Weapon theo Character hiện tại
        CurrentCharacter = CharacterEquip.instance.chosenCharacter.characterData;
        if (WeaponBoxPooler.instance != null && AccountSaveManager.CurrentAccount != null)
        {
            foreach (WeaponData weaponData in AccountSaveManager.CurrentAccount.userWeaponData.OwnedWeapons)
            {
                ShowWeapon(weaponData);
            }
        }
    }
    public void HideWeaponButton()
    {
        characterWeaponBox.gameObject.SetActive(false);
        WeaponButton.SetActive(false);
    }
    public void ShowWeaponButton()
    {
        characterWeaponBox.gameObject.SetActive(true);
        WeaponButton.SetActive(true);
    }
    public void CloseAll()
    {
        WeaponInventoryGroup.SetActive(false);
        CharacterEquip.instance.Close();
        WeaponBoxPooler.instance.ReturnAllWeaponBox();
        MapChoose.instance.StopShowAvailableMaps(); // trong hàm này có sound rồi
    }
    public void Close()
    {
        WeaponInventoryGroup.SetActive(false);
        WeaponBoxPooler.instance.ReturnAllWeaponBox();
        MapChoose.instance.StopShowAvailableMaps(); // trong hàm này có sound rồi
    }
    public void DoEquip()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
        // Copy weapon được chọn vào ô kia
        characterWeaponBox.CharacterEquipWeapon(); // swap weapon cũng đã có trong này
        // Cập nhật weapon được chọn vào character
        AccountSaveManager.CurrentAccount.userCharacterData.CharacterEquipWeapon(CurrentCharacter, CurrentWeaponBox.Weapon.weaponData);
        // Đồng thời xóa ô WeaponBoxUI đó
        if (WeaponBoxPooler.instance != null)
        {
            WeaponBoxPooler.instance.ReturnWeaponBox(CurrentWeaponBox);
        }
        // Lập tức cập nhật chỉ số của character được chọn
        if (CharacterEquip.instance != null)
        {
            CharacterEquip.instance.ChooseCharacter(CharacterEquip.instance.chosenCharacter);
        }
        WeaponEquipUI.SetActive(false);
    }
    public void DoNotEquip()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        WeaponEquipUI.SetActive(false);
    }
    public void DoUnequip()
    {
        if (!WeaponInventoryGroup.activeInHierarchy) // bật weapon inventory lên để ShowWeapon hoạt động được
        {
            Open();
        }
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
        WeaponData weaponToUnequip = characterWeaponBox.Weapon != null ? characterWeaponBox.Weapon.weaponData : null;
        if (weaponToUnequip == null)
        {
            Debug.LogError("[DoUnequip] characterWeaponBox.Weapon.weaponData đang bị NULL!");
            return;
        }
        if (WeaponBoxPooler.instance != null)
        {
            ShowWeapon(weaponToUnequip);
        }
        AccountSaveManager.CurrentAccount.userCharacterData.CharacterUnequipWeapon(CurrentCharacter, weaponToUnequip);
        // Lập tức cập nhật chỉ số của character được chọn
        if (CharacterEquip.instance != null)
        {
            CharacterEquip.instance.ChooseCharacter(CharacterEquip.instance.chosenCharacter);
        }
        characterWeaponBox.ResetWhenUnequip();
        WeaponUnequipUI.SetActive(false);
    }
    public void DoNotUnequip()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        WeaponUnequipUI.SetActive(false);
    }
    public void DoSell()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.BuyCharacter_Sound);
        // Bán vũ khí trong CurrentWeaponBox
        // Thêm tiền vào người chơi
        if (AccountSaveManager.instance != null && CurrencySaveManager.instance != null)
        {
            CurrencySaveManager.instance.AddGem(CurrentWeaponBox.Weapon.weaponData.weaponProfile.WeaponRarity.SellValue);
            Gems_Text.text = AccountSaveManager.CurrentAccount.CurrencyData.UserGems.ToString();
        }
        if (CurrentWeaponBox == characterWeaponBox)
        {
            // Xóa vũ khí đó ra khỏi UsedWeapons
            WeaponData itemToRemove = AccountSaveManager.CurrentAccount.userWeaponData.UsedWeapons
                .Find(w => w != null && w.WeaponInstanceID == CurrentWeaponBox.Weapon.weaponData.WeaponInstanceID);
            if (itemToRemove != null)
            {
                AccountSaveManager.CurrentAccount.userWeaponData.UsedWeapons.Remove(itemToRemove);
                string json = JsonUtility.ToJson(AccountSaveManager.CurrentAccount.userWeaponData.UsedWeapons);
                PlayerPrefs.SetString(UserDataKey.USEDWEAPON_KEY, json);
                PlayerPrefs.Save();
                AccountSaveManager.instance.SaveAccounts();
            }
            else
            {
                Debug.LogError("Item to remove = null");
            }
            // Xóa vũ khí đó ra khỏi OwnedCharacter
            // Kiểm tra trước các biến hệ thống
            if (AccountSaveManager.CurrentAccount?.userCharacterData?.OwnedCharacters == null)
            {
                Debug.LogError("Dữ liệu tài khoản hoặc danh sách nhân vật bị Null!");
                return;
            }

            if (CurrentCharacter == null)
            {
                Debug.LogError("CurrentCharacter chưa được chọn!");
                return;
            }

            // Tìm kiếm an toàn (thêm c != null)
            CharacterData targetCharacter = AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters
                .Find(c => c != null && c.characterID == CurrentCharacter.characterID);
            if (targetCharacter != null)
            {
                targetCharacter.WeaponEquippedData = null; // chỉ là ngắt WeaponData trong cái Account thôi
                string json = JsonUtility.ToJson(AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters);
                PlayerPrefs.SetString(UserDataKey.OWNEDCHARACTER_KEY, json);
                PlayerPrefs.Save();
                AccountSaveManager.instance.SaveAccounts();
            }
            else
            {
                Debug.LogWarning("Không tìm thấy nhân vật phù hợp trong danh sách OwnedCharacters!");
            }
            CurrentCharacter.WeaponEquippedData = null;
            characterWeaponBox.ResetWhenUnequip();
            WeaponUnequipUI.SetActive(false);
        }
        else
        {
            // Xóa vũ khí đó ra khỏi OwnedWeapons
            WeaponData itemToRemove = AccountSaveManager.CurrentAccount.userWeaponData.OwnedWeapons
                .Find(w => w != null && w.WeaponInstanceID == CurrentWeaponBox.Weapon.weaponData.WeaponInstanceID);
            if (itemToRemove != null)
            {
                AccountSaveManager.CurrentAccount.userWeaponData.OwnedWeapons.Remove(itemToRemove);
                string json = JsonUtility.ToJson(AccountSaveManager.CurrentAccount.userWeaponData.OwnedWeapons);
                PlayerPrefs.SetString(UserDataKey.OWNEDWEAPON_KEY, json);
                PlayerPrefs.Save();
                AccountSaveManager.instance.SaveAccounts();
            }
            else
            {
                Debug.LogError("Item to remove = null");
            }
            WeaponBoxPooler.instance.ReturnWeaponBox(CurrentWeaponBox);
            WeaponEquipUI.SetActive(false);
        }
        // Lập tức cập nhật chỉ số của character được chọn
        if (CharacterEquip.instance != null)
        {
            CharacterEquip.instance.ChooseCharacter(CharacterEquip.instance.chosenCharacter);
        }
    }
}
