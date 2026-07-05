using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : BaseEnemy
{
    public GameObject[] Minion;
    public GameObject MagicCircle;
    protected override IEnumerator ResetStats()
    {
        StartCoroutine(base.ResetStats());
        yield return null;
        StartCoroutine(SpawnMinions());
    }
    IEnumerator SpawnMinions()
    {
        while (true)
        {
            GameObject magiccircle = Instantiate(MagicCircle, transform.position, Quaternion.identity);
            Destroy(magiccircle, 1f);
            int random_index = Random.Range(0, Minion.Length);
            for (int i = 0; i < 4; i++)
            {
                /*GameObject newEnemy = Instantiate(Minion[random_index], transform.position, Quaternion.identity);
                BaseEnemy enemy = newEnemy.GetComponent<BaseEnemy>();
                enemy.Waypoint_CurrentIndex = this.Waypoint_CurrentIndex;
                enemy.Waypoint_SelectedIndex = this.Waypoint_SelectedIndex;
                enemy.Distance = this.Distance;*/
                if (EnemyManager.instance != null)
                {
                    BaseEnemy baseEnemy = EnemyManager.instance.GetEnemy(Minion[random_index].GetComponent<BaseEnemy>());
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
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
