using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : GroundCharacter
{
    protected override void OnEnable()
    {
        base.OnEnable();
        Range = 5f;
        Damage = 2f;
        Cooldown = 1.5f;
        Cost = 300f;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = false;
        UpgradeCost = new float[] { 150, 240, 2050, 6300 };
        SellCost = (int)(Cost / 3);
        _hasAbility = false;
        Bow_Attack_Duration = 0.833f;
    }

    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            float min_duration = Bow_Attack_Duration < Cooldown ? Bow_Attack_Duration : Cooldown;
            if (!isStunned) { AttackWithCooldown(min_duration); }
            // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
        }
    }
    public override float GetRange() 
    {
        if (Range <= 5f) {  return 5f; } // <= la chua duoc khoi tao
        else return Range; 
    }
    public override float GetCost()
    {
        if (Cost != 300f) { return 300f; }
        else return Cost;
    }
    public override void UpgradeToLevel1()
    {
        Range = 6f;
        Cooldown = 1f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Range = 7f;
        Damage = 3f;
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
        base.UpgradeToLevel4();
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            characterUI.characterName.text = "Archer";
            characterUI.characterImage.sprite = characterUI.characterImages[0];
            switch (Level)
            {
                case 1:
                    {
                        characterUI.upgradeName.text = "Eagle Eye";
                        characterUI.Info1.text = "Range: 6 => 7";
                        characterUI.Info2.text = "Damage: 2 => 3";
                        characterUI.Info3.text = "+ Hidden Detection";
                        break;
                    }
                case 2:
                    {
                        characterUI.upgradeName.text = "Quick Shot";
                        characterUI.Info1.text = "Cooldown: 1s => 0.5s";
                        characterUI.Info2.text = "Damage: 2 => 6";
                        characterUI.Info3.text = "";
                        break;
                    }
                case 3:
                    {
                        characterUI.upgradeName.text = "Arrow Barrage";
                        characterUI.Info1.text = "Shoot three arrows instead of one.";
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
                        characterUI.Info1.text = "Range: 5 => 6";
                        characterUI.Info2.text = "Cooldown: 1.5s => 1s";
                        characterUI.Info3.text = "";
                        break;
                    }
            }
            base.SetUpgradeInformation();
        }
    }
    public override IEnumerator AttackWithAnimation(float Attack_Duration)
    {
        if (Level < 4)
        {
            BaseEnemy first_enemy = FindFirstEnemy();
            if (first_enemy != null)
            {
                SelfRotate(first_enemy);
                PlayAttackAmination(Attack_Duration);
                yield return new WaitForSeconds(Bow_Attack_Duration / SPUM_Prefabs._anim.speed * 0.5f);
                // Bắn đạn: đạn archer cong cong cho đẹp
                if (BulletPooler.instance != null)
                {
                    BaseBullets bullet = BulletPooler.instance.GetBullet(bullet_Prefab.GetComponent<BaseBullets>().BulletID);
                    if (bullet != null)
                    {
                        bullet.transform.position = Bullet_StartPosition.transform.position;
                        bullet.transform.rotation = Quaternion.identity;
                        bullet.SetCharacter(this);
                        if (first_enemy != null)
                        {
                            bullet.SetEnemy(first_enemy);
                        }
                    }
                }
                // Tạo hiệu ứng nổ đạn (muzzle)
                MuzzleEffect(Quaternion.identity);
                yield return new WaitForSeconds(Bow_Attack_Duration / SPUM_Prefabs._anim.speed * 0.5f);
                SPUM_Prefabs._anim.speed = 1;
            }
        }
        else
        {
            List<BaseEnemy> first_3_enemies = FindThreeFirstEnemies();
            if (first_3_enemies.Count == 0) yield break;
            if (first_3_enemies[0] != null) { SelfRotate(first_3_enemies[0]); }
            // Play animation
            PlayAttackAmination(Attack_Duration);
            yield return new WaitForSeconds(Bow_Attack_Duration / SPUM_Prefabs._anim.speed * 0.5f);
            for (int i = 0; i < first_3_enemies.Count; i++)
            {
                if (BulletPooler.instance != null)
                {
                    BaseBullets bullet = BulletPooler.instance.GetBullet(bullet_Prefab.GetComponent<BaseBullets>().BulletID);
                    if (bullet != null)
                    {
                        bullet.transform.position = Bullet_StartPosition.transform.position;
                        bullet.transform.rotation = Quaternion.identity;
                        bullet.SetCharacter(this);
                        if (first_3_enemies[i] != null) { bullet.SetEnemy(first_3_enemies[i]); }
                    }
                }
            }
            // Tạo hiệu ứng nổ đạn (muzzle)
            MuzzleEffect(Quaternion.identity);
            yield return new WaitForSeconds(Bow_Attack_Duration / SPUM_Prefabs._anim.speed * 0.5f);
            SPUM_Prefabs._anim.speed = 1;
        }
    }
}
