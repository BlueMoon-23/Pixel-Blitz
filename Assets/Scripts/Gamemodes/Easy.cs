using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Easy : Gamemodes
{
    // Chứa logic sinh ra quái theo từng wave
    public override IEnumerator SpawnEnemyWave(int Wave) 
    {
        switch (Wave)
        {
            case 1:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4, 1));
                    break;
                }
            case 2:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 4, 1));
                    break;
                }
            case 3:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 5, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 5, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 3, 1));
                    break;
                }
            case 4:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 10, 1));
                    break;
                }
            case 5:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 6, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 1, 1));
                    break;
                }
            case 6:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 6, 1));
                    break;
                }
            case 7:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 6, 1));
                    break;
                }
            case 8:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 4, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 2, 1));
                    break;
                }
            case 9:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 8, 1));
                    break;
                }
            case 10:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 5, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 1, 1));
                    break;
                }
            case 11:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 8, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 2, 1));
                    break;
                }
            case 12: // wave farm
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 4, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 3, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1, 1));
                    break;
                }
            case 13: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 2, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10, 0.5f));
                    break;
                }
            case 14: // wave farm
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 7, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 6, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 7, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 6, 0.5f));
                    break;
                }
            case 15:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 7, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 1, 1));
                    break;
                }
            case 16: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 5, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 5, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 1, 1));
                    break;
                }
            case 17: // wave farm
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 4, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 8, 1));
                    break;
                }
            case 18:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 20, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 10, 1));
                    break;
                }
            case 19: // wave farm
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 10, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 10, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 5, 1));
                    break;
                }
            case 20:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 1, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 2, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 3, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 4, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 5, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 6, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 7, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 8, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 9, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.EasyFinalBoss, 1, 1));
                    if (BossManager.instance != null)
                    {
                        BossManager.instance.BossHPGroup.SetActive(true);
                        BossManager.instance.BossName.text = "Scarlet Knight";
                    }
                    break;
                }
            default:
                {
                    yield return null;
                    break;
                }
        }
    }
    public override void setGemReward(int wave, ref double BaseGem, ref int BonusGem, bool doVictory)
    {
        BaseGem = 2 * Mathf.Pow(wave, 1.25f);
        if (doVictory) { BonusGem = 138; }
    }
    public override void setCoinFormula(int wave, ref float Formula)
    {
        Formula = (int)Mathf.Pow(125 + 55 * wave, 1.2f);
    }
    public override int getMaxWave()
    {
        return 20;
    }
    public override int getDifficulty()
    {
        return 1;
    }
    public override Color getColor()
    {
        return new Color32(165, 255, 107, 255);
    }
}
