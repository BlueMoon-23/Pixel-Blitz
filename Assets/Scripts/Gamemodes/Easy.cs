using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easy : MonoBehaviour
{
    // Chứa logic sinh ra quái theo từng wave
    
    public enum EnemyName { Normal, Quick, Enraged, NormalBoss, Hidden, Armored, NormalMystery, Necromancer, NecromancerMinion, SkeletonBoss, HiddenBoss, Speed, SpeedyBoss, BossMystery}
    public List<EnemyEntry> enemyEntries = new List<EnemyEntry>();
    private Dictionary<EnemyName, BaseEnemy> EnemyList = new Dictionary<EnemyName, BaseEnemy>(14);
    public GameObject EnemySpawner;
    private void Awake()
    {
        for (int i = 0; i < enemyEntries.Count; i++)
        {
            if (EnemyList.ContainsKey(enemyEntries[i].Name))
            {
                Debug.Log("Co " + enemyEntries[i].Name + " roi");
            }
            else
            {
                EnemyList.Add(enemyEntries[i].Name, enemyEntries[i].Enemy_Prefab);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public BaseEnemy GetEnemyWithName(EnemyName name)
    {
        BaseEnemy enemyPrefab; // (1) Khai báo biến để chứa giá trị kết quả
        // (2) Sử dụng TryGetValue: Tra cứu và gán giá trị chỉ trong 1 lần
        if (EnemyList.TryGetValue(name, out enemyPrefab))
        {
            // Nếu tra cứu THÀNH CÔNG (Key tồn tại)
            return enemyPrefab;
        }
        else
        {
            // Nếu tra cứu THẤT BẠI (Key không tồn tại)
            Debug.LogError("Không tìm thấy EnemyName: " + name + ". Kiểm tra lại Inspector!");
            return null; // Trả về null để tránh lỗi treo ứng dụng
        }
    }
    private IEnumerator SpawnEnemyLayout(EnemyName name, int Quantity)
    {
        for (int i = 0; i < Quantity; i++)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(GetEnemyWithName(name).gameObject, EnemySpawner.transform.position, Quaternion.identity);
        }
    }
    public IEnumerator SpawnEnemyWave(int Wave) 
    {
        switch (Wave)
        {
            case 1: // 4 normal
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    break;
                }
            case 2: // 8 normal
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 8));
                    break;
                }
            case 3: // 4 normal, 4 quick
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 4));
                    break;
                }
            case 4: // 6 quick, 8 normal
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 6));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 8));
                    break;
                }
            case 5: // 8 enraged, 6 quick, 4 normal
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 8));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 6));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 4));
                    break;
                }
            case 6: // 20 enraged
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 20));
                    break;
                }
            case 7: // 10 enraged, 1 normal boss
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 1));
                    break;
                }
            case 8: // 20 enraged, 10 quick, 10 normal
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 20));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 10));
                    break;
                }
            case 9: // 2 normal boss
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 2));
                    break;
                }
            case 10: // 10 hidden
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    break;
                }
            case 11: // 10 hidden, 2 normal boss
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 2));
                    break;
                }
            case 12: // 4 armored
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 4));
                    break;
                }
            case 13: // 15 normal mystery
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 15));
                    break;
                }
            case 14: // 7 normal mystery, 3 normal boss
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 7));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 3));
                    break;
                }
            case 15: // 10 hidden, 1 necromancer
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 1));
                    break;
                }
            case 16: // 20 enraged, 2 normal boss, 1 skeleton boss
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 20));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 2));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1));
                    break;
                }
            case 17: // 3 normal boss, 1 necromancer, 5 enraged
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 1));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 5));
                    break;
                }
            case 18: // 10 hidden, 1 hidden boss, 10 hidden, 5 armored
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 1));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 5));
                    break;
                }
            case 19: // 10 speed, 5 normal mystery, 2 skeleton boss
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 5));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 2));
                    break;
                }
            case 20: // 5 hidden, 3 hidden boss, 15 hidden
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 3));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 15));
                    break;
                }
            case 21: // 3 normal boss, 1 skeleton boss, 5 speed, 5 normal boss, 1 hidden boss, 2 necromancer
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 3));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 5));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 1));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 2));
                    break;
                }
            case 22: // 15 speed, 3 speedy boss, 10 speed
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 15));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 3));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 10));
                    break;
                }
            case 23: // 20 normal mystery, 10 boss mystery
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 20));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 10));
                    break;
                }
            case 24: // 2 speedy boss, 4 boss mystery, 1 skeleton boss, 10 normal boss, 5 speed, 2 hidden boss,
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 2));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 4));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 1));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 5));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 2));
                    break;
                }
            case 25: // 1 normal, 2 quick, 3 enraged, 4 normal boss, 5 hidden, 6 armored, 7 normal mystery, 8 necromancer, 9 skeleton boss, 10 hidden boss, 11 speed, 12 speedy boss, 13 boss mystery
                {
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 1));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 2));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Enraged, 3));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalBoss, 4));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 5));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 6));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.NormalMystery, 7));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Necromancer, 8));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.SkeletonBoss, 9));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.HiddenBoss, 10));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.Speed, 11));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.SpeedyBoss, 12));
                    StartCoroutine(SpawnEnemyLayout(EnemyName.BossMystery, 13));
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
