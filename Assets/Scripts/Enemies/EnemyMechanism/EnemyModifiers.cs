using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyModifiers : MonoBehaviour
{
    // Lưu toàn bộ các modifiers, liên quan đến việc tạo / nhận hiệu ứng vĩnh viễn
    private SortedSet<float> slowModifiers = new SortedSet<float>();
    private SortedSet<float> boostModifiers = new SortedSet<float>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetModifiers() // gọi ở resetstats của baseE
    {
        slowModifiers.Clear();
        boostModifiers.Clear();
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
    }
    public void RemoveSpeedUpModifier(float percent)
    {
        boostModifiers.Remove(percent);
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
}
