using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StarExplosion : BaseExplosion
{
    private BaseCharacter Character;
    private float damageValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(BaseCharacter character, float Damage)
    {
        Character = character;
        damageValue = Damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            bool doStrikethrough = Character != null ? Character.canStrikethroughOrNot() : false;
            baseEnemy.TakeDamage(Character, damageValue, doStrikethrough);
            if (baseEnemy.GetHP() > 0) baseEnemy.StartCoroutine(baseEnemy.enemyEffect.GetStunned(1f));
        }
    }
}
