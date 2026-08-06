using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponPreviewBox : WeaponBoxUI
{
    // Script quản lý weaponbox xuất hiện ở phần droplist của chestscene
    [SerializeField] private TextMeshProUGUI DropRate;
    public void InitializeWeapon(WeaponProfile weaponProfile, float RarityChance, int RarityCount, bool hasOwner)
    {
        if (hasOwner)
        {
            float calculatedRate = (float)System.Math.Round((RarityChance / RarityCount) * 100f, 2);
            DropRate.SetText("{0}%", calculatedRate);
            WeaponImage.color = Color.white;
        }
        else
        {
            DropRate.text = "0%";
            WeaponImage.color = Color.black;
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Weapon.weaponData.weaponProfile == null) return;
        WeaponUIControll.instance.ShowPreviewWeapon(Weapon.weaponData.weaponProfile);
        MoveByPointer(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (Weapon.weaponData.weaponProfile == null) return;
        // Sử dụng RectangleContainsScreenPoint để kiểm tra eventData.position có nằm trong WeaponBoxRectTransform không
        bool isInsideBox = RectTransformUtility.RectangleContainsScreenPoint(
            WeaponBoxRectTransform,
            eventData.position,
            null
        );
        if (isInsideBox)
        {
            DidHoldOutside = false;
            WeaponUIControll.instance.ShowPreviewWeapon(Weapon.weaponData.weaponProfile);
            MoveByPointer(eventData);
        }
        else
        {
            DidHoldOutside = true;
            WeaponUIControll.instance.WeaponUI.gameObject.SetActive(false);
        }
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        WeaponUIControll.instance.WeaponUI.gameObject.SetActive(false);
    }
}
