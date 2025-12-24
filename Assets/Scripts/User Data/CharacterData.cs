using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    // Cơ bản
    public string CharacterName;
    public Sprite CharacterImage;
    public float RangeStat;
    public float DamageStat;
    public float CooldownStat;
    public float CostStat;
    // Của UI
    public string Tier;
    public string Description;
    public string Special;
    public string HiddenDetection;
    public string Strikethrough;
    // Giá tiền
    public int GemRequire;
    public int DiamondRequire;
}
