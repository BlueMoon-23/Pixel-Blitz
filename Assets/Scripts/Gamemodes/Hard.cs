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
            case 1: // 4 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    break;
                }
            case 2: // 4 normal 4 quick
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 4));
                    break;
                }
            case 3: // 10 Enraged
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 10));
                    break;
                }
            case 4: // 10 Enraged, 1 Normal Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 1));
                    break;
                }
            case 5: // 15 Enraged, 8 Quick, 8 Normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 8));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 8));
                    break;
                }
            case 6: // 10 Quick, 2 Normal Boss, 10 Normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 10));
                    break;
                }
            case 7: // 15 Hiddden
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 15));
                    break;
                }
            case 8: // 15 Enraged, 5 Armored
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 5));
                    break;
                }
            case 9: // 10 Hidden, 3 Normal Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    break;
                }
            case 10: // 10 Normal Mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 10));
                    break;
                }
            case 11: // 10 Hidden, 2 Necromancer
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 2));
                    break;
                }
            case 12: // 15 Normal Mystery, 3 Normal Boss, 1 Skeleton Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1));
                    break;
                }
            case 13: // 10 Hidden, 2 Hidden Boss, 10 Hidden
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    break;
                }
            case 14: // 10 Speed, 3 Necromancer, 2 Skeleton Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 2));
                    break;
                }
            case 15: // 10 Speed, 3 Necromancer, 2 Skeleton Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 2));
                    break;
                }
            case 16: // 15 Normal Mystery, 7 Boss Mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 7));
                    break;
                }
            case 17: // 15 Speed, 5 Hidden Boss, 2 Speedy Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 2));
                    break;
                }
            case 18: // 10 Armored, 5 Normal Knight, 5 Skeleton Knight
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 5));
                    break;
                }
            case 19: // 10 Normal Knight, 10 Skeleton Knight
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 10));
                    break;
                }
            case 20: // 15 Boss Mystery, 3 King, 5 Hidden Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5));
                    break;
                }
            case 21: // 15 Speed, 10 Speedy Boss, 1 Horse Rider
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    break;
                }
            case 22: // 5 Heavy Knight, 10 Speedy Boss, 5 King
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    break;
                }
            case 23: // 10 Heavy Knight, 10 Boss Mystery, 10 Heavy Knight, 5 King
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    break;
                }
            case 24: // 1 Horse Rider, 10 Normal Knight, 10 Skeleton Knight, 10 King, 15 Hidden Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 15));
                    break;
                }
            case 25: // 1 Mauler, 5 Hidden Boss, 5 Soul, 5 Necromancer, 8 Soul, 5 King, 1 Healer
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 8));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 1));
                    break;
                }
            case 26: // 10 Soul, 5 Ghoul, 1 Horse Rider, 1 Healer, 8 Heavy Knight, 1 Mauler, 1 Healer
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Ghost, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 8));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 1));
                    break;
                }
            case 27: // 5 Soul, 5 Ghoul, 5 King, 1 Horse Rider, 1 Charger, 10 King, 1 Healer, 3 Mauler
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Ghost, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Charger, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 3));
                    break;
                }
            case 28: // 5 Heavy Knight, 3 Mauler, 4 Healer, 5 King, 2 Mauler, 4 Healer, 2 Horse Rider
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 4));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 4));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 2));
                    break;
                }
            case 29: // 2 Healer, 3 Mauler, 2 Charger, 2 Healer, 2 Horse Rider, 2 Healer, 2 Charger
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Charger, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Charger, 2));
                    break;
                }
            case 30: // 10 Heavy Knight, 10 Ghoul, 10 Soul, 5 King, 5 Mauler, 1 Charger, Final Boss, 10 Boss Mystery, 5 King, 1 Horse Rider, 5 Healer
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Ghost, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Soul, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Mauler, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Charger, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HardFinalBoss, 1));
                    if (GameManager.instance != null)
                    {
                        GameManager.instance.BossHPGroup.SetActive(true);
                        GameManager.instance.BossName.text = "Secret Glamour Angel";
                    }
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Healer, 5));
                    break;
                }
            default:
                {
                    yield return null;
                    break;
                }
        }
    }
}
