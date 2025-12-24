using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyFinalBoss : FinalBoss
{
    // Easy: 25k HP, dậm sàn gây choáng toàn map
    void Start()
    {
        // Move road
        if (WaypointManager.instance != null)
        {
            Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
        }
        isFinalBoss = true;
        StartCoroutine(StompGround());
    }
    void Update()
    {
        Move();
        Die();
    }
}
