using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    // Hai cái này hỏi AccountSaveManager và Authenticator
    public string Username;
    public string Password;
    // Cái này hỏi CurrencySaveManager
    public UserCurrencyData CurrencyData;
    // Cái này hỏi GiftcodeSaveManager
    public List<UserCodeData> redeemedCodes;
    // Cái này hỏi CharacterSaveManager
    public UserCharacterData userCharacterData;
    // Cái này hỏi MatchSaveManager
    public List<MatchData> userMatchData;
    // Cái này bên win_lose sẽ sửa
    public int ClearedTimes = 0;
    public int RoundTimes = 0;
    public int AttemptTimes = 0;
    // Cái này hỏi TutorialManager
    public bool hasPlayedTutorial;
}
