using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gamemodes : MonoBehaviour
{
    public enum EnemyName { Normal, Quick, Enraged, NormalBoss, Hidden, Armored, NormalMystery, Necromancer, NecromancerMinion, SkeletonBoss, HiddenBoss, Speed, SpeedyBoss, BossMystery, EasyFinalBoss, NormalKnight, SkeletonKnight, King, HeavyKnight, HorseRider, MediumFinalBoss, Soul, Ghost, Healer, Charger, Mauler, HardFinalBoss }
    public List<EnemyEntry> enemyEntries = new List<EnemyEntry>();
    protected Dictionary<EnemyName, BaseEnemy> EnemyList = new Dictionary<EnemyName, BaseEnemy>();
    public string FinalBossName;
    // EnemySpawner
    protected GameObject EnemySpawner;
    // Thêm interval làm cho doSkip vô tình làm lố
    protected bool finished_spawningEnemies = false;
    protected void Awake()
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
    protected IEnumerator SpawnEnemyLayout(EnemyName name, int Quantity, float interval)
    {
        finished_spawningEnemies = false;
        for (int i = 0; i < Quantity; i++)
        {
            yield return new WaitForSeconds(interval);
            // Set lại EnemySpawner một cách tự động
            if (Win_Lose.instance.Defeated)
            {
                StopAllCoroutines();
                yield break;
            }
            if (WaypointManager.instance != null)
            {
                GameObject[] Waypoints = WaypointManager.instance.GetWaypoints(out int Waypoints_index);
                EnemySpawner = Waypoints[0];
                /*GameObject newEnemy = Instantiate(GetEnemyWithName(name).gameObject, EnemySpawner.transform.position, Quaternion.identity);
                BaseEnemy baseEnemy = newEnemy.GetComponent<BaseEnemy>();
                if (baseEnemy != null)
                {
                    baseEnemy.Waypoint_SelectedIndex = Waypoints_index;
                }*/
                if (EnemyManager.instance != null)
                {
                    BaseEnemy baseEnemy = EnemyManager.instance.GetEnemy(GetEnemyWithName(name));
                    if (baseEnemy != null)
                    {
                        baseEnemy.isSummoned = false;
                        baseEnemy.transform.position = EnemySpawner.transform.position;
                        baseEnemy.transform.rotation = Quaternion.identity;
                        baseEnemy.Waypoint_SelectedIndex = Waypoints_index;
                    }
                }
            }
        }
        yield return new WaitForSeconds(interval);
        finished_spawningEnemies = true;
    }
    public virtual IEnumerator SpawnEnemyWave(int Wave)
    {
        yield break;
    }
    public bool isFinished_spawning() { return finished_spawningEnemies; }
    public abstract void setGemReward(int wave, ref double BaseGem, ref int BonusGem, bool doVictory);
    public abstract void setCoinFormula(int wave, ref float Formula);
    public abstract int getMaxWave();
    public abstract Color getColor();
    public abstract int getDifficulty();
}
