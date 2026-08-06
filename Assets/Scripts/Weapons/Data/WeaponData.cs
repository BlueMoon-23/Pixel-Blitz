using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string WeaponInstanceID;
    public string WeaponProfileID;
    public WeaponProfile weaponProfile;
    public List<Perk> Perks = new List<Perk>();
    public float GetBuff(PerkType type)
    {
        foreach (Perk perk in Perks)
        {
            if (perk.BuffType == type) return perk.PerkBonus;
        }
        return 1f;
    }
    public float RaiseCost()
    {
        if (weaponProfile != null)
        {
            return weaponProfile.WeaponRarity.CostMultiplier;
        }
        return 1f;
    }
}

