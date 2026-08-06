using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleport : MonoBehaviour, ITeleportable
{
    public bool isTeleporting { get; set; }
    public EnemyPortal EndPortal { get; set; }
    private BaseEnemy enemy;
    void Start()
    {
        enemy = GetComponent<BaseEnemy>();
        isTeleporting = false;
    }
    public bool CanBeTargeted()
    {
        if (!gameObject.activeInHierarchy) return false;
        return !isTeleporting;
    }
    public void DoTeleport(EnemyPortal enemyPortal)
    {
        if (!enemy) return;
        if (EndPortal == null)
        {
            EndPortal = enemyPortal;
            isTeleporting = true;
            enemy.gameObject.transform.position = EndPortal.gameObject.transform.position;
            enemy.TeleportToWaypoint(EndPortal.WaypointLocations);
        }
    }
    public void StopTeleport(EnemyPortal enemyPortal)
    {
        if (EndPortal != null && EndPortal == enemyPortal)
        {
            EndPortal = null;
            isTeleporting = false;
        }
    }
}
