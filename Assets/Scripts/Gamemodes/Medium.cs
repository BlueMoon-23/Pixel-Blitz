using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medium : Gamemodes
{
    // Chứa logic sinh ra quái theo từng wave
    public override IEnumerator SpawnEnemyWave(int Wave)
    {
        switch (Wave)
        {
            case 1: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 6, 1));
                    break;
                }
            case 2: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 6, 0.75f));
                    break;
                }
            case 3: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 6, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 4, 1));
                    break;
                }
            case 4: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 7, 1));
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
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10, 0.75f));
                    break;
                }
            case 7: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 7, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 1, 0.75f));
                    break;
                }
            case 8: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 10, 0.75f));
                    break;
                }
            case 9: 
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 2, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10, 0.5f));
                    break;
                }
            case 10:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 8, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 6, 0.75f));
                    break;
                }
            case 11:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 10, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1, 0.75f));
                    break;
                }
            case 12:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 1, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5, 0.5f));
                    break;
                }
            case 13:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 10, 0.5f));
                    break;
                }
            case 14:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 10, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 2, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 10, 0.75f));
                    break;
                }
            case 15:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 6, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 2, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 6, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5, 0.75f));
                    break;
                }
            case 16:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 3, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 5, 0.75f));
                    break;
                }
            case 17:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 2, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5, 0.75f));
                    break;
                }
            case 18:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 12, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 15, 0.5f));
                    break;
                }
            case 19:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 10, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 10, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 10, 0.5f));
                    break;
                }
            case 20:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 10, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 10, 0.75f));
                    break;
                }
            case 21:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 10, 1.25f));
                    break;
                }
            case 22:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1, 1.25f));
                    break;
                }
            case 23:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 6, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1, 1.25f));
                    break;
                }
            case 24:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 8, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1, 1.25f));
                    break;
                }
            case 25:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 3, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.MediumFinalBoss, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 3, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3, 1f));
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
        BaseGem = 3.8 * Mathf.Pow(wave, 1.5f);
        if (doVictory) { BonusGem = 25; }
    }
    public override void setCoinFormula(int wave, ref float Formula)
    {
        Formula = (int)Mathf.Pow(125 + 55 * wave, 1.2f);
    }
    public override int getMaxWave()
    {
        return 25;
    }
    public override int getDifficulty()
    {
        return 2;
    }
    public override Color getColor()
    {
        return new Color32(255, 187, 73, 255);
    }
}
