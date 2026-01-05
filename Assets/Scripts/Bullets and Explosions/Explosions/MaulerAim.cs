using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaulerAim : MonoBehaviour
{
    // Object này có công dụng là di chuyển về phía Aimedcharacter. Mỗi 0.1s sẽ sinh ra BlastEffect tại this.transform.position
    public GameObject BlastEffect; // flashexplosionpink
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
            GameObject newBlastEffect = Instantiate(BlastEffect, transform.position, Quaternion.identity);
            Destroy(newBlastEffect, 1.0f);
            yield return new WaitForSeconds(1 / AimSpeed);
        }
    }
    public void SetAimedCharacter(BaseCharacter groundCharacter) { this.AimedCharacter = groundCharacter; }
}
