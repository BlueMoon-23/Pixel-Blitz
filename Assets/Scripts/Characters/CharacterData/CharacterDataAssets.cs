using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Character")]
public class CharacterProfile : ScriptableObject
{
    // Cơ bản
    public string CharacterName;
    public Sprite CharacterImage;
    public Color32 CharacterColor;
    public float CostStat;
    public bool isCliff;
    [Header("Nếu sử dụng animation, điền vào thời gian thực hiện animation")]
    public float AttackDuration;
    public List<CharacterLevelData> characterLevelDatas = new List<CharacterLevelData>();
    public List<CustomStats> characterCustomStats = new List<CustomStats>();
    // Của UI
    public string Tier;
    public string Description;
    public string Special;
    // Giá tiền
    public int GemRequire;
    public int DiamondRequire;
}
