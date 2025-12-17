using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FrostExplosion : BaseBullets
{
    // Start is called before the first frame update
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
                baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                Freezer freezer = character as Freezer; // as là toán tử ép kiểu
                baseEnemy.GetFreeze(freezer.FreezeTime, freezer.FreezeCount);
            }
            Destroy(this.gameObject);
        }
    }
}
