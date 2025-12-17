using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class BaseBullets : MonoBehaviour
{
    [SerializeField] protected BaseCharacter character;
    [SerializeField] protected BaseEnemy enemy;
    [SerializeField] protected float BulletSpeed = 10f;
    // Serialize field giup unity biet duoc rang object nay can duoc luu tru
    public GameObject Explosion_SFX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void SetCharacter(BaseCharacter character)
    { 
        this.character = character; 
    }
    public void SetEnemy(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    protected void Move()
    {
        if (enemy != null)
        {
            float Angle_in_Radian = Mathf.Atan2(enemy.transform.position.y - transform.position.y, enemy.transform.position.x - transform.position.x);
            Quaternion Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
            transform.rotation = Angle_in_Quaternion;
            transform.position += transform.up * BulletSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null && baseEnemy == enemy)
        {
            if (character != null)
            {
                baseEnemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
            }
            GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
            Destroy(spawnedSFX, 0.5f);
            Destroy(this.gameObject);
        }
    }
}
