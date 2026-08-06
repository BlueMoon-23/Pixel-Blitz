using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Weapon")]
public class WeaponProfile : ScriptableObject
{
    public Rarity WeaponRarity;
    public CharacterProfile WeaponOwner;
    public string WeaponName;
    public Sprite WeaponImage;
    public RuntimeAnimatorController WeaponAnimatorController;
    [Header("Lưu transform để load lên")]
    public Vector2 RectTransformPosition;
    public Quaternion Rotation;
    public Vector3 LocalScale;
}



