using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData
{
    public string characterID; // là tên character nhưng không ghi hoa tất cả ký tự
    public WeaponData WeaponEquippedData; // là weapon character đang sử dụng
    public CharacterProfile characterProfile; // playerpref không lưu scriptable object được, phải dùng con bài khác
}
