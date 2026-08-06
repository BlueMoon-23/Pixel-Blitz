using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSaveManager : MonoBehaviour
{
    // Script quản lý danh sách vũ khí
    public List<WeaponData> WeaponDatabase;
    public static WeaponSaveManager instance;
    private Dictionary<string, WeaponProfile> WeaponProfileDict = new Dictionary<string, WeaponProfile>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (var weaponData in WeaponDatabase)
        {
            WeaponProfileDict.Add(weaponData.WeaponProfileID, weaponData.weaponProfile);
        }
        // Load lại data người chơi. load này ở character equip và shop
        if (PlayerPrefs.HasKey(UserDataKey.OWNEDWEAPON_KEY))
        {
            if (AccountSaveManager.instance != null)
            {
                AccountSaveManager.instance.LoadAccounts();
            }
        }
    }
    // Weapon Data chỉ không lưu được WeaponProfile do là scriptable object, cần duyệt lại để gán lại profile
    public void ReloadUserWeapon()
    {
        if (AccountSaveManager.instance != null)
        {
            foreach (WeaponData weaponData in AccountSaveManager.CurrentAccount.userWeaponData.OwnedWeapons)
            {
                if (weaponData.weaponProfile == null && !string.IsNullOrEmpty(weaponData.WeaponProfileID))
                {
                    weaponData.weaponProfile = WeaponProfileDict[weaponData.WeaponProfileID];
                }
            }
            foreach (WeaponData weaponData in AccountSaveManager.CurrentAccount.userWeaponData.UsedWeapons)
            {
                if (weaponData.weaponProfile == null && !string.IsNullOrEmpty(weaponData.WeaponProfileID))
                {
                    weaponData.weaponProfile = WeaponProfileDict[weaponData.WeaponProfileID];
                }
            }
            foreach (CharacterData characterData in AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters)
            {
                if (characterData.WeaponEquippedData.weaponProfile == null && !string.IsNullOrEmpty(characterData.WeaponEquippedData.WeaponProfileID))
                {
                    characterData.WeaponEquippedData.weaponProfile = WeaponProfileDict[characterData.WeaponEquippedData.WeaponProfileID];
                }
            }
        }
    }
}
