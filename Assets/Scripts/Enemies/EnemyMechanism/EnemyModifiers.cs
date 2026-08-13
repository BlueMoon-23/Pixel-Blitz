using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyModifiers : MonoBehaviour
{
    // Lưu toàn bộ các modifiers, liên quan đến việc tạo / nhận hiệu ứng vĩnh viễn
    private SortedSet<float> slowModifiers = new SortedSet<float>();
    private SortedSet<float> boostModifiers = new SortedSet<float>();
    private SortedSet<float> HPModifiers = new SortedSet<float>();
    private EnemyStats enemyStats;
    public float ModifiedHP { get; private set; }
    public float ModifiedSpeed { get; private set; }
    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            ModifiedHP = enemyStats.enemyProfile.MaxHP;
            ModifiedSpeed = enemyStats.enemyProfile.OldSpeed;
        }
    }
    public void ResetModifiers() // gọi ở resetstats của baseE
    {
        slowModifiers.Clear();
        boostModifiers.Clear();
        HPModifiers.Clear();
        RecalculateSpeed();
        RecalculateHP();
    }
    public void AddSlowModifier(float percent)
    {
        slowModifiers.Add(percent);
    }
    public void AddSpeedUpModifier(float percent)
    {
        boostModifiers.Add(percent);
    }
    public bool ContainsModifier(float percent)
    {
        return slowModifiers.Contains(percent) || boostModifiers.Contains(percent);
    }
    public void RemoveSlowModifier(float percent)
    {
        slowModifiers.Remove(percent);
        RecalculateSpeed();
    }
    public void RemoveSpeedUpModifier(float percent)
    {
        boostModifiers.Remove(percent);
        RecalculateSpeed();
    }
    public float GetMinSlowPercent()
    {
        if (slowModifiers.Count == 0) return 1f;
        return slowModifiers.Min;
    }
    public float GetMaxBoostPercent()
    {
        if (boostModifiers.Count == 0) return 1f;
        return boostModifiers.Max;
    }
    public void RecalculateSpeed()
    {
        // mong muốn: speed = oldspeed * min của mảng slowmodifier * mã cua mảng boostmodifier
        float slow_factor = GetMinSlowPercent();
        float boost_factor = GetMaxBoostPercent();
        if (enemyStats != null) ModifiedSpeed = enemyStats.enemyProfile.OldSpeed * slow_factor * boost_factor;
    }
    public void AddHPModifier(float percent)
    {
        HPModifiers.Add(percent);
        RecalculateHP();
    }
    public void RemoveHPModifier(float percent)
    {
        HPModifiers.Remove(percent);
        RecalculateHP();
    }
    public void RecalculateHP()
    {
        if (enemyStats != null)
        {
            float total = enemyStats.enemyProfile.MaxHP;
            foreach (float percent in HPModifiers)
            {
                total *= percent;
            }
            ModifiedHP = total;
        }
    }
}
