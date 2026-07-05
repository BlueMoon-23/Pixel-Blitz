using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : BaseCharacter
{
    public override void AttackWithoutAnimation()
    {
        if (characterEffect.isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (characterAttack.GetEnemyCountInRange() != 0)
            {
                BaseEnemy first_enemy = characterAttack.FindFirstEnemy();
                if (first_enemy != null)
                {
                    SelfRotate(first_enemy);
                    // Bắn đạn: lưu ý là truyền góc là hướng bắn của mình luôn chứ không dùng transform.rotation hay quaternion.identity
                    float Angle_in_Radian = Mathf.Atan2(first_enemy.Center.transform.position.y - transform.position.y, first_enemy.Center.transform.position.x - transform.position.x);
                    Quaternion Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
                    if (BulletPooler.instance != null)
                    {
                        BaseBullets bullet = BulletPooler.instance.GetBullet(bullet_Prefab.GetComponent<BaseBullets>().BulletID);
                        if (bullet != null)
                        {
                            bullet.transform.position = Bullet_StartPosition.transform.position;
                            bullet.transform.rotation = Angle_in_Quaternion;
                            bullet.SetCharacter(this);
                            if (first_enemy != null)
                            {
                                bullet.SetEnemy(first_enemy);
                            }
                        }
                        // Gán headgun cho rangerlaser
                        RangerLaser rangerLaser = bullet.GetComponent<RangerLaser>();
                        rangerLaser.HeadGun = Bullet_StartPosition;
                    }
                    // Tạo hiệu ứng nổ đạn (muzzle)
                    MuzzleEffect(Angle_in_Quaternion);
                    Clock = 0f;
                }
            }
            else
            {
                Clock = Cooldown;
            }
        }
    }
}
