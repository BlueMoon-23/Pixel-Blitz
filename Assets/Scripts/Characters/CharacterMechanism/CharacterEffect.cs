using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    // Stunned Effect
    [Header("Effect")]
    public GameObject StunnedEffect;
    private int EffectID;
    private bool _isStunned = false;
    public bool isStunned
    {
        get { return _isStunned; }
        set { _isStunned = value; }
    }
    private float _stunEndTime;
    public float stunEndTime
    {
        get { return _stunEndTime; }
        set { _stunEndTime = value; }
    }
    public void ResetCharacterEffect()
    {
        isStunned = false;
        stunEndTime = Time.time;
    }
    private void Start()
    {
        EffectID = StunnedEffect.GetComponent<BaseExplosion>().ExplosionID;
    }
    public IEnumerator GetStunned(float duration) // LOGIC CŨ LÀ STOP COROUTINE THÌ LÒI RA LỖI CỦA UNITY, NÊN ĐỔI CHỨ K CÓ SAI NGHEN
    {
        isStunned = true;
        stunEndTime = Time.time + duration;
        //GameObject newEffect = Instantiate(StunnedEffect, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        //Destroy(newEffect, duration);
        if (ExplosionPooler.instance != null)
        {
            BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(EffectID);
            if (explosionSFX != null)
            {
                explosionSFX.transform.position = transform.position + new Vector3(0, 1f, 0);
                explosionSFX.transform.rotation = Quaternion.identity * Quaternion.Euler(90f, 0, 0);
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, duration));
            }
        }
        // vòng lặp kiểm tra thời gian stun ngay trong chính hàm này
        while (Time.time < stunEndTime) { yield return null; }
        isStunned = false;
    }
}
