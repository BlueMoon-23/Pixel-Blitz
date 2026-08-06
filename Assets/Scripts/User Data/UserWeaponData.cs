using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserWeaponData
{
    public List<WeaponData> UsedWeapons = new List<WeaponData>();
    public List<WeaponData> OwnedWeapons = new List<WeaponData>();
}
