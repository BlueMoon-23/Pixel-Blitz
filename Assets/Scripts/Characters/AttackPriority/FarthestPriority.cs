using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarthestPriority : IAttackPriority
{
    public bool Priority(BaseEnemy enemy1, BaseEnemy enemy2, BaseCharacter character)
    {
        float Distance1 = Vector3.Distance(character.transform.position, enemy1.transform.position);
        float Distance2 = Vector3.Distance(character.transform.position, enemy2.transform.position);
        return Distance1 < Distance2;
    }
    public string PriorityName => "Farthest Enemy";
}

