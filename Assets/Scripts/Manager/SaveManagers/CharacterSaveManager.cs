using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSaveManager : MonoBehaviour
{
    // Lưu toàn bộ character
    public List<CharacterData> allCharacters = new List<CharacterData>();
    // Và logic mua character
    public static CharacterSaveManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
        // Load lại data người chơi. load này ở character equip và shop
        if (PlayerPrefs.HasKey(UserDataKey.OWNEDCHARACTER_KEY))
        {
            if (AccountSaveManager.instance != null)
            {
                AccountSaveManager.instance.LoadAccounts();
            }
        }
        else
        {
            // Cấp sẵn Archer (0) và Musketeer(2) 
            BuyCharacter(allCharacters[0]);
            BuyCharacter(allCharacters[2]);
        }
    }
    public void BuyCharacter(CharacterData characterData)
    {
        if (characterData != null)
        {
            if (AccountSaveManager.instance != null)
            {
                AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters.Add(characterData);
                // Ghi lên OwnedCharacterKey
                string json = JsonUtility.ToJson(AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters);
                PlayerPrefs.SetString(UserDataKey.OWNEDCHARACTER_KEY, json);
                PlayerPrefs.Save();
                AccountSaveManager.instance.SaveAccounts();
            }
        }
    }
}
