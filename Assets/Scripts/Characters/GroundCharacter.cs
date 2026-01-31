using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCharacter : BaseCharacter
{
    private void Start()
    {
        isCliff = false;
    }
    public override float GetCost()
    {
        return 0;
    }
    public override void UpgradeToLevel1()
    {

    }
    public override void UpgradeToLevel2()
    {

    }
    public override void UpgradeToLevel3()
    {

    }
    public override void UpgradeToLevel4()
    {
        if (OriginalUnitRoot != null)
        {
            OriginalUnitRoot.SetActive(false);
        }
        if (MaxLevelUnitRoot != null)
        {
            MaxLevelUnitRoot.SetActive(true);
            Animator maxlevelanimator = MaxLevelUnitRoot.GetComponent<Animator>();
            if (maxlevelanimator != null)
            {
                SPUM_Prefabs._anim = maxlevelanimator;
            }
            // Animation
            SPUM_Prefabs = GetComponent<SPUM_Prefabs>();
            if (SPUM_Prefabs == null)
            {
                SPUM_Prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
                if (!SPUM_Prefabs.allListsHaveItemsExist())
                {
                    SPUM_Prefabs.PopulateAnimationLists();
                }
            }
            SPUM_Prefabs.OverrideControllerInit();
            foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
            {
                IndexPair[state] = 0;
            }
        }
    }
    public override void SetAbilityIcon()
    {

    }
    public override void Ability(Vector3 position)
    {

    }
}
