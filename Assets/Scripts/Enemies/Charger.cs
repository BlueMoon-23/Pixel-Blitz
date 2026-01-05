using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : BaseEnemy
{
    public GameObject LightningEffect;
    void Start()
    {
        // Move road
        if (WaypointManager.instance != null)
        {
            Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
        }
        isFinalBoss = false;
        SpeedUpAllEnemies();
    }
    void Update()
    {
        Move();
        Die();
    }
    private void SpeedUpAllEnemies()
    {
        if (SoundManager.Instance != null) { SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.ChargerSound); }
        GameObject newLightningEffect = Instantiate(LightningEffect, transform.position, Quaternion.identity);
        Destroy(newLightningEffect, 1.0f );
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.SpeedUpAllEnemies(1.5f);
        }
    }
}
