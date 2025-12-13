using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigunner : GroundCharacter
{
    void Start()
    {
        Range = 8f;
        Damage = 2f;
        Cooldown = 0.15f;
        Cost = 2000f;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = false;
        UpgradeCost = new float[] { 500, 1000, 7000, 10000 };
        SellCost = (int)(Cost / 3);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override float GetRange()
    {
        if (Range <= 8f) { return 8f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override void UpgradeToLevel1()
    {
        Cooldown = 0.1f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Range = 12f;
        hasHiddenDetection = true;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Cooldown = 0.05f;
        Damage = 8f;
        Range = 15f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Level = 4;
        // Self-Clone
    }
    public override void SetUpgradeInformation()
    {
        characterUI.characterName.text = "Minigunner";
        characterUI.characterImage.sprite = characterUI.characterImages[2];
        switch (Level)
        {
            case 0:
                {
                    characterUI.upgradeName.text = "Better Handling";
                    characterUI.Info1.text = "Cooldown: 0.15s => 0.1s";
                    characterUI.Info2.text = "";
                    characterUI.Info3.text = "";
                    break;
                }
            case 1:
                {
                    characterUI.upgradeName.text = "Eye Spy";
                    characterUI.Info1.text = "Range: 8 => 12";
                    characterUI.Info2.text = "+ Hidden detection";
                    characterUI.Info3.text = "";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Optimized Caliber";
                    characterUI.Info1.text = "Range: 12 => 15";
                    characterUI.Info2.text = "Damage: 2 => 8";
                    characterUI.Info3.text = "Cooldown: 0.1s => 0.05s";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Self-Clone";
                    characterUI.Info1.text = "+ Clone Ability:";
                    characterUI.Info2.text = "spawn a level 3 minigunner";
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
