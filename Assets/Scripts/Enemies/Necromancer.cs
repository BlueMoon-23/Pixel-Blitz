using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : BaseEnemy
{
    public GameObject Minion;
    public GameObject MagicCircle;
    void Start()
    {
        // Move road
        if (WaypointManager.instance != null)
        {
            Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
        }
        /*// Bỏ vào awake thì bị lỗi nếu instantiate từ necromancer và boss mystery
        if (Waypoint_CurrentIndex == 0) // Đảm bảo việc gán từ bên ngoài
        {
            Waypoint_CurrentIndex = 1;
        }*/
        StartCoroutine(SpawnMinions());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Die();
    }
    IEnumerator SpawnMinions()
    {
        while (true)
        {
            GameObject magiccircle = Instantiate(MagicCircle, transform.position, Quaternion.identity);
            Destroy(magiccircle, 1f);
            for (int i = 0; i < 4; i++)
            {
                GameObject newEnemy = Instantiate(Minion, transform.position, Quaternion.identity);
                BaseEnemy enemy = newEnemy.GetComponent<BaseEnemy>();
                enemy.Waypoint_CurrentIndex = this.Waypoint_CurrentIndex;
                enemy.Waypoint_SelectedIndex = this.Waypoint_SelectedIndex;
                enemy.Distance = this.Distance;
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
