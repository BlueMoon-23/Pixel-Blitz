using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyFinalBoss : FinalBoss
{
    // Easy: 25k HP, dậm sàn gây choáng toàn map
    void Start()
    {
        isFinalBoss = true;
        StartCoroutine(StompGround());
    }
    void Update()
    {
        Move();
        Die();
    }
}
