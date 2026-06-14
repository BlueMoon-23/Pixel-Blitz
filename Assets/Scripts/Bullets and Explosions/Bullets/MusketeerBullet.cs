using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerBullet : BaseBullets
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected override void ExplodeOnImpact()
    {
        base.ExplodeOnImpact();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null && baseEnemy == enemy)
        {
            if (character != null)
            {
                baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                // khác biệt nằm ở đây
                if (baseEnemy.isDieOrNot())
                {
                    Musketeer musketeer = character as Musketeer;
                    if (musketeer != null)
                    {
                        musketeer.AttackImmediately();
                    }
                }
            }
            ExplodeOnImpact();
        }
    }
}
