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

    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            if (!isStunned) { AttackWithoutAnimation(); }
            // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
        }
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
            if (Level < 4)
            {
                SetStatInfo(6, "Explosion Radius", ExplosionRadius, ExplosionRadiusByLevels[Level + 1]);
            }
        }
    }
}
