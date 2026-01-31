using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : CliffCharacter
{
    protected override void OnEnable()
    {
        base.OnEnable();
        Range = 11f;
        Damage = 50f;
        Cooldown = 4f;
        Cost = 3200;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = true;
        UpgradeCost = new float[] { 600, 3400, 14500, 30000 };
        SellCost = (int)(Cost / 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            if (!isStunned) { AttackWithoutAnimation(); }
            // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
        }
    }
    public override float GetRange()
    {
        if (Range <= 11f) { return 11f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 3200) { return 3200; }
        else return Cost;
    }
    public override void UpgradeToLevel1()
    {
        Cooldown = 3.5f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Range = 15f;
        Damage = 95f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Range = 18f;
        Cooldown = 3f;
        Damage = 275;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Cooldown = 6f;
        Damage = 1200;
        Level = 4;
        base.UpgradeToLevel4();
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            characterUI.characterName.text = "Ranger";
            characterUI.characterImage.sprite = characterUI.characterImages[3];
            switch (Level)
            {
                case 0:
                    {
                        characterUI.upgradeName.text = "Faster Reloading";
                        characterUI.Info1.text = "Cooldown: 4s => 3.5s";
                        characterUI.Info2.text = "";
                        characterUI.Info3.text = "";
                        break;
                    }
                case 1:
                    {
                        characterUI.upgradeName.text = "Cherry Blossom";
                        characterUI.Info1.text = "Range: 11 => 15";
                        characterUI.Info2.text = "Damage: 50 => 95";
                        characterUI.Info3.text = "";
                        break;
                    }
                case 2:
                    {
                        characterUI.upgradeName.text = "Quickdraw Specialist";
                        characterUI.Info1.text = "Range: 15 => 18";
                        characterUI.Info2.text = "Damage: 95 => 275";
                        characterUI.Info3.text = "Cooldown: 3.5s => 3s";
                        break;
                    }
                case 3:
                    {
                        characterUI.upgradeName.text = "Wild Exceptional";
                        characterUI.Info1.text = "Stun enemies for 1s";
                        characterUI.Info2.text = "Damage: 275 => 1200";
                        characterUI.Info3.text = "Cooldown: 3s => 6s";
                        break;
                    }
                default:
                    {
                        characterUI.upgradeName.text = "";
                        characterUI.Info1.text = "";
                        characterUI.Info2.text = "";
                        characterUI.Info3.text = "";
                        break;
                    }
            }
            base.SetUpgradeInformation();
        }
    }
    public override void AttackWithoutAnimation()
    {
        if (isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (range.enemies_in_range.Count != 0)
            {
                BaseEnemy first_enemy = FindFirstEnemy();
                if (first_enemy != null)
                {
                    SelfRotate(first_enemy);
                    // Bắn đạn: lưu ý là truyền góc là hướng bắn của mình luôn chứ không dùng transform.rotation hay quaternion.identity
                    float Angle_in_Radian = Mathf.Atan2(first_enemy.Center.transform.position.y - transform.position.y, first_enemy.Center.transform.position.x - transform.position.x);
                    Quaternion Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
                    if (BulletPooler.instance != null)
                    {
                        BaseBullets bullet = BulletPooler.instance.GetBullet(bullet_Prefab.GetComponent<BaseBullets>().BulletID);
                        if (bullet != null)
                        {
                            bullet.transform.position = Bullet_StartPosition.transform.position;
                            bullet.transform.rotation = Angle_in_Quaternion;
                            bullet.SetCharacter(this);
                            if (first_enemy != null)
                            {
                                bullet.SetEnemy(first_enemy);
                            }
                        }
                        BulletPooler.instance.StartCoroutine(BulletPooler.instance.ReturnBulletWithDelay(bullet, 1f));
                        // Gán headgun cho rangerlaser
                        RangerLaser rangerLaser = bullet.GetComponent<RangerLaser>();
                        rangerLaser.HeadGun = Bullet_StartPosition;
                    }
                    // Tạo hiệu ứng nổ đạn (muzzle)
                    MuzzleEffect(Angle_in_Quaternion);
                    Clock = 0f;
                }
            }
            else
            {
                Clock = Cooldown;
            }
        }
    }
}
