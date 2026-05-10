using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterLevelData
{
    public float RangeStat;
    public float DamageStat;
    public float CooldownStat;
    public float UpgradeCost;
    public string UpgradeName;
    public string Special;
    public bool hasHiddenDetection;
    public bool canStrikethrough;
    public bool hasAbility = false;
}
