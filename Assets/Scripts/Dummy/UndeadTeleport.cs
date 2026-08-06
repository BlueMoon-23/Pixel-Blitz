using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadTeleport : MonoBehaviour, ITeleportable
{
    public bool isTeleporting { get; set; }
    public EnemyPortal EndPortal { get; set; }
    private SummonerUndead undead;
    void Start()
    {
        undead = GetComponent<SummonerUndead>();
        isTeleporting = false;
    }
    public void DoTeleport(EnemyPortal enemyPortal)
    {
        if (!undead) return;
        if (EndPortal == null)
        {
            EndPortal = enemyPortal;
            isTeleporting = true;
            undead.gameObject.transform.position = EndPortal.gameObject.transform.position;
            undead.TeleportToWaypoint(EndPortal.WaypointLocations);
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
