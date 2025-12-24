using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class RangerLaser : BaseBullets
{
    private LineRenderer lineRenderer;
    public GameObject HeadGun;
    private bool hasDealtDamage = false;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        StartCoroutine(DestroyAfter1Second());
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
            Collider2D hit = Physics2D.OverlapCircle(enemy.Center.transform.position, 0.5f); // Khi chạm tạo 1 vùng collider hình tròn bán kính 0.5f
            if (hit != null)
            {
                BaseEnemy baseEnemy = hit.GetComponent<BaseEnemy>();
                if (baseEnemy != null && baseEnemy == enemy)
                {
                    if (character != null && !hasDealtDamage)
                    {
                        baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                        hasDealtDamage = true;
                        // Stun for 1s when level 4
                        if (character.GetLevel() >= 4)
                        {
                            baseEnemy.StartCoroutine(baseEnemy.GetStunned(1f));
                        }
                    }
                    GameObject spawnedSFX = Instantiate(Explosion_SFX, baseEnemy.transform.position, Quaternion.identity);
                    Destroy(spawnedSFX, 0.5f);
                    Destroy(this.gameObject, 0.5f);

                }
            }
            else
            {
                Destroy(this.gameObject, 0.5f);
            }
        }
    }
    private IEnumerator DestroyAfter1Second()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
        yield break;
    }
}
