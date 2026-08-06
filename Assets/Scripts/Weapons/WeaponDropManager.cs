using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDropManager : MonoBehaviour
{
    // Script quản lý việc thưởng vũ khí cho người chơi sau khi win (thua không thưởng)
    // Cơ chế thưởng: drop vũ khí của character đang được sử dụng
    private WeaponDropService DropService;
    private WeaponRollService RollService;
    public static WeaponDropManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DropService = GetComponent<WeaponDropService>();
        RollService = GetComponent<WeaponRollService>();
        RollService.InitializeWeaponDropListInGame();
        RollService.InitializeEnhancedPrefixSum();
    }
    public void DropWeapon()
    {
        WeaponProfile weaponProfile;
        List<Perk> PerkList = new List<Perk>();
        (weaponProfile, PerkList) = RollService.RollWeapon();
        DropService.ShowWeaponDropped(weaponProfile, PerkList);
    }
}
