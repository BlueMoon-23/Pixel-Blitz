using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : BaseCharacter
{
    protected override void OnEnable()
    {
        base.OnEnable();
        Bow_Attack_Duration = 0.833f;
    }
    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            float min_duration = Bow_Attack_Duration < Cooldown ? Bow_Attack_Duration : Cooldown;
            if (!isStunned) { AttackWithCooldown(min_duration); }
            // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
        }
    }
    public override IEnumerator AttackWithAnimation(float Attack_Duration)
    {
        if (Level < 4)
        {
            BaseEnemy first_enemy = FindFirstEnemy();
            if (first_enemy != null)
            {
                SelfRotate(first_enemy);
                PlayAttackAmination(Attack_Duration);
                yield return new WaitForSeconds(Bow_Attack_Duration / SPUM_Prefabs._anim.speed * 0.5f);
                // Bắn đạn: đạn archer cong cong cho đẹp
                if (BulletPooler.instance != null)
                {
                    BaseBullets bullet = BulletPooler.instance.GetBullet(bullet_Prefab.GetComponent<BaseBullets>().BulletID);
                    if (bullet != null)
                    {
                        bullet.transform.position = Bullet_StartPosition.transform.position;
                        bullet.transform.rotation = Quaternion.identity;
                        bullet.SetCharacter(this);
                        if (first_enemy != null)
                        {
                            bullet.SetEnemy(first_enemy);
                        }
                    }
                }
                // Tạo hiệu ứng nổ đạn (muzzle)
                MuzzleEffect(Quaternion.identity);
                yield return new WaitForSeconds(Bow_Attack_Duration / SPUM_Prefabs._anim.speed * 0.5f);
                SPUM_Prefabs._anim.speed = 1;
            }
        }
        else
        {
            List<BaseEnemy> first_3_enemies = FindThreeFirstEnemies();
            if (first_3_enemies.Count == 0) yield break;
            if (first_3_enemies[0] != null) { SelfRotate(first_3_enemies[0]); }
            // Play animation
            PlayAttackAmination(Attack_Duration);
            yield return new WaitForSeconds(Bow_Attack_Duration / SPUM_Prefabs._anim.speed * 0.5f);
            for (int i = 0; i < first_3_enemies.Count; i++)
            {
                if (BulletPooler.instance != null)
                {
                    BaseBullets bullet = BulletPooler.instance.GetBullet(bullet_Prefab.GetComponent<BaseBullets>().BulletID);
                    if (bullet != null)
                    {
                        bullet.transform.position = Bullet_StartPosition.transform.position;
                        bullet.transform.rotation = Quaternion.identity;
                        bullet.SetCharacter(this);
                        if (first_3_enemies[i] != null) { bullet.SetEnemy(first_3_enemies[i]); }
                    }
                }
            }
            // Tạo hiệu ứng nổ đạn (muzzle)
            MuzzleEffect(Quaternion.identity);
            yield return new WaitForSeconds(Bow_Attack_Duration / SPUM_Prefabs._anim.speed * 0.5f);
            SPUM_Prefabs._anim.speed = 1;
        }
    }
}
