using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBullet : BaseBullets
{
    private bool isBounced = false;
    private GameObject TargetWaypoint;
    private Vector3 TargetDirection;
    // Start is called before the first frame update
    void OnEnable()
    {
        isBounced = false;
        if (BulletPooler.instance != null)
        {
            BulletPooler.instance.StartCoroutine(BulletPooler.instance.ReturnBulletWithDelay(this, 1.25f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBounced) { Move(); }
        else { BouncedMove(); }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBounced)
        {
            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null && baseEnemy == enemy)
            {
                if (character != null)
                {
                    Summoner summoner = character as Summoner;
                    if (summoner != null)
                    {
                        summoner.Stack_for_Grave(character.GetDamage() < baseEnemy.GetHP() ? character.GetDamage() : baseEnemy.GetHP());
                    }
                    baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                }
                //GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
                //Destroy(spawnedSFX, 0.5f);
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
                if (character.GetLevel() < 3)
                {
                    if (BulletPooler.instance != null)
                    {
                        BulletPooler.instance.ReturnBullet(this);
                    }
                }
                else // Đạn nảy
                {
                    isBounced = true;
                    TargetWaypoint = enemy.Waypoints[enemy.Waypoint_CurrentIndex - 1];
                    TargetDirection = (TargetWaypoint.transform.position - this.transform.position).normalized;
                    // Cơ chế: chạm vào enemy sẽ lấy waypointcurrentindex của enemy
                }
            }
        }
        else
        {
            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null)
            {
                if (character != null)
                {
                    Summoner summoner = character as Summoner;
                    if (summoner != null)
                    {
                        summoner.Stack_for_Grave(character.GetDamage() < baseEnemy.GetHP() ? character.GetDamage() : baseEnemy.GetHP());
                    }
                    baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                }
                //GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
                //Destroy(spawnedSFX, 0.5f);
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
            }
        }
    }
    private void BouncedMove()
    {
        // Sau đó đạn sẽ di chuyển về phía waypointcurrentindex - 1
        if (TargetDirection != Vector3.zero)
        {
            transform.position += TargetDirection * BulletSpeed * Time.deltaTime;
        }
        else
        {
            Debug.Log("targetwaypoint = null");
            Move();
        }
    }
}
