using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData
{
    public MapInformation mapInformation;
    public Gamemodes gamemode;
    public float Difficulty()
    {
        int Gamemode_difficulty = 0;
        if (gamemode.GetType() == typeof(Easy))
        {
            Gamemode_difficulty = 1;
        }
        else if (gamemode.GetType() == typeof(Medium))
        {
            Gamemode_difficulty = 2;
        }
        else if (gamemode.GetType() == typeof(Hard))
        {
            Gamemode_difficulty = 3;
        }
        //
        return mapInformation.StarRate + Gamemode_difficulty;
    }
    public int CharacterRequirement()
    {
        return (int)Difficulty();
    }
}
