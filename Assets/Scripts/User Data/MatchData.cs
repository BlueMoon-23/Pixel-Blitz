using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchData
{
    public string MapName;
    public string Gamemode;
    public string Status;
    public List<CharacterName> CharacterLoadout;
    public int Attempted;
    public string TimePlayed;
    public MatchData()
    {
        CharacterLoadout = new List<CharacterName>();
    }
}
