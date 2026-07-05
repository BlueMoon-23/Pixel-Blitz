using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketeerBullet : BaseBullets
{
    public float ExplosionRadius;
    public bool isCluster = false;
    private Vector3 ClusterDirection;
    //
    private float x_direction = 0f;
    private float y_direction = 0f;
    void OnEnable()
    {
        StartCoroutine(SetupAtStart());
    }
    private IEnumerator SetupAtStart()
    {
        yield return null;
        if (character != null)
        {
            Rocketeer rocketeer = character.GetComponent<Rocketeer>();
            if (rocketeer != null)
            {
                if (isCluster)
                {
                    ExplosionRadius = 1.5f;
                }
                else
                {
                    ExplosionRadius = rocketeer.GetExplosionRadius();
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isCluster)
        {
            ClusterMove(ClusterDirection);
        }
        else
        {
            Move();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCluster)
        {
            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null && baseEnemy == enemy)
            {
                ExplodeOnImpact();
            }
        }
    }
    private void ClusterMove(Vector3 clusterDirection)
    {
        if (clusterDirection == new Vector3(1f, 1f, 0f).normalized)
        {
            // Di chuyển
            x_direction += BulletSpeed * Time.deltaTime;
            // Đi theo y = -2.5x^2 + 5.5x
            y_direction = (float)(-2.5 * x_direction * x_direction + 5.5 * x_direction);
        }
        else if (clusterDirection == new Vector3(-1f, 1f, 0f).normalized)
        {
            // Di chuyển
            x_direction -= BulletSpeed * Time.deltaTime;
            // Đi theo y = -2.5x^2 - 5.5x
            y_direction = (float)(-2.5 * x_direction * x_direction - 5.5 * x_direction);
        }
        else if (clusterDirection == new Vector3(1f, -1f, 0f).normalized)
        {
            // Di chuyển
            x_direction += BulletSpeed * Time.deltaTime;
            // Đi theo y = -2.5x^2 + 3.5x
            y_direction = (float)(-2.5 * x_direction * x_direction + 2.5 * x_direction);
        }
        else if (clusterDirection == new Vector3(-1f, -1f, 0f).normalized)
        {
            // Di chuyển
            x_direction -= BulletSpeed * Time.deltaTime;
            // Đi theo y = -2.5x^2 - 3.5x
            y_direction = (float)(-2.5 * x_direction * x_direction - 2.5 * x_direction);
        }
        else
        {
            y_direction = clusterDirection.y;
            x_direction = clusterDirection.x;
        }
        float angle = Mathf.Atan2(y_direction, x_direction) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        this.transform.position += transform.up * BulletSpeed * Time.deltaTime;
    }
    protected override void ExplodeOnImpact()
    {
        // Sinh ra 4 đạn con nổ 4 bên nếu character có level = 4
        if (character.GetLevel() >= 4 && !isCluster)
        {
            for (int i = 0; i < 4; i++)
            {
                // 0: -1 -1
                // 1: -1 1
                // 2: 1 -1
                // 3: 1 1
                float x = i < 2 ? -1 : 1;
                float y = i % 2 == 0 ? -1 : 1;
                Vector3 newDirection = new Vector3(x, y, 0).normalized;
                if (BulletPooler.instance != null)
                {
                    BaseBullets ClusterRocket = BulletPooler.instance.GetBullet(this.BulletID);
                    if (ClusterRocket != null)
                    {
                        ClusterRocket.transform.position = this.transform.position;
                        ClusterRocket.transform.rotation = Quaternion.identity;
                        // Cài lại thông số cho đạn
                        RocketeerBullet ClusterRocketBullet = ClusterRocket.GetComponent<RocketeerBullet>();
                        ClusterRocketBullet.BulletSpeed = 3.125f;
                        ClusterRocketBullet.SetCharacter(character);
                        ClusterRocketBullet.isCluster = true;
                        ClusterRocketBullet.ClusterDirection = newDirection;
                        if (BulletPooler.instance != null)
                        {
                            BulletPooler.instance.StartCoroutine(BulletPooler.instance.DestroyCluster(ClusterRocketBullet, 1.0f));
                        }
                    }
                }
            }
        }
        // Sinh đạn trước khi nổ (bị destroy)
        // Không bỏ logic sinh thêm đạn trong ondestroy
        //
        // Tạo 1 vòng tròn collider, rồi gây damage lên toàn bộ enemy trong vòng này
        Collider2D[] enemyInRadius = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);
        foreach (Collider2D enemy in enemyInRadius)
        {
            BaseEnemy enemyGetDamaged = enemy.GetComponent<BaseEnemy>();
            if (enemyGetDamaged != null)
            {
                if (character != null)
                {
                    if (!isCluster)
                    {
                        enemyGetDamaged.TakeDamage(character, character.GetDamage(), character.canStrikethroughOrNot());
                    }
                    else
                    {
                        enemyGetDamaged.TakeDamage(character, 50, character.canStrikethroughOrNot());
                    }
                }
            }
        }
        //GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
        //spawnedSFX.transform.localScale = new Vector3(ExplosionRadius, ExplosionRadius, ExplosionRadius);
        //Destroy(spawnedSFX, 0.5f);
        if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showExplosion)
        {
            BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(BulletExplosionID);
            if (explosionSFX != null)
            {
                explosionSFX.transform.position = this.transform.position;
                explosionSFX.transform.rotation = Quaternion.identity;
                explosionSFX.transform.localScale = new Vector3(ExplosionRadius, ExplosionRadius, ExplosionRadius);
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
            }
        }
        else if (ExplosionPooler.instance != null && GameSetting.instance != null && !GameSetting.instance._showExplosion)
        {
            BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(LowGraphic_BulletExplosionID);
            if (explosionSFX != null)
            {
                explosionSFX.transform.position = this.transform.position;
                explosionSFX.transform.rotation = Quaternion.identity;
                explosionSFX.transform.localScale = new Vector3(2 * ExplosionRadius, 2 * ExplosionRadius, 2 * ExplosionRadius);
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
            }
        }
        // khôi phục cài đặt gốc
        BulletSpeed = 30f;
        isCluster = false;
        x_direction = 0f;
        y_direction = 0f;
        if (BulletPooler.instance != null)
        {
            BulletPooler.instance.ReturnBullet(this);
        }
    }
    public void Explode()
    {
        ExplodeOnImpact();
    }
}
