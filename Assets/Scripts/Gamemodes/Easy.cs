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
            case 1: // 4 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    break;
                }
            case 2: // 8 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 8));
                    break;
                }
            case 3: // 4 normal, 4 quick
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    yield return (SpawnEnemyLayout(EnemyName.Quick, 4));
                    break;
                }
            case 4: // 6 quick, 8 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 6));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 8));
                    break;
                }
            case 5: // 8 enraged, 6 quick, 4 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 8));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 6));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    break;
                }
            case 6: // 15 enraged
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 15));
                    break;
                }
            case 7: // 10 enraged, 1 normal boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 1));
                    break;
                }
            case 8: // 20 enraged, 10 quick, 10 normal
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 20));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 10));
                    break;
                }
            case 9: // 2 normal boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 2));
                    break;
                }
            case 10: // 15 hidden
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 15));
                    break;
                }
            case 11: // 10 hidden, 3 normal boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    break;
                }
            case 12: // 6 armored
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 6));
                    break;
                }
            case 13: // 15 normal mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 15));
                    break;
                }
            case 14: // 10 normal mystery, 3 normal boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    break;
                }
            case 15: // 20 hidden, 2 necromancer
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 20));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 2));
                    break;
                }
            case 16: // 20 enraged, 4 normal boss, 1 skeleton boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 20));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 4));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1));
                    break;
                }
            case 17: // 3 normal boss, 3 necromancer, 5 normal mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 5));
                    break;
                }
            case 18: // 15 hidden, 1 hidden boss, 15 hidden, 10 armored
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 10));
                    break;
                }
            case 19: // 10 speed, 5 necromancer, 2 skeleton boss
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 2));
                    break;
                }
            case 20: // 5 hidden, 3 hidden boss, 10 hidden, 2 hidden boss, 5 hidden
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5));
                    break;
                }
            case 21: // 6 normal boss, 3 skeleton boss, 10 speed, 3 hidden boss, 2 necromancer
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 6));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 2));
                    break;
                }
            case 22: // 15 speed, 3 speedy boss, 10 speed
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 15));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    break;
                }
            case 23: // 20 normal mystery, 15 boss mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 20));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 15));
                    break;
                }
            case 24: // 3 speedy boss, 5 boss mystery, 2 skeleton boss, 10 normal boss, 5 speed, 4 hidden boss,
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 4));
                    break;
                }
            case 25: // 1 normal, 2 quick, 3 enraged, 4 normal boss, 5 hidden, 6 armored, 7 normal mystery, 8 necromancer, 9 skeleton boss, 10 hidden boss, 11 speed, 12 speedy boss, 13 boss mystery
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 2));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 3));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 4));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 6));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 7));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 8));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 9));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 10));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 11));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 12));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 13));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.EasyFinalBoss, 1));
                    if (GameManager.instance != null)
                    {
                        GameManager.instance.BossHPGroup.SetActive(true);
                        GameManager.instance.BossName.text = "Scarlet Knight";
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
}
