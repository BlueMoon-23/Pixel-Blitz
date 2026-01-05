using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterID; // là tên character nhưng không ghi hoa tất cả ký tự
    public CharacterProfile characterProfile; // playerpref không lưu scriptable object được, phải dùng con bài khác
}
