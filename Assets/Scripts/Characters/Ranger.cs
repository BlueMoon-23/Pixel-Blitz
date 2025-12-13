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
        UpgradeCost = new float[] { 900, 4250, 6750, 15000 };
        SellCost = (int)(Cost / 3);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override float GetRange()
    {
        if (Range <= 12f) { return 12f; } // <= la chua duoc khoi tao
        else return Range;
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
        Cooldown = 3f;
        Damage = 150f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Range = 25f;
        Cooldown = 7f;
        Damage = 1500f;
        Level = 4;
        // Self-Clone
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
                    characterUI.Info1.text = "Damage: 65 => 150";
                    characterUI.Info2.text = "Cooldown: 3.5s => 3s";
                    characterUI.Info3.text = "";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Wild Exceptional";
                    characterUI.Info1.text = "Damage: 150 => 1500";
                    characterUI.Info2.text = "Range: 15 => 25";
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

}
