using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    // Enemy prefab nằm sẵn ở modemanager. phải duyệt qua mảng đó
    public Transform poolParent;
    private Dictionary<string, Stack<BaseEnemy>> pools;
    private int activeCount = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /*private List<BaseEnemy> EnemyList = new List<BaseEnemy>();
    public void AddEnemy(BaseEnemy enemy)
    {
        EnemyList.Add(enemy);
    }
    public void RemoveEnemy(BaseEnemy enemy)
    {
        EnemyList.Remove(enemy);
    }
    public int GetEnemyListCount()
    {
        return EnemyList.Count;
    }
    public void DestroyAllEnemies()
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            Destroy(EnemyList[i].gameObject);
        }
        EnemyList.Clear();
    }
    public void SpeedUpAllEnemies(float percent)
    {
        for (int i = 0;i < EnemyList.Count;i++)
        {
            EnemyList[i].GetSpeedUp(percent);
        }
    }*/
    private void Start()
    {
        poolParent = this.transform;
        pools = new Dictionary<string, Stack<BaseEnemy>>();
        if (ModeManager.instance != null)
        {
            for (int i = 0; i < ModeManager.instance.enemy_Prefabs.Count; i++)
            {
                CreateEnemy(ModeManager.instance.enemy_Prefabs[i]);
            }
        }
        activeCount = 0;
    }
    public BaseEnemy CreateEnemy(BaseEnemy enemy)
    {
        BaseEnemy newEnemy = Instantiate(enemy, poolParent);
        newEnemy.gameObject.SetActive(false);
        string key = newEnemy.name.Replace("(Clone)", "").Trim();
        if (pools.TryGetValue(key, out Stack<BaseEnemy> pool))
        {
            pool.Push(newEnemy);
        }
        else
        {
            pool = new Stack<BaseEnemy>();
            pool.Push(newEnemy);
            pools[key] = pool;
        }
        return newEnemy;
    }
    public BaseEnemy GetEnemy(BaseEnemy enemy)
    {
        Stack<BaseEnemy> pool = null;
        string key = enemy.name.Replace("(Clone)", "").Trim();
        if (pools.ContainsKey(key))
        {
            pool = pools[key];
        }
        else
        {
            CreateEnemy(enemy);
            pool = pools[key];
        }
        if (pool.Count > 0)
        {
            BaseEnemy newEnemy = pool.Pop();
            newEnemy.gameObject.SetActive(true);
            activeCount++;
            return newEnemy;
        }
        else
        {
            BaseEnemy newEnemy = CreateEnemy(enemy);
            newEnemy.gameObject.SetActive(true);
            pool.Pop();
            activeCount++;
            return newEnemy;
        }
    }
    public void ReturnEnemy(BaseEnemy enemy)
    {
        enemy.transform.SetParent(poolParent);
        enemy.gameObject.SetActive(false);
        string key = enemy.name.Replace("(Clone)", "").Trim();
        if (pools.TryGetValue(key, out Stack<BaseEnemy> pool))
        {
            if (!pool.Contains(enemy))
            {
                activeCount--;
                pool.Push(enemy);
            }
        }
    }
    public IEnumerator ReturnEnemyWithDelay(BaseEnemy enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnEnemy(enemy);
    }
    public void ClearPool()
    {
        foreach (var pool in pools.Values)
        {
            pool.Clear();
        }
        for (int i = poolParent.childCount - 1; i >= 0; i--)
        {
            if (poolParent.GetChild(i).gameObject != null)
            {
                Destroy(poolParent.GetChild(i).gameObject);
            }
        }
        pools.Clear();
    }
    public void SpeedUpAllEnemies(float percent)
    {
        // Speed up enemy ĐANG ACTIVE
        foreach(var pool in pools.Values)
        {
            foreach (var enemy in pool)
            {
                if (enemy != null && enemy.gameObject.activeInHierarchy)
                {
                    enemy.ModifySpeed(percent);
                }
            }
        }
    }
    public bool isEmptyEnemies()
    {
        return activeCount <= 0;
    }
}
