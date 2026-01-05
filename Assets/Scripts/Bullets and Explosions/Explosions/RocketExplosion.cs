using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : BaseBullets
{
    public float ClusterDamage = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            if (character != null)
            {
                if (ClusterDamage == 0f)
                {
                    baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                }
                else // nghĩa là đã được cài từ bên ngoài
                {
                    baseEnemy.TakeDamage(ClusterDamage, character.canStrikethroughOrNot());
                }
            }
            Destroy(this.gameObject);
        }
    }
}
