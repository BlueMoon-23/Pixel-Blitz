using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponUIControll : MonoBehaviour
{
    public Image WeaponUI;
    public TextMeshProUGUI WeaponName;
    public TextMeshProUGUI WeaponPerkCount;
    public List<PerkUI> PerkObjects = new List<PerkUI>();
    public RectTransform rectTransform;
    public static WeaponUIControll instance;
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
        rectTransform = GetComponent<RectTransform>();
    }
    public void ShowWeapon(WeaponData weaponData)
    {

        WeaponUI.gameObject.SetActive(true);
        WeaponName.text = weaponData.weaponProfile.WeaponName;
        WeaponName.color = weaponData.weaponProfile.WeaponRarity.Color;
        WeaponPerkCount.text = "";
        WeaponUI.sprite = weaponData.weaponProfile.WeaponRarity.Frame;
        foreach (var frameImage in PerkObjects)
        {
            frameImage.FrameImage.color = weaponData.weaponProfile.WeaponRarity.Color;
        }
        int index = 0;
        for (; index < math.min(weaponData.Perks.Count, PerkObjects.Count); index++)
        {
            PerkObjects[index].FrameImage.gameObject.SetActive(true);
            PerkObjects[index].PerkType.text = weaponData.Perks[index].BuffType.ToString();
            if (weaponData.Perks[index].BuffType == PerkType.Cooldown)
            {
                PerkObjects[index].PerkValue.text = $"-{weaponData.Perks[index].PerkBonus * 100f}%";
            }
            else
            {
                PerkObjects[index].PerkValue.SetText("x{0}", weaponData.Perks[index].PerkBonus);
            }
        }
        for (; index < PerkObjects.Count; index++)
        {
            PerkObjects[index].FrameImage.gameObject.SetActive(false);
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(WeaponUI.rectTransform);
    }
    public void ShowPreviewWeapon(WeaponProfile weaponProfile)
    {
        WeaponUI.gameObject.SetActive(true);
        WeaponName.text = weaponProfile.WeaponName;
        WeaponName.color = weaponProfile.WeaponRarity.Color;
        WeaponPerkCount.text = $"({weaponProfile.WeaponRarity.PerkCount} Random {(weaponProfile.WeaponRarity.PerkCount == 1 ? "Perk" : "Perks")})";
        WeaponUI.sprite = weaponProfile.WeaponRarity.Frame;
        foreach (var frameImage in PerkObjects)
        {
            frameImage.FrameImage.color = weaponProfile.WeaponRarity.Color;
        }
        for (int index = 0; index < PerkObjects.Count; index++)
        {
            PerkObjects[index].FrameImage.gameObject.SetActive(true);
            PerkObjects[index].PerkType.text = weaponProfile.WeaponRarity.perkBonusRanges[index].BuffType.ToString();
            if (weaponProfile.WeaponRarity.perkBonusRanges[index].BuffType == PerkType.Cooldown)
            {
                PerkObjects[index].PerkValue.text = $"-{weaponProfile.WeaponRarity.perkBonusRanges[index].MinBonus * 100f}%~-{weaponProfile.WeaponRarity.perkBonusRanges[index].MaxBonus * 100f}%";
            }
            else
            {
                PerkObjects[index].PerkValue.text = $"x{weaponProfile.WeaponRarity.perkBonusRanges[index].MinBonus}~x{weaponProfile.WeaponRarity.perkBonusRanges[index].MaxBonus}";
            }
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(WeaponUI.rectTransform);
    }
}
