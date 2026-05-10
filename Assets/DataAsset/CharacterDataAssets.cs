using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Character")]
public class CharacterProfile : ScriptableObject
{
    // Cơ bản
    public string CharacterName;
    public Sprite CharacterImage;
    public float CostStat;
    public bool isCliff;
    public List<CharacterLevelData> characterLevelDatas = new List<CharacterLevelData>();
    // Của UI
    public string Tier;
    public string Description;
    public string Special;
    // Giá tiền
    public int GemRequire;
    public int DiamondRequire;
}
