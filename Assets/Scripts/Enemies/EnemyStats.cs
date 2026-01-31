using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Lưu trữ toàn bộ stats gốc (chỉnh từ inspector, ... ấy). pooling sẽ lấy nó làm cơ sở để reset lại enemy
    public float MaxHP;
    public float OldSpeed;
    public bool isHidden;
    public bool isArmored;
    // HP Bar;
    public float Original_x_HPScale;
}

