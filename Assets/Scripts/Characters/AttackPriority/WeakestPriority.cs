using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakestPriority : IAttackPriority
{
    public bool Priority(BaseEnemy enemy1, BaseEnemy enemy2, BaseCharacter character)
    {
        return enemy1.GetHP() > enemy2.GetHP();
    }
    public string PriorityName => "Weakest Enemy";
}
