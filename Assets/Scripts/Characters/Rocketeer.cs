using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketeer : CliffCharacter
{
    private float ExplosionRadius;
    void Start()
    {
        Range = 10f;
        Damage = 12f;
        Cooldown = 3f;
        Cost = 1000;
        ExplosionRadius = 1;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = true;
        UpgradeCost = new float[] { 1500, 3300, 9500, 19500 };
        SellCost = (int)(Cost / 3);
        _hasAbility = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned) { AttackWithoutAnimation(); }
        // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
    }
    public override float GetRange()
    {
        if (Range <= 10f) { return 10f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 1000) { return 1000; }
        else return Cost;
    }
    public float GetExplosionRadius() { return ExplosionRadius; }
    public override void UpgradeToLevel1()
    {
        Damage = 25f;
        ExplosionRadius = 1.5f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Range = 12f;
        Damage = 45f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Damage = 105f;
        Range = 15f;
        ExplosionRadius = 2.5f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Damage = 450f;
        Level = 4;
    }
    public override void SetUpgradeInformation()
    {
        characterUI.characterName.text = "Rocketeer";
        characterUI.characterImage.sprite = characterUI.characterImages[4]; // Copy paste nhớ chỉnh ở đây dùm con
        switch (Level)
        {
            case 0:
                {
                    characterUI.upgradeName.text = "Blast Off";
                    characterUI.Info1.text = "Damage: 12 => 25";
                    characterUI.Info2.text = "Explosion Radius: 1 => 1.5";
                    characterUI.Info3.text = "";
                    break;
                }
            case 1:
                {
                    characterUI.upgradeName.text = "Long-Frontal Fire";
                    characterUI.Info1.text = "Range: 10 => 12";
                    characterUI.Info2.text = "Damage: 25 => 45";
                    characterUI.Info3.text = "";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Seismic Crash";
                    characterUI.Info1.text = "Range: 12 => 15";
                    characterUI.Info2.text = "Damage: 45 => 105";
                    characterUI.Info3.text = "Explosion Radius: 1.5 => 2.5";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Heart on Fire Fragments";
                    characterUI.Info1.text = "Damage: 105 => 450";
                    characterUI.Info2.text = "Rockets now launch 4 bombs on its impact, each deals 100 damage";
                    characterUI.Info3.text = "";
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
