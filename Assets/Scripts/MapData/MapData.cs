using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData : IComparable<MapData>
{
    public MapInformation mapInformation;
    public Gamemodes gamemode;
    public float Difficulty()
    {
        return mapInformation.StarRate + gamemode.getDifficulty();
    }
    public int CharacterRequirement()
    {
        return (int)Difficulty();
    }
    // Nạp chồng toán tử < trong c#? 
    public static bool operator <(MapData left, MapData right)
    {
        if (left.Difficulty() != right.Difficulty())
            return left.Difficulty() < right.Difficulty();
        if (left.mapInformation.name != right.mapInformation.name) // string không có toán tử <
            return string.Compare(left.mapInformation.name, right.mapInformation.name, StringComparison.Ordinal) < 0;
        return string.Compare(left.gamemode.name, right.gamemode.name, StringComparison.Ordinal) < 0;
    }
    // Nạp chồng toán tử > (Bắt buộc đi kèm với <)
    public static bool operator >(MapData left, MapData right)
    {
        return right < left;
    }
    // Làm sao để cái dòng này Maps.Sort((x, y) => x.Difficulty().CompareTo(y.Difficulty())); có thể so sánh giữa 2 map data luôn?
    public int CompareTo(MapData other)
    {
        int diffCmp = this.Difficulty().CompareTo(other.Difficulty());
        if (diffCmp != 0) return diffCmp;
        int nameCmp = string.Compare(this.mapInformation.name, other.mapInformation.name, StringComparison.Ordinal);
        if (nameCmp != 0) return nameCmp;
        return string.Compare(this.gamemode.name, other.gamemode.name, StringComparison.Ordinal);
    }
}
