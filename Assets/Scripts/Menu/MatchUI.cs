using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchUI : MonoBehaviour
{
    public TextMeshProUGUI MapData;
    public TextMeshProUGUI WinLose;
    public Image[] CharacterLoadout;
    public TextMeshProUGUI Attempt;
    public TextMeshProUGUI TimePlayed;
    public void UpdateUI(MatchData matchData)
    {
        MapData.text = matchData.MapName + " - " + matchData.Gamemode;
        WinLose.text = matchData.Status;
        Attempt.text = matchData.Attempted.ToString();
        if (WinLose.text == "Victory")
        {
            WinLose.color = new Color32(235, 255, 0, 255);
        }
        else
        {
            WinLose.color = new Color32(179, 0, 255, 255);
        }
        TimePlayed.text = matchData.TimePlayed;
        int i = 0;
        for (; i < matchData.CharacterLoadout.Count; i++)
        {
            foreach (CharacterData characterData in CharacterSaveManager.instance.allCharacters)
            {
                if (matchData.CharacterLoadout[i] == characterData.characterID)
                {
                    CharacterLoadout[i].sprite = characterData.characterProfile.CharacterImage;
                }
            }
        }
        for (; i < CharacterLoadout.Length; i++)
        {
            CharacterLoadout[i].transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
