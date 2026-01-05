using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketeerBullet : BaseBullets
{
    public GameObject RocketExplosion;
    public float ExplosionRadius;
    private bool isCluster = false;
    private Vector3 ClusterDirection;
    //
    private float x_direction = 0f;
    private float y_direction = 0f;
    void Start()
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
                        GameObject ClusterRocket = Instantiate(this.gameObject, this.transform.position, Quaternion.identity);
                        // Cài lại thông số cho đạn
                        RocketeerBullet ClusterRocketBullet = ClusterRocket.GetComponent<RocketeerBullet>();
                        ClusterRocketBullet.BulletSpeed = 3.125f;
                        ClusterRocketBullet.SetCharacter(character);
                        ClusterRocketBullet.isCluster = true;
                        ClusterRocketBullet.ClusterDirection = newDirection;
                        Destroy(ClusterRocket, 1f);
                    }
                }
                // Sinh đạn trước khi nổ (bị destroy)
                // Không bỏ logic sinh thêm đạn trong ondestroy
                Explode();
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
    private void OnDestroy()
    {
        if (isCluster)
        {
            Explode();
        }
    }
    private void Explode()
    {
        GameObject explosion = Instantiate(RocketExplosion, this.transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(ExplosionRadius, ExplosionRadius, ExplosionRadius);
        Destroy(explosion, 0.1f ); // Không có lệnh này là explosion sẽ chờ mục tiêu bước vào ở cluster sau khi nổ bị thừa
        RocketExplosion baseBullets = explosion.GetComponent<RocketExplosion>();
        baseBullets.SetCharacter(character);
        if (character.GetLevel() >= 4 && isCluster) { baseBullets.ClusterDamage = 100; }
        GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
        spawnedSFX.transform.localScale = new Vector3(ExplosionRadius, ExplosionRadius, ExplosionRadius);
        Destroy(spawnedSFX, 0.5f);
        Destroy(this.gameObject); // thêm 0.5f vô để lệnh for đằng dưới thực hiện cho hết => không cần, đổi thứ tự thực thi là xong
    }
}
