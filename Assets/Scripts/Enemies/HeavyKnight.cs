using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyKnight : BaseEnemy
{
    public GameObject StompEffect;
    public GameObject LowGraphic_StompEffect;
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
    protected override IEnumerator ResetStats()
    {
        yield return StartCoroutine(base.ResetStats());
        yield return null;
        isFinalBoss = false;
        StartCoroutine(StompGround());
    }
    protected IEnumerator StompGround()
    {
        do
        {
            if (SPUM_Prefabs != null)
            {
                SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, 0);
                SPUM_Prefabs._anim.speed = 0.5f;
            }
            yield return new WaitForSeconds(1f);
            if (SoundManager.Instance != null) { SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.StompGround_Sound); }
            /*GameObject newEffect = Instantiate(StompEffect, transform.position, Quaternion.identity);
            HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
            hitCharacterExplosion.StunDuration = 1f;
            Destroy(newEffect, 1f);*/
            GameObject chosenExplosion_SFX = StompEffect;
            if (GameSetting.instance != null && !GameSetting.instance._showExplosion)
            {
                chosenExplosion_SFX = LowGraphic_StompEffect;
            }
            if (ExplosionPooler.instance != null)
            {
                BaseExplosion newEffect = ExplosionPooler.instance.GetExplosion(chosenExplosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
                if (newEffect != null)
                {
                    newEffect.transform.position = this.transform.position;
                    newEffect.transform.rotation = Quaternion.identity;
                    HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
                    hitCharacterExplosion.StunDuration = 1f;
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(newEffect, 0.5f));
                }
            }
            yield return new WaitForSeconds(14f);
        }
        while (true);
    }
}
