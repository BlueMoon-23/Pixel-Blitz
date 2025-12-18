using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : GroundCharacter
{
    void Start()
    {
        Range = 6f;
        Damage = 3f;
        Cooldown = 2f;
        Cost = 600f;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = false;
        UpgradeCost = new float[] { 150, 750, 1500, 9000 };
        SellCost = (int)(Cost / 3);
        _hasAbility = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned) { AttackWithCooldown(Bow_Attack_Duration); }
        // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
    }
    public override float GetRange() 
    {
        if (Range <= 6f) {  return 6f; } // <= la chua duoc khoi tao
        else return Range; 
    }
    public override float GetCost()
    {
        if (Cost != 600f) { return 600f; }
        else return Cost;
    }
    public override void UpgradeToLevel1()
    {
        Range = 7f;
        Cooldown = 1.5f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Range = 10f;
        hasHiddenDetection = true;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Cooldown = 0.5f;
        Damage = 6f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Cooldown = 0.25f;
        Damage = 10f;
        Level = 4;
        // ShootIn3
    }
    public override void SetUpgradeInformation()
    {
        characterUI.characterName.text = "Archer";
        characterUI.characterImage.sprite = characterUI.characterImages[0];
        switch (Level)
        {
            case 1:
                {
                    characterUI.upgradeName.text = "Eagle Eye";
                    characterUI.Info1.text = "Range: 7 => 10";
                    characterUI.Info2.text = "+ Hidden Detection";
                    characterUI.Info3.text = "";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Quick Shot";
                    characterUI.Info1.text = "Cooldown: 1.5s => 0.5s";
                    characterUI.Info2.text = "Damage: 3 => 6";
                    characterUI.Info3.text = "";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Arrow Barrage";
                    characterUI.Info1.text = "Shoot three arrows";
                    characterUI.Info2.text = "Cooldown: 0.5s => 0.25s";
                    characterUI.Info3.text = "Damage: 6 => 10";
                    break;
                }
            case 4:
                {
                    characterUI.upgradeName.text = "";
                    characterUI.Info1.text = "";
                    characterUI.Info2.text = "";
                    characterUI.Info3.text = "";
                    break;
                }
            default:
                {
                    characterUI.upgradeName.text = "Better Gloves";
                    characterUI.Info1.text = "Range: 6 => 7";
                    characterUI.Info2.text = "Cooldown: 2s => 1.5s";
                    characterUI.Info3.text = "";
                    break;
                }
        }
        base.SetUpgradeInformation();
    }
    public override IEnumerator AttackWithAnimation(float Attack_Duration)
    {
        if (Level < 4)
        {
            BaseEnemy first_enemy = FindFirstEnemy();
            if (first_enemy != null && !first_enemy.isDieOrNot())
            {
                if (first_enemy.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                else
                {
                    transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                // Play animation
                SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, IndexPair[PlayerState.ATTACK]);
                SPUM_Prefabs._anim.speed = 2 * Attack_Duration / Cooldown;
                yield return new WaitForSeconds(Attack_Duration / 2 + 0.1f);
                // Bắn đạn: đạn archer cong cong cho đẹp
                GameObject newBullet = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, Quaternion.identity);
                BaseBullets bullet = newBullet.GetComponent<BaseBullets>();
                bullet.SetCharacter(this);
                bullet.SetEnemy(first_enemy);
                yield return new WaitForSeconds(Attack_Duration - (Attack_Duration / 2 + 0.1f));
                SPUM_Prefabs._anim.speed = 1;
            }
        }
        else
        {
            BaseEnemy[] first_3_enemies = FindThreeFirstEnemies();
            if (first_3_enemies[0] != null && !first_3_enemies[0].isDieOrNot())
            {
                if (first_3_enemies[0].transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                else
                {
                    transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                // Play animation
                SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, IndexPair[PlayerState.ATTACK]);
                SPUM_Prefabs._anim.speed = 2 * Attack_Duration / Cooldown;
                yield return new WaitForSeconds(Attack_Duration / 2 + 0.1f);
                for (int i = 0; i < first_3_enemies.Length; i++)
                {
                    GameObject newBullet = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, Quaternion.identity);
                    BaseBullets bullet = newBullet.GetComponent<BaseBullets>();
                    bullet.SetCharacter(this);
                    bullet.SetEnemy(first_3_enemies[i]);
                }
                yield return new WaitForSeconds(Attack_Duration - (Attack_Duration / 2 + 0.1f));
                SPUM_Prefabs._anim.speed = 1;
            }
        }
    }
}
