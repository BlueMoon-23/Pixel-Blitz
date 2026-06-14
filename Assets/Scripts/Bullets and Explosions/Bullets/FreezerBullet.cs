using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerBullet : BaseBullets
{
    void OnEnable()
    {
        StartCoroutine(SetupAtStart());
    }
    private IEnumerator SetupAtStart()
    {
        yield return null;
        if (character != null && character.GetLevel() < 3)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (character != null)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected override void ExplodeOnImpact()
    {
        if (character != null && character.GetLevel() < 4)
        {
            if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showExplosion)
            {
                BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(Explosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
                if (explosionSFX != null)
                {
                    explosionSFX.transform.position = this.transform.position;
                    explosionSFX.transform.rotation = Quaternion.identity;
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
                }
            }
            if (BulletPooler.instance != null)
            {
                BulletPooler.instance.ReturnBullet(this);
            }
        }
        else if (character.GetLevel() >= 4)
        {
            if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showExplosion)
            {
                BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(Explosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
                if (explosionSFX != null)
                {
                    explosionSFX.transform.position = this.transform.position;
                    explosionSFX.transform.rotation = Quaternion.identity;
                    explosionSFX.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
                }
            }
            else if (GameSetting.instance != null && GameSetting.instance != null && !GameSetting.instance._showExplosion)
            {
                BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(LowGraphic_Explosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
                if (explosionSFX != null)
                {
                    explosionSFX.transform.position = this.transform.position;
                    explosionSFX.transform.rotation = Quaternion.identity;
                    explosionSFX.transform.localScale = new Vector3(3f, 3f, 3f);
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
                }
            }
            if (BulletPooler.instance != null)
            {
                BulletPooler.instance.ReturnBullet(this);
            }
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (character != null && character.GetLevel() < 4)
        {
            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null && baseEnemy == enemy)
            {
                baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                Freezer freezer = character as Freezer; // as là toán tử ép kiểu
                baseEnemy.GetFreeze(freezer.FreezeTime, freezer.FreezeCount);
                baseEnemy.ModifySpeed(0.7f);
                ExplodeOnImpact();
            }
        }
        else if (character.GetLevel() >= 4)
        {
            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null && baseEnemy == enemy)
            {
                // Tạo 1 vòng tròn collider, rồi gây damage lên toàn bộ enemy trong vòng này
                Collider2D[] enemyInRadius = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                foreach (Collider2D enemy in enemyInRadius)
                {
                    BaseEnemy enemyGetDamaged = enemy.GetComponent<BaseEnemy>();
                    if (enemyGetDamaged != null)
                    {
                        enemyGetDamaged.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                        Freezer freezer = character as Freezer; // as là toán tử ép kiểu
                        enemyGetDamaged.GetFreeze(freezer.FreezeTime, freezer.FreezeCount);
                        baseEnemy.ModifySpeed(0.7f);
                    }
                }
                ExplodeOnImpact();
            }
        }
    }
}
