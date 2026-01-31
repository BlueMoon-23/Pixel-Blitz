using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : BaseEnemy
{
    public GameObject MagicCircle;
    protected override IEnumerator ResetStats()
    {
        StartCoroutine(base.ResetStats());
        yield return null;
        isFinalBoss = false;
        StartCoroutine(HealEnemiesInCircle());
    }
    private IEnumerator HealEnemiesInCircle()
    {
        do
        {
            /*GameObject newMagicCircle = Instantiate(MagicCircle, transform.position, Quaternion.identity);
            Destroy(newMagicCircle, 1f);*/
            if (ExplosionPooler.instance != null)
            {
                BaseExplosion newMagicCircle = ExplosionPooler.instance.GetExplosion(MagicCircle.GetComponent<BaseExplosion>().ExplosionID);
                if (newMagicCircle != null)
                {
                    newMagicCircle.transform.position = this.transform.position;
                    newMagicCircle.transform.rotation = Quaternion.identity;
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(newMagicCircle, 1f));
                }
            }
            yield return new WaitForSeconds(2f);
        }
        while (true);
    }
}
