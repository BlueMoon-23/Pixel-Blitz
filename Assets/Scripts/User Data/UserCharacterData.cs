using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class UserCharacterData
{
    // Class lưu data các character người chơi đã có, kèm theo số lần sử dụng
    public List<CharacterData> OwnedCharacters;
    // Tạo một struct để lưu cặp (ID - Số lần dùng)
    [System.Serializable]
    public struct CharacterUsedTime
    {
        public string characterID;
        public int usedTimes;
    }
    // Đổi Dictionary thành List này để hiển thị được Inspector VÀ lưu được JSON
    public List<CharacterUsedTime> Character_with_UsedTimes = new List<CharacterUsedTime>();
    public UserCharacterData()
    {
        OwnedCharacters = new List<CharacterData>();
        Character_with_UsedTimes = new List<CharacterUsedTime>();
    }
    public void RecordCharacter(CharacterData character)
    {
        // Tìm xem trong List đã có characterID này chưa
        int index = AccountSaveManager.CurrentAccount.userCharacterData.Character_with_UsedTimes
            .FindIndex(c => c.characterID == character.characterID);
        if (index >= 0)
        {
            // Nếu có rồi thì lấy ra tăng số lần dùng lên
            var data = AccountSaveManager.CurrentAccount.userCharacterData.Character_with_UsedTimes[index];
            data.usedTimes++;
            AccountSaveManager.CurrentAccount.userCharacterData.Character_with_UsedTimes[index] = data;
        }
        else
        {
            // Nếu chưa có thì add mới vào List với số lần dùng là 1
            AccountSaveManager.CurrentAccount.userCharacterData.Character_with_UsedTimes.Add(new UserCharacterData.CharacterUsedTime
            {
                characterID = character.characterID,
                usedTimes = 1
            });
        }
        string json = JsonUtility.ToJson(AccountSaveManager.CurrentAccount.userCharacterData);
        PlayerPrefs.SetString(UserDataKey.CHARACTERWITHUSEDTIME_KEY, json);
        PlayerPrefs.Save();
        AccountSaveManager.instance.SaveAccounts();
    }
    public void CancelCharacter(string characterID) // Đã sửa tham số thành string
    {
        // Tìm xem trong List đã có characterID này chưa bằng cách so sánh trực tiếp với chuỗi characterID truyền vào
        int index = AccountSaveManager.CurrentAccount.userCharacterData.Character_with_UsedTimes
            .FindIndex(c => c.characterID == characterID);
        if (index >= 0)
        {
            // Nếu có rồi thì lấy ra giảm số lần dùng xuống (đoạn này logic cũ ghi chú là tăng nhưng code là --, mình giữ nguyên -- nhé)
            var data = AccountSaveManager.CurrentAccount.userCharacterData.Character_with_UsedTimes[index];
            data.usedTimes--;
            AccountSaveManager.CurrentAccount.userCharacterData.Character_with_UsedTimes[index] = data;
        }
        else
        {
            return;
        }
        // Lưu dữ liệu
        string json = JsonUtility.ToJson(AccountSaveManager.CurrentAccount.userCharacterData);
        PlayerPrefs.SetString(UserDataKey.CHARACTERWITHUSEDTIME_KEY, json);
        PlayerPrefs.Save();
        AccountSaveManager.instance.SaveAccounts();
    }
    public List<CharacterData> Top4_Character_with_MostUses()
    {
        // Lọc và sắp xếp trực tiếp từ List data mới
        var top4Data = Character_with_UsedTimes.OrderByDescending(c => c.usedTimes).Take(4).ToList();
        // Map ngược lại danh sách CharacterData tương ứng trong OwnedCharacters
        List<CharacterData> Result = new List<CharacterData>();
        foreach (var data in top4Data)
        {
            CharacterData character = OwnedCharacters.Find(c => c.characterID == data.characterID);
            if (character != null)
            {
                Result.Add(character);
            }
        }
        return Result;
    }
}
