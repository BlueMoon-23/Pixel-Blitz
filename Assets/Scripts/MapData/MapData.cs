using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData
{
    public MapInformation mapInformation;
    public string targetScene;
    public Gamemodes gamemode;
    public float Difficulty()
    {
        return mapInformation.StarRate + gamemode.getDifficulty();
    }
    public int CharacterRequirement()
    {
        return (int)Difficulty();
    }
}
