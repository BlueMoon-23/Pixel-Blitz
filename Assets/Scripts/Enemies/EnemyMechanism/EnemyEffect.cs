using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    private BaseEnemy Enemy;
    // Script quản lý việc enemy bị stun / freeze / burn / ...
    [Header("Freeze effect")]
    public Material FreezeMaterial;
    public Material OriginalMaterial;
    private SpriteRenderer[] spriteRenderers;
    private int FreezeStack = 3;
    private int FreezeCurrentStack = 0;
    private bool _isFrozen = false;
    public bool isFrozen
    {
        get { return _isFrozen; }
        set { _isFrozen = value; }
    }
    [Header("Stunned effect")]
    public GameObject StunEffectPrefab;
    public GameObject StunEffectLocation;
    private int StunEffectID;
    private bool _isStunned = false;
    public bool isStunned
    {
        get { return _isStunned; }
        set { _isStunned = value; }
    }
    public void ResetEnemyEffect()
    {
        FreezeCurrentStack = 0;
        isFrozen = false;
        isStunned = false;
        if (spriteRenderers != null)
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.material = OriginalMaterial;
            }
        }
    }
    void Start()
    {
        Enemy = GetComponent<BaseEnemy>();
        spriteRenderers = Enemy.EnemyRoot.GetComponentsInChildren<SpriteRenderer>();
        StunEffectID = StunEffectPrefab.GetComponent<BaseExplosion>().ExplosionID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetFreeze(float FreezeTime, int FreezeCount)
    {
        if (Enemy is not FinalBoss)
        {
            FreezeCurrentStack++;
            FreezeStack = FreezeCount;
            if (FreezeCurrentStack == FreezeStack)
            {
                StartCoroutine(BeFrozen(FreezeTime));
            }
        }
    }
    public IEnumerator GetStunned(float StunDuration)
    {
        if (Enemy is not FinalBoss)
        {
            isStunned = true;
            if (ExplosionPooler.instance != null)
            {
                BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(StunEffectID);
                if (explosionSFX != null)
                {
                    if (StunEffectLocation != null) explosionSFX.transform.position = StunEffectLocation.transform.position;
                    else explosionSFX.transform.position = transform.position + new Vector3(0, transform.localScale.y * 0.75f, 0);
                    explosionSFX.transform.rotation = Quaternion.identity * Quaternion.Euler(90f, 0, 0);
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, StunDuration));
                }
            }
            yield return new WaitForSeconds(StunDuration);
            isStunned = false;
        }
    }
    private IEnumerator BeFrozen(float FreezeTime)
    {
        isFrozen = true;
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material = FreezeMaterial;
        }
        yield return new WaitForSeconds(FreezeTime);
        isFrozen = false;
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material = OriginalMaterial;
        }
        FreezeCurrentStack = 0;
        yield break;
    }
}
