using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBullet : BaseBullets
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null && baseEnemy == enemy)
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
                // Sinh hiệu ứng
                GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
                spawnedSFX.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(spawnedSFX, 0.5f);
                Destroy(this.gameObject);
            }
        }
    }
}
