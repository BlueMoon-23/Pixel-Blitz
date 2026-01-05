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
            case 1: // 4 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    break;
                }
            case 2: // 3 normal 3 quick
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 3));
                    break;
                }
            case 3: // 6 normal, 8 quick
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 6));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 8));
                    break;
                }
            case 4: // 8 enraged, 6 quick, 4 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 8));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 6));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    break;
                }
            case 5: // 15 enraged, 1 normal boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 1));
                    break;
                }
            case 6: // 20 enraged, 10 quick, 10 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 20));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 10));
                    break;
                }
            case 7: // 2 normal boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 2));
                    break;
                }
            case 8: // 15 hidden
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 15));
                    break;
                }
            case 9: // 10 hidden, 3 normal boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    break;
                }
            case 10: // 15 normal mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 15));
                    break;
                }
            case 11: // 10 normal mystery, 3 normal boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    break;
                }
            case 12: // 20 normal mystery, 4 normal boss, 1 skeleton boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 20));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 4));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1));
                    break;
                }
            case 13: // 10 hidden, 1 hidden boss, 10 hidden
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    break;
                }
            case 14: // 10 speed, 5 normal mystery, 2 skeleton boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 2));
                    break;
                }
            case 15: // 3 Normal Boss, 3 Skeleton Boss, 3 Hidden Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 3));
                    break;
                }
            case 16: // 15 Speed, 5 Hidden Boss, 15 Hidden
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 15));
                    break;
                }
            case 17: // 6 Normal Boss, 4 Skeleton Boss, 10 Speed, 1 Speedy Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 6));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 4));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 1));
                    break;
                }
            case 18: // 10 Speed, 3 Speedy Boss, 10 Speed
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    break;
                }
            case 19: // 20 normal mystery, 10 boss mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 20));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 10));
                    break;
                }
            case 20: // 15 Normal Knight
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 15));
                    break;
                }
            case 21: // 5 Normal Knight, 5 Skeleton Boss, 5 Normal Knight, 5 Boss Mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 5));
                    break;
                }
            case 22: // 5 Normal Knight, 5 Skeleton Knight, 10 Normal Knight, 10 Skeleton Knight
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 10));
                    break;
                }
            case 23: // 5 Hidden Boss, 10 Sleketon Knight, 1 King
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 1));
                    break;
                }
            case 24: // 15 Boss Mystery, 5 King
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    break;
                }
            case 25: // 15 Speed, 10 Speedy Boss, 1 Horse Rider
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    break;
                }
            case 26: // 1 Horse Rider, 6 Normal Knight, 7 Skeleton Knight, 3 King, 10 Hidden Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalKnight, 6));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 7));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 10));
                    break;
                }
            case 27: // 5 Heavy Knight, 10 Hidden Boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 10));
                    break;
                }
            case 28: // 5 Skeleton Knight, 3 King, 5 Heavy Knight, 1 Horse Rider
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    break;
                }
            case 29: // 5 Heavy Knight, 5 Boss Mystery, 5 Heavy Knight, 5 King
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    break;
                }
            case 30: // 5 King, 5 Heavy Knight, 1 Horse Rider, Final Boss, 5 Heavy Knight, 1 Horse Rider, 5 King
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.MediumFinalBoss, 1));
                    if (GameManager.instance != null)
                    {
                        GameManager.instance.BossHPGroup.SetActive(true);
                        GameManager.instance.BossName.text = "Abyssal Void Gatekeeper";
                    }
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HeavyKnight, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HorseRider, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.King, 5));
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
