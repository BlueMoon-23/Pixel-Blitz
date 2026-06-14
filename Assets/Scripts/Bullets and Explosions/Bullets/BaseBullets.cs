using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullets : MonoBehaviour
{
    [SerializeField] protected BaseCharacter character;
    [SerializeField] protected BaseEnemy enemy;
    [SerializeField] protected Transform EnemyCenter;
    [SerializeField] protected float BulletSpeed = 10f;
    public int BulletID;
    // Serialize field giup unity biet duoc rang object nay can duoc luu tru
    public GameObject Explosion_SFX;
    public GameObject LowGraphic_Explosion_SFX;
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
    public void SetEnemy(BaseEnemy enemy) // gọi theo giây
    {
        this.enemy = enemy;
        this.EnemyCenter = enemy.Center.transform;
    }
    /// <summary>
    /// Điều kiện move không phụ thuộc vào enemy. Khi enemy biến mất, tiếp tục di chuyển đến vị trí đã định sẵn, sau đó thực hiện nổ
    /// </summary>
    protected void Move() // gọi theo frame
    {
        if (enemy != null && enemy.gameObject.activeInHierarchy)
        {
            this.EnemyCenter = enemy.Center.transform;
        }
        // Kiểm tra khoảng cách đủ gần để thực hiện hành vi nổ tại chỗ
        else if (Vector3.Distance(transform.position, EnemyCenter.position) <= BulletSpeed * Time.deltaTime + 0.05f)
        {
            ExplodeOnImpact();
        }
        // Đi tới vị trí enemyCenter thay vì đích danh enemy
        float Angle_in_Radian = Mathf.Atan2(EnemyCenter.transform.position.y - transform.position.y, EnemyCenter.transform.position.x - transform.position.x);
        Quaternion Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
        transform.rotation = Angle_in_Quaternion;
        transform.position += transform.up * BulletSpeed * Time.deltaTime;
    }
    protected virtual void ExplodeOnImpact()
    {
        if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showExplosion)
        {
            BaseExplosion explosionSFX = ExplosionPooler.instance.GetExplosion(Explosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
            if (explosionSFX != null)
            {
                explosionSFX.transform.position = this.transform.position;
                explosionSFX.transform.rotation = Quaternion.identity;
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosionSFX, 0.5f));
            }
        }
        if (BulletPooler.instance != null)
        {
            BulletPooler.instance.ReturnBullet(this);
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
            //GameObject spawnedSFX = Instantiate(Explosion_SFX, this.transform.position, Quaternion.identity);
            //Destroy(spawnedSFX, 0.5f);
            ExplodeOnImpact();
        }
    }
}
