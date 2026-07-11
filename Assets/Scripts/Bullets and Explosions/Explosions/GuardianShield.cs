using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianShield : MonoBehaviour
{
    public Guardian guardian { get; set; }
    private List<BaseEnemy> EnemiesInAura = new List<BaseEnemy>();
    private void OnEnable()
    {
        StartCoroutine(StayModifySpeed());
    }
    private void OnDisable()
    {
        // Xóa phần tử phải duyệt ngược
        for (int i = EnemiesInAura.Count - 1; i >= 0; i--)
        {
            BaseEnemy enemy = EnemiesInAura[i];
            // Phòng hờ quái bị hủy (Destroy) đột ngột
            if (enemy == null)
            {
                EnemiesInAura.RemoveAt(i);
                continue;
            }
            if (enemy.ContainsModifier(0.75f))
            {
                enemy.RemoveModifySpeed(0.75f);
            }
        }
    }
    private IEnumerator StayModifySpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (guardian.GetLevel() >= 1)
            {
                // Xóa phần tử phải duyệt ngược
                for (int i = EnemiesInAura.Count - 1; i >= 0; i--)
                {
                    BaseEnemy enemy = EnemiesInAura[i];
                    // Phòng hờ quái bị hủy (Destroy) đột ngột
                    if (enemy == null)
                    {
                        EnemiesInAura.RemoveAt(i);
                        continue;
                    }
                    if (!enemy.ContainsModifier(0.75f))
                    {
                        enemy.ModifySpeed(0.75f);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProtectPlayerSide(collision);
        BlockProjectile(collision);
        SlowEnemies(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveProtect(collision);
        RemoveSlowEnemies(collision);
    }
    private void BlockProjectile(Collider2D collision)
    {
        // Chặn đạn
        HitCharacterExplosion hitCharacterExplosion = collision.GetComponent<HitCharacterExplosion>();
        MaulerAim maulerAim = collision.GetComponent<MaulerAim>();
        if (maulerAim != null)
        {
            Destroy(maulerAim.gameObject);
            if (guardian != null && guardian.GetLevel() >= 3) guardian.ShieldExplode(); // gethit sẽ bị trừ lố không đáng
        }
        else if (hitCharacterExplosion != null)
        {
            ExplosionPooler.instance.ReturnExplosion(hitCharacterExplosion);
            if (guardian != null) guardian.ShieldGetHit();
        }
    }
    private void ProtectPlayerSide(Collider2D collision)
    {
        IStunnable stunnable = null;
        ISide side = null;
        // Tìm ngay trên chính cái Collider va chạm (dummy)
        stunnable = collision.GetComponent<IStunnable>();
        side = collision.GetComponent<ISide>();
        // Nếu không thấy (character), leo lên parent rồi quét xuống các component con
        if (stunnable == null && collision.transform.parent != null)
        {
            stunnable = collision.transform.parent.GetComponentInChildren<IStunnable>();
        }
        if (side == null && collision.transform.parent != null)
        {
            side = collision.transform.parent.GetComponentInChildren<ISide>();
        }
        // Xử lý
        if (stunnable != null && side != null && side.IsPlayerSide())
        {
            stunnable.SetStunImmunity();
        }
    }
    private void RemoveProtect(Collider2D collision)
    {
        IStunnable stunnable = null;
        ISide side = null;
        stunnable = collision.GetComponent<IStunnable>();
        side = collision.GetComponent<ISide>();
        if (stunnable == null && collision.transform.parent != null)
        {
            stunnable = collision.transform.parent.GetComponentInChildren<IStunnable>();
        }
        if (side == null && collision.transform.parent != null)
        {
            side = collision.transform.parent.GetComponentInChildren<ISide>();
        }
        if (stunnable != null && side != null && side.IsPlayerSide())
        {
            stunnable.RemoveStunImmunity();
        }
    }
    private void SlowEnemies(Collider2D collision)
    {
        BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            EnemiesInAura.Add(enemy);
            if (guardian.GetLevel() >= 1) enemy.ModifySpeed(0.75f);
        }
    }
    private void RemoveSlowEnemies(Collider2D collision)
    {
        BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            EnemiesInAura.Remove(enemy);
            if (guardian.GetLevel() >= 1) enemy.RemoveModifySpeed(0.75f);
        }
    }
}

