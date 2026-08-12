using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Enemy")]
public class EnemyProfile : ScriptableObject
{
    // Lưu trữ toàn bộ stats gốc (chỉnh từ inspector, ... ấy). pooling sẽ lấy nó làm cơ sở để reset lại enemy
    public float MaxHP;
    public float OldSpeed;
    public bool isHidden;
    public bool isArmored;
    // Liên quan đến Index
    public string Name; // Tên để hiển thị
    public Sprite EnemyImage;
    public string Description;
}
