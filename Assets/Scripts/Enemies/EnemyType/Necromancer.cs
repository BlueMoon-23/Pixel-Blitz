using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : BaseEnemy
{
    public GameObject[] Minion;
    public GameObject MagicCircle;
    private Coroutine SpawnCoroutine;
    protected override IEnumerator ResetStats()
    {
        StartCoroutine(base.ResetStats());
        yield return null;
        SpawnCoroutine = StartCoroutine(SpawnMinions());
    }
    IEnumerator SpawnMinions()
    {
        while (true)
        {
            // chỉ summon nếu đang không teleport
            if (!CanBeTargeted())
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            GameObject magiccircle = Instantiate(MagicCircle, transform.position, Quaternion.identity);
            Destroy(magiccircle, 1f);
            int random_index = Random.Range(0, Minion.Length);
            for (int i = 0; i < 4; i++)
            {
                // Check lại NGAY TRƯỚC mỗi lần summon, vì teleport có thể xảy ra
                // giữa các lần yield trong chính vòng for này
                while (!CanBeTargeted())
                {
                    yield return new WaitForSeconds(0.5f);
                }
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
    private void OnDisable()
    {
        if (SpawnCoroutine != null)
        {
            StopCoroutine(SpawnCoroutine);
            SpawnCoroutine = null;
        }
    }
}
