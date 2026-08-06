using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class này là thông tin Perk thực tế của weapon
[System.Serializable]
public class Perk
{
    public PerkType BuffType;
    public float PerkBonus;
}

// Class này là thông tin Perk gắn lên trên Rarity, để nó biết được vùng để roll
[System.Serializable]
public class PerkBonusRange
{
    public PerkType BuffType;
    public float MinBonus;
    public float MaxBonus;
    public float GetRandomBonus()
    {
        int min = Mathf.RoundToInt(MinBonus * 100); // Ra đúng 105
        int max = Mathf.RoundToInt(MaxBonus * 100); // Ra đúng 110
        int randomInt = UnityEngine.Random.Range(min, max + 1); // Trả về random từ 105 đến 110
        return randomInt / 100f;
    }
}

public enum PerkType { Range, Damage, Cooldown }