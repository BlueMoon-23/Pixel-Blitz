using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketeer : BaseCharacter
{
    [SerializeField] private float[] ExplosionRadiusByLevels;
    private float ExplosionRadius;
    protected override void OnEnable()
    {
        base.OnEnable();
        ExplosionRadius = ExplosionRadiusByLevels[0];
    }
    public float GetExplosionRadius() { return ExplosionRadius; }
    public override void Upgrade()
    {
        base.Upgrade();
        ExplosionRadius = ExplosionRadiusByLevels[Level];
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            base.SetUpgradeInformation();
            if (Level < profile.characterLevelDatas.Count - 1)
            {
                SetStatInfo(6, "Explosion Radius", ExplosionRadius, ExplosionRadiusByLevels[Level + 1]);
            }
        }
    }
}
