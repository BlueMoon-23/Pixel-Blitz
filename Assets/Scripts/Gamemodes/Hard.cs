using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hard : Gamemodes
{
    // Chứa logic sinh ra quái theo từng wave
    public override IEnumerator SpawnEnemyWave(int Wave)
    {
        switch (Wave)
        {
            case 1:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 6, 1f));
                    break;
                }
            case 2:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 5, 1f));
                    break;
                }
            case 3:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 4, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 4, 1f));
                    break;
                }
            case 4:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 10, 1f));
                    break;
                }
            case 5:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 5, 1f));
                    break;
                }
            case 6:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 10, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 7, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 7, 0.75f));
                    break;
                }
            case 7:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 8, 1f));
                    break;
                }
            case 8:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 4, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 2, 1f));
                    break;
                }
            case 9:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 6, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 5, 0.75f));
                    break;
                }
            case 10:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 10, 1f));
                    break;
                }
            case 11:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 2, 1f));
                    break;
                }
            case 12:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 5, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1, 1f));
                    break;
                }
            case 13:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10, 0.75f));
                    break;
                }
            case 14:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 4, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1, 1f));
                    break;
                }
            case 15:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 5, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 5, 0.75f));
                    break;
                }
            case 16:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10, 0.5f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 3, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5, 0.75f));
                    break;
                }
            case 17:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 2, 1f));
                    break;
                }
            case 18:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 8, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 6, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 6, 1f));
                    break;
                }
            case 19:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 5 , 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 5, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 1, 1f));
                    break;
                }
            case 20:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 15, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5, 1f));
                    break;
                }
            case 21:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 4, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 5, 1f));
                    break;
                }
            case 22:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5, 1f));
                    break;
                }
            case 23:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 4, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 4, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Ghost, 4, 0.75f));
                    break;
                }
            case 24:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 2, 1f));
                    break;
                }
            case 25:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 4, 1f));
                    break;
                }
            case 26:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 4, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 6, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Ghost, 6, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 2, 1f));
                    break;
                }
            case 27:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Ghost, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Charger, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 3, 1f));
                    break;
                }
            case 28:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 4, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 3, 1f));
                    break;
                }
            case 29:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Charger, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Charger, 2, 1f));
                    break;
                }
            case 30:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 4, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Ghost, 4, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 4, 0.75f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Charger, 1, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HardFinalBoss, 1, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 2, 1f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1, 1.25f));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 3, 1f));
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
        BaseGem = 2.75 * Mathf.Pow(wave, 1.6f);
        if (doVictory) { BonusGem = 50; }
    }
    public override void setCoinFormula(int wave, ref float Formula)
    {
        Formula = (int)Mathf.Pow(125 + 55 * wave, 1.2f);
    }
    public override int getMaxWave()
    {
        return 30;
    }
    public override int getDifficulty()
    {
        return 3;
    }
    public override Color getColor()
    {
        return new Color32(255, 100, 76, 255);
    }
}
