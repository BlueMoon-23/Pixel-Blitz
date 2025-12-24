using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerBullet : BaseBullets
{
    public GameObject FrostExplosion;
    void Start()
    {
        if (character.GetLevel() < 3)
        {
            transform.localScale *= 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (character != null && character.GetLevel() < 4)
        {
            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null && baseEnemy == enemy)
            {
                baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
                Freezer freezer = character as Freezer; // as là toán tử ép kiểu
                baseEnemy.GetFreeze(freezer.FreezeTime, freezer.FreezeCount);
                GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
                Destroy(spawnedSFX, 0.5f);
                Destroy(this.gameObject);
            }
        }
        else if (character.GetLevel() >= 4)
        {
            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null && baseEnemy == enemy)
            {
                GameObject explosion = Instantiate(FrostExplosion, this.transform.position, Quaternion.identity);
                BaseBullets baseBullets = explosion.GetComponent<BaseBullets>();
                baseBullets.SetCharacter(character);
                GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
                spawnedSFX.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Destroy(spawnedSFX, 0.5f);
                Destroy(this.gameObject);
            }
        }
    }
}
