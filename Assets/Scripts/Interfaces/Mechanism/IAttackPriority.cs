using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackPriority
{
    public bool Priority(BaseEnemy enemy1, BaseEnemy enemy2, BaseCharacter character);
    public bool IsRandom => false; // bool IsRandom() { return false; }
    public string PriorityName => "";
}
