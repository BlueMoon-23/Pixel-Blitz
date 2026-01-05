using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyKnight : BaseEnemy
{
    public GameObject StompEffect;
    void Start()
    {
        // Move road
        if (WaypointManager.instance != null)
        {
            Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
        }
        isFinalBoss = false;
        StartCoroutine(StompGround());
    }
    void Update()
    {
        Move();
        Die();
    }
    protected IEnumerator StompGround()
    {
        do
        {
            SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, IndexPair[PlayerState.ATTACK]);
            SPUM_Prefabs._anim.speed = 0.5f;
            yield return new WaitForSeconds(1f);
            if (SoundManager.Instance != null) { SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.StompGround_Sound); }
            GameObject newEffect = Instantiate(StompEffect, transform.position, Quaternion.identity);
            HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
            hitCharacterExplosion.StunDuration = 1f;
            Destroy(newEffect, 1f);
            yield return new WaitForSeconds(14f);
        }
        while (true);
    }
}
