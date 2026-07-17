using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeleportable
{
    bool isTeleporting { get; set; }
    EnemyPortal EndPortal { get; set; }
    void DoTeleport(EnemyPortal destinationPortal);
    void StopTeleport(EnemyPortal currentPortal);
}
