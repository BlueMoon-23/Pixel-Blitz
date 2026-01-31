using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : BaseEnemy
{
    public GameObject LightningEffect;
    protected override IEnumerator ResetStats()
    {
        StartCoroutine(base.ResetStats());
        yield return null;
        isFinalBoss = false;
        SpeedUpAllEnemies();
    }
    private void SpeedUpAllEnemies()
    {
        if (SoundManager.Instance != null) { SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.ChargerSound); }
        GameObject newLightningEffect = Instantiate(LightningEffect, transform.position, Quaternion.identity);
        Destroy(newLightningEffect, 1.0f );
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.SpeedUpAllEnemies(1.5f);
        }
    }
}
