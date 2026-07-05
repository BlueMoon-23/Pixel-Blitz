using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class RangerLaser : BaseBullets
{
    private LineRenderer lineRenderer;
    public GameObject HeadGun;
    private bool hasDealtDamage = false;
    private void OnEnable()
    {
        hasDealtDamage = false;
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {

    }

    // Update is called once per frame
    void LateUpdate() // LateUpdate để tính toán cho chính xác
    {
        if (!hasDealtDamage) StartCoroutine(Stretch());
    }
    protected IEnumerator Stretch()
    {
        lineRenderer.SetPosition(0, HeadGun.transform.position);
        if (enemy != null)
        {
            lineRenderer.SetPosition(1, enemy.Center.transform.position + new Vector3(0, 0.25f, 0));
            float Angle = Mathf.Atan2(enemy.Center.transform.position.y - transform.position.y, enemy.Center.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetDirection = Quaternion.Euler(0, 0, Angle - 90f);
            transform.rotation = targetDirection;
            yield return new WaitForSeconds(0.25f);
            DealDamage();
        }
        yield break;
    }
    protected void DealDamage()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 50f);
        if (enemy != null)
        {
            if (character != null && !hasDealtDamage)
            {
                enemy.TakeDamage(character, character.GetDamage(), character.canStrikethroughOrNot());
                hasDealtDamage = true;
                // Stun for 1s when level 4
                if (character.GetLevel() >= 4)
                {
                    if (enemy != null && enemy.GetHP() > 0) enemy.StartCoroutine(enemy.enemyEffect.GetStunned(1f));
                }
                //GameObject spawnedSFX = Instantiate(Explosion_SFX, enemy.transform.position, Quaternion.identity);
                //Destroy(spawnedSFX, 0.5f);
                BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(BulletExplosionID);
                if (explosionSFX != null)
                {
                    explosionSFX.transform.position = enemy.transform.position;
                    explosionSFX.transform.rotation = Quaternion.identity;
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
                }
                if (BulletPooler.instance != null)
                {
                    BulletPooler.instance.StartCoroutine(BulletPooler.instance.ReturnBulletWithDelay(this, 0.25f));
                }
            }
        }
    }
}
