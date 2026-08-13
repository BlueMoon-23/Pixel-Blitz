using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PulserLaser : BaseBullets
{
    private Pulser Owner;
    private LineRenderer lineRenderer;
    public GameObject HeadGun;
    private float TickClock = 0.1f;
    private float Tick = 0.1f;
    private void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    new private void Start()
    {
        base.Start();
        TickClock = character.GetCooldown();
        Tick = character.GetCooldown();
    }
    // update của base bullet xóa pulserlaser nè
    private void Update()
    {

    }
    // Update is called once per frame
    void LateUpdate() // LateUpdate để tính toán cho chính xác
    {
        Stretch();
    }
    protected void Stretch()
    {
        lineRenderer.SetPosition(0, HeadGun.transform.position);
        if (enemy != null)
        {
            lineRenderer.SetPosition(1, enemy.Center.transform.position + new Vector3(0, 0.25f, 0));
            float Angle = Mathf.Atan2(enemy.Center.transform.position.y - transform.position.y, enemy.Center.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetDirection = Quaternion.Euler(0, 0, Angle - 90f);
            transform.rotation = targetDirection;
            // Gây damage mỗi 0.1s
            TickClock += Time.deltaTime;
            if (TickClock >= Tick)
            {
                DealDamage();
                TickClock = 0f;
            }
        }
    }
    protected void DealDamage()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 50f);
        if (enemy != null)
        {
            if (character != null)
            {
                Owner = character as Pulser;
                if (Owner != null)
                {
                    if (!Owner.isReachingMaxPulse())
                    {
                        Owner.StackPulse(character.GetDamage() < enemy.GetHP() ? character.GetDamage() : enemy.GetHP());
                        enemy.TakeDamage(character, character.GetDamage(), character.canStrikethroughOrNot());
                    }
                    else
                    {
                        Owner.DrainPulse(2 * character.GetDamage());
                        // Tạo 1 vòng tròn collider, rồi gây damage lên toàn bộ enemy trong vòng này
                        Collider2D[] enemyInRadius = Physics2D.OverlapCircleAll(enemy.transform.position, 0.5f);
                        foreach (Collider2D enemy in enemyInRadius)
                        {
                            BaseEnemy enemyGetDamaged = enemy.GetComponent<BaseEnemy>();
                            if (enemyGetDamaged != null)
                            {
                                enemyGetDamaged.TakeDamage(character, 2 * character.GetDamage(), character.canStrikethroughOrNot());
                            }
                        }
                        ExplodeOnImpact(enemy.transform.position);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Khác ExplodeOnImpact của bullet nhé
    /// </summary>
    protected void ExplodeOnImpact(Vector3 ExplosionPosition)
    {
        if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showExplosion)
        {
            BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(BulletExplosionID);
            if (explosionSFX != null)
            {
                explosionSFX.transform.position = ExplosionPosition;
                explosionSFX.transform.rotation = Quaternion.identity;
                explosionSFX.transform.localScale = new Vector3(1f, 1f, 1f);
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
            }
        }
        else if (GameSetting.instance != null && GameSetting.instance != null && !GameSetting.instance._showExplosion)
        {
            BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(LowGraphic_BulletExplosionID);
            if (explosionSFX != null)
            {
                explosionSFX.transform.position = ExplosionPosition;
                explosionSFX.transform.rotation = Quaternion.identity;
                explosionSFX.transform.localScale = new Vector3(1f, 1f, 1f);
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
            }
        }
    }
}
