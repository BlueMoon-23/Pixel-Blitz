using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
    public GameObject PillarBlast;
    public WeaponRollService RollService;
    private void Awake()
    {
        if (ChestManager.instance != null && ChestManager.instance.hasEnhanced)
        {
            RollService.InitializeEnhancedPrefixSum();
        }
        else
        {
            RollService.InitializeNormalPrefixSum();
        }
    }
    void Start()
    {
        StartCoroutine(OpenChest());
    }
    public IEnumerator OpenChest()
    {
        yield return new WaitForSeconds(0.8333f);
        PillarBlast.SetActive(true);
        WeaponProfile weaponProfile;
        List<Perk> PerkList = new List<Perk>();
        (weaponProfile, PerkList) = RollService.RollWeapon();
        ChestManager.instance.ShowWeaponDropped(weaponProfile, PerkList);
    }
    public List<WeaponProfile> GetWeaponDropListFromService()
    {
        return RollService.GetSortedWeaponDropList();
    }
}
