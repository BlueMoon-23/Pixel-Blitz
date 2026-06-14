using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using UnityEngine;

// Tạo Class bọc này để JsonUtility hiểu được cái List
[System.Serializable]
public class MatchDataWrapper
{
    public List<MatchData> list = new List<MatchData>();
}

public class MatchSaveManager : MonoBehaviour
{
    public MatchDataWrapper userMatchData; // Profile UI sẽ đọc nên để public
    public static MatchSaveManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey(UserDataKey.MATCHHISTORY_KEY))
        {
            string jsonData = PlayerPrefs.GetString(UserDataKey.MATCHHISTORY_KEY);
            userMatchData = JsonUtility.FromJson<MatchDataWrapper>(jsonData);
        }
    }
    /// <summary>
    /// kiểm tra xem nhóm mapData và loadout có đúng bằng với matchData không
    /// </summary>
    private bool isTheSameRound(MapData mapData, List<CharacterInfomation> loadout, MatchData matchData)
    {
        if (matchData.MapName == mapData.mapInformation.MapName && matchData.Gamemode == mapData.gamemode.name)
        {
            foreach (CharacterInfomation character1 in loadout)
            {
                bool isFound = false;
                foreach (string character2 in userMatchData.list[userMatchData.list.Count - 1].CharacterLoadout)
                {
                    if (character1.characterData.characterID == character2)
                    {
                        isFound = true;
                        break;
                    }
                }
                if (!isFound)
                {
                    Debug.Log("Co su sai khac ve character: " + character1.characterData.characterID);
                    return false;
                }
            }
            return true;
        }
        Debug.Log("Co su sai khac ve thong tin chung");
        return false;
    }
    public void CreateMatch(MapData mapData, List<CharacterInfomation> loadout)
    {
        if (userMatchData.list.Count > 0)
        {
            if (userMatchData.list[userMatchData.list.Count - 1].Status == "Defeat")
            {
                if (isTheSameRound(mapData, loadout, userMatchData.list[userMatchData.list.Count - 1])) {
                    userMatchData.list[userMatchData.list.Count - 1].Attempted++;
                    return;
                }
            }
        }
        MatchData matchData = new MatchData();
        matchData.MapName = mapData.mapInformation.MapName;
        matchData.Gamemode = mapData.gamemode.name;
        foreach (CharacterInfomation character in loadout)
        {
            matchData.CharacterLoadout.Add(character.characterData.characterID);
        }
        matchData.Status = "Defeat";
        matchData.Attempted = 1;
        matchData.TimePlayed = "00:00";
        userMatchData.list.Add(matchData);
        AccountSaveManager.CurrentAccount.RoundTimes++;
    }
    public void RestartMatch()
    {
        if (userMatchData.list.Count > 0)
        {
            if (userMatchData.list[userMatchData.list.Count - 1].Status == "Defeat")
            {
                userMatchData.list[userMatchData.list.Count - 1].Attempted++;
                return;
            }
        }
        MatchData newMatch = new MatchData();
        newMatch.MapName = userMatchData.list[userMatchData.list.Count - 1].MapName;
        newMatch.Gamemode = userMatchData.list[userMatchData.list.Count - 1].Gamemode;
        foreach (string character in userMatchData.list[userMatchData.list.Count - 1].CharacterLoadout)
        {
            newMatch.CharacterLoadout.Add(character);
        }
        newMatch.Status = "Defeat";
        newMatch.Attempted = 1;
        newMatch.TimePlayed = "00:00";
        userMatchData.list.Add(newMatch);
        AccountSaveManager.CurrentAccount.RoundTimes++;
    }
    public void UpdateCurrentMatch(bool doVictory, int timePlayed)
    {
        if (userMatchData.list.Count <= 0) return;
        if (timePlayed == 0) // Không chơi thì không tính
        {
            if (userMatchData.list[userMatchData.list.Count - 1].Attempted <= 1)
            {
                userMatchData.list.RemoveAt(userMatchData.list.Count - 1);
                AccountSaveManager.CurrentAccount.RoundTimes--;
            }
            else
            {
                userMatchData.list[userMatchData.list.Count - 1].Attempted--;
            }
            AccountSaveManager.CurrentAccount.AttemptTimes--;
            foreach (string character in userMatchData.list[userMatchData.list.Count - 1].CharacterLoadout)
            {
                AccountSaveManager.CurrentAccount.userCharacterData.CancelCharacter(character);
            }
        }
        else
        {
            MatchData currentMatch = userMatchData.list[userMatchData.list.Count - 1];
            if (currentMatch != null)
            {
                currentMatch.Status = doVictory ? "Victory" : "Defeat";
                currentMatch.TimePlayed = (timePlayed / 60).ToString("D2") + " : " + (TimeManager.instance.Get_TimePlayed() % 60).ToString("D2");
                string json = JsonUtility.ToJson(userMatchData);
                PlayerPrefs.SetString(UserDataKey.MATCHHISTORY_KEY, json);
                PlayerPrefs.Save();
                AccountSaveManager.instance.SaveAccounts();
            }
        }
    }
}
