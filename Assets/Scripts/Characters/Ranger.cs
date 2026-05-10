using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : BaseCharacter
{
    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            if (!isStunned) { AttackWithoutAnimation(); }
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
                        BulletPooler.instance.StartCoroutine(BulletPooler.instance.ReturnBulletWithDelay(bullet, 1f));
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
