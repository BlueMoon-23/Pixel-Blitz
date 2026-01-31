using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaulerAim : MonoBehaviour
{
    // Object này có công dụng là di chuyển về phía Aimedcharacter. Mỗi 0.1s sẽ sinh ra BlastEffect tại this.transform.position
    public GameObject BlastEffect; // flashexplosionpink
    public GameObject LowGraphic_BlastEffect;
    private BaseCharacter AimedCharacter;
    private float AimSpeed = 10f;
    private Vector3 Direction;
    void Start()
    {
        Direction = (AimedCharacter.transform.position - this.transform.position).normalized;
        StartCoroutine(CreateBlastEffect());
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Direction * AimSpeed * Time.deltaTime;
    }
    private IEnumerator CreateBlastEffect()
    {
        while (true)
        {
            //GameObject newBlastEffect = Instantiate(BlastEffect, transform.position, Quaternion.identity);
            //Destroy(newBlastEffect, 1.0f);
            GameObject chosenExplosion_SFX = BlastEffect;
            if (GameSetting.instance != null && !GameSetting.instance._showExplosion)
            {
                chosenExplosion_SFX = LowGraphic_BlastEffect;
            }
            if (ExplosionPooler.instance != null)
            {
                BaseExplosion newBlastEffect = ExplosionPooler.instance.GetExplosion(chosenExplosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
                if (newBlastEffect != null)
                {
                    newBlastEffect.transform.position = this.transform.position;
                    newBlastEffect.transform.rotation = Quaternion.identity;
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(newBlastEffect, 1.0f));
                }
            }
            yield return new WaitForSeconds(1 / AimSpeed);
        }
    }
    public void SetAimedCharacter(BaseCharacter groundCharacter) { this.AimedCharacter = groundCharacter; }
}
