using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyModifiers : MonoBehaviour
{
    // Lưu toàn bộ các modifiers, liên quan đến việc tạo / nhận hiệu ứng vĩnh viễn
    List<float> slowModifiers = new List<float>();
    List<float> boostModifiers = new List<float>();
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
    public float GetMinSlowPercent()
    {
        float min = 1f;
        for (int i = 0; i < slowModifiers.Count; i++)
        {
            if (slowModifiers[i] < min) { min = slowModifiers[i]; }
        }
        return min;
    }
    public float GetMaxBoostPercent()
    {
        float max = 1f;
        for (int i = 0; i < boostModifiers.Count; i++)
        {
            if (boostModifiers[i] > max) { max = boostModifiers[i]; }
        }
        return max;
    }
}
