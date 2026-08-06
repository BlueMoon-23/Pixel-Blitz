using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterWeaponBox : WeaponBoxUI
{
    public bool HasWeapon;
    public Image BoxFrame { get; set; }
    public Sprite DefaultFrame;
    protected new void Awake()
    {
        base.Awake();
        BoxFrame = GetComponent<Image>();
        HasWeapon = false;
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (HasWeapon) base.OnDrag(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (HasWeapon) base.OnPointerDown(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (HasWeapon)
        {
            if (!isValidWeapon(Weapon.weaponData)) return;
            WeaponUIControll.instance.WeaponUI.gameObject.SetActive(false);
            if (!DidHoldOutside)
            {
                WeaponEquip.instance.CurrentWeaponBox = this;
                WeaponEquip.instance.WeaponUnequipUI.gameObject.SetActive(true);
                // Hiện thông tin giá bán
                if (Weapon != null)
                {
                    WeaponEquip.instance.UnequipSell.text = "Sell (+" + Weapon.weaponData.weaponProfile.WeaponRarity.SellValue + " Gems)";
                }
            }
        }
    }
    public void ResetWhenUnequip()
    {
        HasWeapon = false;
        WeaponImage.enabled = false;
        BoxFrame.sprite = DefaultFrame;
        Weapon.weaponData = null; // Xóa liên kết
        UpdateWeapon();
    }
    public void CharacterEquipWeapon()
    {
        if (HasWeapon)
        {
            WeaponEquip.instance.DoUnequip();
        }
        BoxFrame.sprite = WeaponEquip.instance.CurrentWeaponBox.Weapon.weaponData.weaponProfile.WeaponRarity.Frame;
        HasWeapon = true;
        WeaponImage.enabled = true;
        Weapon.weaponData = WeaponEquip.instance.CurrentWeaponBox.Weapon.weaponData;
        UpdateWeapon();
    }
    private bool isValidWeapon(WeaponData weaponData)
    {
        if (weaponData != null && weaponData.weaponProfile != null)
        {
            foreach (WeaponData weaponData1 in AccountSaveManager.CurrentAccount.userWeaponData.UsedWeapons)
            {
                if (weaponData1.WeaponInstanceID == weaponData.WeaponInstanceID)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
    /// <summary>
    /// Gán lại weapon mới lên ô WeaponBox và ô Character Information
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="characterInfomation"></param>
    public void InitCharacterWeapon(CharacterData characterData, CharacterInfomation characterInfomation)
    {
        if (!isValidWeapon(characterData.WeaponEquippedData))
        {
            WeaponImage.sprite = null;
            WeaponImage.transform.localScale = Vector3.zero;
            ResetWhenUnequip(); // Trường hợp character này đã equip weapon xong chuyển sang con khác chưa có vũ khí, hasweapon vẫn chưa bị reset về false 
            return;
        }
        BoxFrame.sprite = characterData.WeaponEquippedData.weaponProfile.WeaponRarity.Frame;
        HasWeapon = true;
        WeaponImage.enabled = true;
        Weapon.weaponData = characterData.WeaponEquippedData;
        characterInfomation.characterData.WeaponEquippedData = Weapon.weaponData;
        UpdateWeapon();
    }
}
