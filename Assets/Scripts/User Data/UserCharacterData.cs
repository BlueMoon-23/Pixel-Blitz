using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserCharacterData
{
    // Class lưu data các character người chơi đã có
    public List<CharacterData> OwnedCharacters;
    public UserCharacterData()
    {
        OwnedCharacters = new List<CharacterData>();
    }
}
