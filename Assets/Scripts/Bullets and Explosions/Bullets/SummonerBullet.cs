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
    protected override void ExplodeOnImpact()
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
        if (!isBounced)
        {
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
                if (WaypointManager.instance != null)
                {
                    foreach (WaypointInformation Path in WaypointManager.instance.List_of_Waypoints)
                    {
                        for (int i = 0; i < Path.Waypoints.Length - 1; i++)
                        {
                            if (x_Between_2_Waypoints(i, transform.position.x, Path.Waypoints) && y_Between_2_Waypoints(i, transform.position.y, Path.Waypoints))
                            {
                                TargetWaypoint = Path.Waypoints[i];
                                break;
                            }
                        }
                    }
                }
                TargetDirection = (TargetWaypoint.transform.position - this.transform.position).normalized;
                // Cơ chế: chạm vào enemy sẽ lấy waypointcurrentindex của enemy
            }
        }
    }
    private bool x_Between_2_Waypoints(int index, float x_position, GameObject[] Waypoints)
    {
        return ((Waypoints[index].transform.position.x <= x_position && x_position <= Waypoints[index + 1].transform.position.x) || (Waypoints[index].transform.position.x >= x_position && x_position >= Waypoints[index + 1].transform.position.x));
    }
    private bool y_Between_2_Waypoints(int index, float y_position, GameObject[] Waypoints)
    {
        return ((Waypoints[index].transform.position.y <= y_position && y_position <= Waypoints[index + 1].transform.position.y) || (Waypoints[index].transform.position.y >= y_position && y_position >= Waypoints[index + 1].transform.position.y));
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
                ExplodeOnImpact();
            }
        }
        else
        {
            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null) // khác nhau ở đây nè
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
                ExplodeOnImpact();
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
