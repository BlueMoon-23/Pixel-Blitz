using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Freezer : BaseCharacter
{
    [SerializeField] private float[] FreezeTimeByLevels;
    [SerializeField] private int[] FreezeCountByLevels;
    private float _FreezeTime;
    public float FreezeTime
    {
        get { return _FreezeTime; }
    }
    private int _FreezeCount;
    public int FreezeCount
    {
        get { return _FreezeCount; }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _FreezeTime = FreezeTimeByLevels[0];
        _FreezeCount = FreezeCountByLevels[0];
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
    public override void Upgrade()
    {
        base.Upgrade();
        _FreezeTime = FreezeTimeByLevels[Level];
        _FreezeCount = FreezeCountByLevels[Level];
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            base.SetUpgradeInformation();
            if (Level < profile.characterLevelDatas.Count - 1)
            {
                SetStatInfo(6, "Freeze time", _FreezeTime, FreezeTimeByLevels[Level + 1]);
                SetStatInfo(7, "Freeze hit count", _FreezeCount, FreezeCountByLevels[Level + 1]);
            }
        }
    }
    public override void AttackWithoutAnimation()
    {
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (range.enemies_in_range.Count != 0)
            {
                StartCoroutine(Burst());
                Clock = 0f;
            }
            else
            {
                Clock = Cooldown;
            }
        }
    }
    private IEnumerator Burst()
    {
        for (int i = 1; i <= 3; i++)
        {
            BaseEnemy first_enemy = FindFirstEnemy();
            if (first_enemy != null)
            {
                SelfRotate(first_enemy);
                Quaternion Angle_in_Quaternion = Shoot(first_enemy);
                MuzzleEffect(Angle_in_Quaternion);
            }
            yield return new WaitForSeconds(0.25f);
        }
        yield break;
    }
}
