using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musketeer : BaseCharacter
{
    public bool didAttackImmediately;
    protected override void OnEnable()
    {
        base.OnEnable();
        didAttackImmediately = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            if (!isStunned) 
            {
                AttackWithoutAnimation(); 
            }
            // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
        }
    }
    public override void AttackWithoutAnimation()
    {
        if (isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (range.enemies_in_range.Count != 0)
            {
                BaseEnemy first_enemy = FindFirstEnemy();
                if (first_enemy != null)
                {
                    SelfRotate(first_enemy);
                    Quaternion Angle_in_Quaternion = Shoot(first_enemy);
                    MuzzleEffect(Angle_in_Quaternion);
                    Clock = 0f;
                    didAttackImmediately = false;
                }
            }
            else
            {
                Clock = Cooldown;
            }
        }
    }
    public void AttackImmediately()
    {
        if (isStunned || didAttackImmediately || Level < 3) { return; }
        BaseEnemy first_enemy = FindFirstEnemy();
        if (first_enemy != null)
        {
            SelfRotate(first_enemy);
            Quaternion Angle_in_Quaternion = Shoot(first_enemy);
            MuzzleEffect(Angle_in_Quaternion);
            Clock = 0f;
            didAttackImmediately = true;
        }
    }
}

/* Bắn chuỗi là: 
 * khi bắn 1 mục tiêu nếu kết liễu mục tiêu thì sẽ ngay lập tức thực hiện lại hàm bắn mục tiêu
 * vấn đề: cần 1 khoảng thời gian để chờ thông tin kết liễu. mong muốn là khi mục tiêu bị kết liễu thì ngay lập tức thực hiện hàm
 */
