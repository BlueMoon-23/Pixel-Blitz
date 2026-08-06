using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WeaponDropService : MonoBehaviour
{
    // Script quản lý chung việc hiển thị Weapon drop được. WeaponDropManager và ChestManager có cái này
    public WeaponDropBox weaponDropBox;
    private Image WeaponFrame;
    [SerializeField] private Image WeaponGlow;
    public CanvasGroup frameCanvasGroup { get; private set; } // Kéo CanvasGroup của weapon frame vào đây
    protected void Start()
    {
        WeaponFrame = weaponDropBox.GetComponent<Image>();
        frameCanvasGroup = weaponDropBox.GetComponent<CanvasGroup>();
        frameCanvasGroup.gameObject.SetActive(false);
    }
    // Update is called once per frame
    public void ShowWeaponDropped(WeaponProfile weaponProfile, List<Perk> weaponPerks)
    {
        frameCanvasGroup.gameObject.SetActive(true);
        // Tạo vũ khí mới đưa về người chơi
        WeaponData NewWeapon = new WeaponData();
        NewWeapon.weaponProfile = weaponProfile;
        NewWeapon.WeaponInstanceID = Guid.NewGuid().ToString();
        NewWeapon.WeaponProfileID = weaponProfile.WeaponName;
        NewWeapon.Perks = weaponPerks;
        weaponDropBox.Weapon.weaponData = NewWeapon;
        if (AccountSaveManager.instance != null)
        {
            AccountSaveManager.CurrentAccount.userWeaponData.OwnedWeapons.Add(NewWeapon);
            // Ghi lên OwnedCharacterKey
            string json = JsonUtility.ToJson(AccountSaveManager.CurrentAccount.userWeaponData.OwnedWeapons);
            PlayerPrefs.SetString(UserDataKey.OWNEDWEAPON_KEY, json);
            PlayerPrefs.Save();
            AccountSaveManager.instance.SaveAccounts();
        }
        // Cập nhật khung
        WeaponFrame.sprite = weaponProfile.WeaponRarity.Frame;
        WeaponGlow.color = weaponProfile.WeaponRarity.Color;
        weaponDropBox.UpdateWeapon();
    }
}
