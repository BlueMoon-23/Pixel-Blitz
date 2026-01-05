using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : BaseEnemy
{
    public GameObject MagicCircle;
    void Start()
    {
        // Move road
        if (WaypointManager.instance != null)
        {
            Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
        }
        isFinalBoss = false;
        StartCoroutine(HealEnemiesInCircle());
    }
    void Update()
    {
        Move();
        Die();
    }
    private IEnumerator HealEnemiesInCircle()
    {
        do
        {
            GameObject newMagicCircle = Instantiate(MagicCircle, transform.position, Quaternion.identity);
            Destroy(newMagicCircle, 1f);
            yield return new WaitForSeconds(1f);
        }
        while (true);
    }
}
