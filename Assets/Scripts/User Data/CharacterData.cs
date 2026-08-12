using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData
{
    public CharacterName characterID;
    public WeaponData WeaponEquippedData; // là weapon character đang sử dụng
    public CharacterProfile characterProfile; // playerpref không lưu scriptable object được, phải dùng con bài khác
}
