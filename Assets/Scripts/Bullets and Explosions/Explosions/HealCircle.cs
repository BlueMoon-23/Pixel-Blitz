using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCircle : BaseExplosion
{
    // Script này gắn lên magic circle green chứ không phải healfield
    // mỗi 2s hồi 750 máu cho toàn bộ enemies nằm trong vòng magic circle của nó
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.GetHealed(500f);
        }
    }
}
