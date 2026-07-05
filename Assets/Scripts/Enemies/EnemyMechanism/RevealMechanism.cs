using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealMechanism : MonoBehaviour
{
    // Mỗi RevealCooldonw giây, tự tắt hidden detection trong vòng RevealDuration giây
    [SerializeField] private float RevealCooldown;
    [SerializeField] private float RevealDuration;
    [SerializeField] private GameObject HiddenTag;
    [SerializeField] private GameObject SmokeExplosion;
    private BaseEnemy baseEnemy;
    private void OnEnable()
    {
        baseEnemy = GetComponent<BaseEnemy>();
        if (baseEnemy != null) StartCoroutine(Reveal());
    }
    private IEnumerator Reveal()
    {
        yield return null;
        do
        {
            baseEnemy.isHidden = true;
            HiddenTag.SetActive(true);
            RevealExplosion();
            yield return new WaitForSeconds(RevealCooldown);
            // Reveal
            baseEnemy.isHidden = false;
            HiddenTag.SetActive(false);
            RevealExplosion();
            yield return new WaitForSeconds(RevealDuration);
        }
        while (true);
    }
    private void RevealExplosion()
    {
        if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showExplosion)
        {
            BaseExplosion explosion = ExplosionPooler.instance.GetExplosion(SmokeExplosion.GetComponent<BaseExplosion>().ExplosionID);
            if (explosion != null)
            {
                explosion.transform.position = baseEnemy.transform.position;
                explosion.transform.rotation = baseEnemy.transform.rotation;
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosion, 0.5f));
            }
        }
    }
}
