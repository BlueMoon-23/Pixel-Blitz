using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCalculator
{
    public static float CalculateRange(float RangeStat, WeaponData weaponData)
    {
        if (weaponData == null) return RangeStat;
        float rawRange = RangeStat * weaponData.GetBuff(PerkType.Range);
        return Mathf.Round(rawRange * 100f) / 100f;
    }
    public static float CalculateDamage(float DamageStat, WeaponData weaponData)
    {
        if (weaponData == null) return DamageStat;
        float rawDamage = DamageStat * weaponData.GetBuff(PerkType.Damage);
        return Mathf.Round(rawDamage * 100f) / 100f;
    }
    public static float CalculateCooldown(float CooldownStat, WeaponData weaponData)
    {
        if (weaponData == null || weaponData.GetBuff(PerkType.Cooldown) == 1) return CooldownStat;
        float rawCooldown = CooldownStat * (1 - weaponData.GetBuff(PerkType.Cooldown));
        return Mathf.Round(rawCooldown * 100f) / 100f;
    }
    public static float CalculateCost(float CostStat, WeaponData weaponData)
    {
        if (weaponData == null)
        {
            return CostStat;
        }
        return (int)(CostStat * weaponData.RaiseCost());
    }
}
