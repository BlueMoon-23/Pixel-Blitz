using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryEnemy : BaseEnemy
{
    public GameObject[] EnemyList;
    void Start()
    {
        // Move road
        if (WaypointManager.instance != null)
        {
            Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Die();
    }
    private void OnDestroy()
    {
        int i = Random.Range(0, EnemyList.Length);
        GameObject newEnemy = Instantiate(EnemyList[i], transform.position, Quaternion.identity);
        BaseEnemy enemy = newEnemy.GetComponent<BaseEnemy>();
        enemy.Waypoint_CurrentIndex = this.Waypoint_CurrentIndex;
        enemy.Waypoint_SelectedIndex = this.Waypoint_SelectedIndex;
        enemy.Distance = this.Distance;
        if (EnemyManager.instance != null) { EnemyManager.instance.RemoveEnemy(this); }
    }
}
