using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musketeer : GroundCharacter
{
    public bool didAttackImmediately;
    protected override void OnEnable()
    {
        base.OnEnable();
        Range = 5f;
        Damage = 7f;
        Cooldown = 2f;
        Cost = 800;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = false;
        UpgradeCost = new float[] { 350, 1680, 3100, 8500 };
        SellCost = (int)(Cost / 3);
        _hasAbility = false;
        didAttackImmediately = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            if (!isStunned) 
            {
                AttackWithoutAnimation(); 
            }
            // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
        }
    }
    public override float GetRange()
    {
        if (Range <= 5f) { return 5f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 800) { return 800; }
        else return Cost;
    }
    public override void UpgradeToLevel1()
    {
        Range = 6f;
        Damage = 10f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Damage = 25f;
        canStrikethrough = true;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Damage = 40f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Cooldown = 1.5f;
        Damage = 100f;
        Range = 8f;
        Level = 4;
        base.UpgradeToLevel4();
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            characterUI.characterName.text = "Musketeer";
            characterUI.characterImage.sprite = characterUI.characterImages[8];
            switch (Level)
            {
                case 0:
                    {
                        characterUI.upgradeName.text = "Sharpened Aim";
                        characterUI.Info1.text = "Range: 5 => 6";
                        characterUI.Info2.text = "Damage: 7 => 10";
                        characterUI.Info3.text = "";
                        break;
                    }
                case 1:
                    {
                        characterUI.upgradeName.text = "Piercing Melody";
                        characterUI.Info1.text = "Damage: 10 => 25";
                        characterUI.Info2.text = "+ Strikethrough";
                        characterUI.Info3.text = "";
                        break;
                    }
                case 2:
                    {
                        characterUI.upgradeName.text = "Venus Chain";
                        characterUI.Info1.text = "Damage: 25 => 40";
                        characterUI.Info2.text = "Automatically shoot at the next enemy if its target enemy died.";
                        characterUI.Info3.text = "";
                        break;
                    }
                case 3:
                    {
                        characterUI.upgradeName.text = "Fragile Silence";
                        characterUI.Info1.text = "Range: 8 => 10";
                        characterUI.Info2.text = "Damage: 40 => 100";
                        characterUI.Info3.text = "Cooldown: 2s => 1.5s";
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
                    Quaternion Angle_in_Quaternion = Shoot(first_enemy);
                    MuzzleEffect(Angle_in_Quaternion);
                    Clock = 0f;
                    didAttackImmediately = false;
                }
            }
            else
            {
                Clock = Cooldown;
            }
        }
    }
    public void AttackImmediately()
    {
        if (isStunned || didAttackImmediately || Level < 3) { return; }
        BaseEnemy first_enemy = FindFirstEnemy();
        if (first_enemy != null)
        {
            SelfRotate(first_enemy);
            Quaternion Angle_in_Quaternion = Shoot(first_enemy);
            MuzzleEffect(Angle_in_Quaternion);
            Clock = 0f;
            didAttackImmediately = true;
        }
    }
}

/* Bắn chuỗi là: 
 * khi bắn 1 mục tiêu nếu kết liễu mục tiêu thì sẽ ngay lập tức thực hiện lại hàm bắn mục tiêu
 * vấn đề: cần 1 khoảng thời gian để chờ thông tin kết liễu. mong muốn là khi mục tiêu bị kết liễu thì ngay lập tức thực hiện hàm
 */
