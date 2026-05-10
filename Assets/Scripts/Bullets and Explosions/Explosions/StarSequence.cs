using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSequence : BaseExplosion
{
    // Start is called before the first frame update
    private float damageValue = 0f;

    public GameObject[] StarExplosions; // lắp theo hệ đếm -2, -1, 0, 1 , 2
    void OnEnable()
    {
        StartCoroutine(ExplodeInSequence());
        if (ExplosionPooler.instance != null)
        {
            ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(this, 2.0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDamage(float Damage)
    {
        damageValue = Damage;
    }
    private void setExplodeDamage(GameObject Explostion, float Damage)
    {
        StarExplosion starExplosion = Explostion.GetComponent<StarExplosion>();
        if (starExplosion != null)
        {
            starExplosion.SetDamage(Damage);
        }
    }
    private IEnumerator ExplodeInSequence()
    {
        for (int i = 0; i <= 2; i++)
        {
            StarExplosions[i + 2].SetActive(true);
            setExplodeDamage(StarExplosions[i + 2], damageValue);
            StarExplosions[-i + 2].SetActive(true);
            setExplodeDamage(StarExplosions[-i + 2], damageValue);
            yield return new WaitForSeconds(0.5f);
            StarExplosions[i + 2].SetActive(false);
            StarExplosions[-i + 2].SetActive(false);
        }
        // vòng 1: bật 2, 2
        // vòng 2: bậc 3, 1
        // vòng 3: bậc 4, 0
    }
}
