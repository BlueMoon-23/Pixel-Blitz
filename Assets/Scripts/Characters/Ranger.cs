using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : CliffCharacter
{
    void Start()
    {
        Range = 12f;
        Damage = 50f;
        Cooldown = 4.5f;
        Cost = 4500f;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = true;
        UpgradeCost = new float[] { 900, 4250, 10000, 22500 };
        SellCost = (int)(Cost / 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned) { AttackWithoutAnimation(); }
        // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
    }
    public override float GetRange()
    {
        if (Range <= 12f) { return 12f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 4500f) { return 4500f; }
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
        Damage = 65f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Range = 25f;
        Cooldown = 3f;
        Damage = 150f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Cooldown = 7f;
        Damage = 1500f;
        Level = 4;
        // Stun
    }
    public override void SetUpgradeInformation()
    {
        characterUI.characterName.text = "Ranger";
        characterUI.characterImage.sprite = characterUI.characterImages[3];
        switch (Level)
        {
            case 0:
                {
                    characterUI.upgradeName.text = "Faster Reloading";
                    characterUI.Info1.text = "Cooldown: 4.5s => 3.5s";
                    characterUI.Info2.text = "";
                    characterUI.Info3.text = "";
                    break;
                }
            case 1:
                {
                    characterUI.upgradeName.text = "Cherry Blossom";
                    characterUI.Info1.text = "Range: 12 => 15";
                    characterUI.Info2.text = "Damage: 50 => 65";
                    characterUI.Info3.text = "";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Quickdraw Specialist";
                    characterUI.Info1.text = "Range: 15 => 25";
                    characterUI.Info2.text = "Damage: 65 => 150";
                    characterUI.Info3.text = "Cooldown: 3.5s => 3s";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Wild Exceptional";
                    characterUI.Info1.text = "Stun enemies for 1s";
                    characterUI.Info2.text = "Damage: 150 => 1500";
                    characterUI.Info3.text = "Cooldown: 3s => 7s";
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
    public override void AttackWithoutAnimation()
    {
        if (isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            BaseEnemy first_enemy = FindFirstEnemy();
            if (first_enemy != null && !first_enemy.isDieOrNot())
            {
                // Xoay bản thân
                if (first_enemy.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                else
                {
                    transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                // Bắn đạn: lưu ý là truyền góc là hướng bắn của mình luôn chứ không dùng transform.rotation hay quaternion.identity
                float Angle_in_Radian = Mathf.Atan2(first_enemy.transform.position.y - transform.position.y, first_enemy.transform.position.x - transform.position.x);
                Quaternion Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
                GameObject newBullet = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, Angle_in_Quaternion);
                Destroy(newBullet, 1f);
                BaseBullets bullet = newBullet.GetComponent<BaseBullets>();
                bullet.SetCharacter(this);
                bullet.SetEnemy(first_enemy);
                // Gán headgun cho rangerlaser
                RangerLaser rangerLaser = bullet.GetComponent<RangerLaser>();
                rangerLaser.HeadGun = Bullet_StartPosition;
                // Tạo hiệu ứng nổ đạn (muzzle)
                GameObject muzzle = Instantiate(BulletMuzzle, Bullet_StartPosition.transform.position, Angle_in_Quaternion);
                Destroy(muzzle, 0.25f);
            }
            Clock = 0f;
        }
    }
}
