using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBullet : BaseBullets
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected override void ExplodeOnImpact()
    {
        // Tạo 1 vòng tròn collider, rồi gây damage lên toàn bộ enemy trong vòng này
        Collider2D[] enemyInRadius = Physics2D.OverlapCircleAll(transform.position, 2.0f);
        foreach (Collider2D enemy in enemyInRadius)
        {
            BaseEnemy enemyGetDamaged = enemy.GetComponent<BaseEnemy>();
            if (enemyGetDamaged != null)
            {
                enemyGetDamaged.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
            }
        }
        // Tăng thời gian tồn tại của Vortex lên 0.5s
        Collider2D[] vortexInRadius = Physics2D.OverlapCircleAll(transform.position, 2.0f);
        foreach (Collider2D vortex in vortexInRadius)
        {
            WizardVortex wizardVortex = vortex.GetComponent<WizardVortex>();
            if (wizardVortex != null)
            {
                wizardVortex.ExtendDuration(0.5f);
            }
        }
        // Sinh hiệu ứng
        //GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
        //spawnedSFX.transform.localScale = new Vector3(2f, 2f, 2f);
        //Destroy(spawnedSFX, 0.5f);
        if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showExplosion)
        {
            BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(Explosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
            if (explosionSFX != null)
            {
                explosionSFX.transform.position = this.transform.position;
                explosionSFX.transform.rotation = Quaternion.identity;
                explosionSFX.transform.localScale = new Vector3(2f, 2f, 2f);
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
            }
        }
        else if (ExplosionPooler.instance != null && GameSetting.instance != null && !GameSetting.instance._showExplosion)
        {
            BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(LowGraphic_Explosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
            if (explosionSFX != null)
            {
                explosionSFX.transform.position = this.transform.position;
                explosionSFX.transform.rotation = Quaternion.identity;
                explosionSFX.transform.localScale = new Vector3(4f, 4f, 4f);
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
            }
        }
        if (BulletPooler.instance != null)
        {
            BulletPooler.instance.ReturnBullet(this);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null && baseEnemy == enemy)
        {
            ExplodeOnImpact();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.0f); // cùng radius với OverlapCircleAll
    }

}
