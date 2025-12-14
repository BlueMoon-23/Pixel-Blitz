using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private List<BaseEnemy> EnemyList = new List<BaseEnemy>();
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
}
