using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : BaseCharacter
{
    public override IEnumerator AttackWithAnimation(float Attack_Duration)
    {
        if (Level < 4)
        {
            BaseEnemy first_enemy = characterAttack.FindFirstEnemy();
            if (first_enemy != null)
            {
                SelfRotate(first_enemy);
                PlayAttackAmination(Attack_Duration);
                yield return new WaitForSeconds(profile.AttackDuration / SPUM_Prefabs._anim.speed * 0.5f);
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
                yield return new WaitForSeconds(profile.AttackDuration / SPUM_Prefabs._anim.speed * 0.5f);
                SPUM_Prefabs._anim.speed = 1;
            }
        }
        else
        {
            List<BaseEnemy> first_3_enemies = characterAttack.FindThreeFirstEnemies();
            if (first_3_enemies.Count == 0) yield break;
            if (first_3_enemies[0] != null) { SelfRotate(first_3_enemies[0]); }
            // Play animation
            PlayAttackAmination(Attack_Duration);
            yield return new WaitForSeconds(profile.AttackDuration / SPUM_Prefabs._anim.speed * 0.5f);
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
            yield return new WaitForSeconds(profile.AttackDuration / SPUM_Prefabs._anim.speed * 0.5f);
            SPUM_Prefabs._anim.speed = 1;
        }
    }
}
