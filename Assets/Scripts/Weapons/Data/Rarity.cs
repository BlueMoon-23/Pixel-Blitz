using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Rarity")]
public class Rarity : ScriptableObject
{
    public WeaponRarity weaponRarity;
    public List<PerkBonusRange> perkBonusRanges = new List<PerkBonusRange>();
    public Color32 Color;
    public Sprite Frame;
    public int PerkCount;
    public int SellValue;
    public float CostMultiplier;
    public float NormalDropChance; // Tỉ lệ drop bình thường khi mở rương thường
    public float EnhancedDropChance; // Tỉ lệ drop cao hơn khi mở rương có dùng thêm diamond, và khi chơi bình thường
    public AudioClip RaritySFX; // được bật khi drop weapon theo rarity
    public float GetPerkBonus(PerkType type)
    {
        foreach (PerkBonusRange perk in perkBonusRanges)
        {
            if (perk.BuffType == type) return perk.GetRandomBonus();
        }
        return 0f;
    }
}
public enum WeaponRarity { Common, Rare, Legendary }