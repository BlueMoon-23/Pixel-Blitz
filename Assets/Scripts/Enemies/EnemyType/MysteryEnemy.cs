using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryEnemy : BaseEnemy
{
    public GameObject[] EnemyList;
    // Update is called once per frame
    protected override void Die()
    {
        if (HP <= 0)
        {
            int i = Random.Range(0, EnemyList.Length);
            /*GameObject newEnemy = Instantiate(EnemyList[i], transform.position, Quaternion.identity);
            BaseEnemy enemy = newEnemy.GetComponent<BaseEnemy>();
            enemy.Waypoint_CurrentIndex = this.Waypoint_CurrentIndex;
            enemy.Waypoint_SelectedIndex = this.Waypoint_SelectedIndex;
            enemy.Distance = this.Distance;*/
            if (EnemyManager.instance != null)
            {
                BaseEnemy baseEnemy = EnemyManager.instance.GetEnemy(EnemyList[i].GetComponent<BaseEnemy>());
                if (baseEnemy != null)
                {
                    baseEnemy.isSummoned = true;
                    baseEnemy.transform.position = this.transform.position;
                    baseEnemy.transform.rotation = Quaternion.identity;
                    baseEnemy.Waypoint_CurrentIndex = this.Waypoint_CurrentIndex;
                    baseEnemy.Waypoint_SelectedIndex = this.Waypoint_SelectedIndex;
                    baseEnemy.Distance = this.Distance;
                }
            }
        }
        base.Die();
    }
}
