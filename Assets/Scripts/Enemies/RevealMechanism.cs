using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealMechanism : MonoBehaviour
{
    // Mỗi RevealCooldonw giây, tự tắt hidden detection trong vòng RevealDuration giây
    [SerializeField] private float RevealCooldown;
    [SerializeField] private float RevealDuration;
    [SerializeField] private GameObject HiddenTag;
    private BaseEnemy baseEnemy;
    private void OnEnable()
    {
        baseEnemy = GetComponent<BaseEnemy>();
        if (baseEnemy != null) StartCoroutine(Reveal());
    }
    private IEnumerator Reveal()
    {
        do
        {
            baseEnemy.isHidden = true;
            HiddenTag.SetActive(true);
            yield return new WaitForSeconds(RevealCooldown);
            // Reveal
            baseEnemy.isHidden = false;
            HiddenTag.SetActive(false);
            yield return new WaitForSeconds(RevealDuration);
        }
        while (true);
    }
}
