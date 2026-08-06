using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPriority : IAttackPriority
{
    public bool Priority(BaseEnemy enemy1, BaseEnemy enemy2, BaseCharacter character)
    {
        return enemy1.Distance < enemy2.Distance;
    }
    public bool IsRandom => true;
    public string PriorityName => "Random";
}
