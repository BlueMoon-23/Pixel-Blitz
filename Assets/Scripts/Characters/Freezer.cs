using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : GroundCharacter
{
    private float FreezeTime;
    void Start()
    {
        Range = 5f;
        Damage = 1f;
        Cooldown = 3f;
        Cost = 650f;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = false;
        UpgradeCost = new float[] { 600, 800, 1750, 5000 };
        SellCost = (int)(Cost / 3);
        FreezeTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override float GetRange()
    {
        if (Range <= 5f) { return 5f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override void UpgradeToLevel1()
    {
        Damage = 2f;
        Cooldown = 2f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Range = 7f;
        FreezeTime = 1f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        canStrikethrough = true;
        // Freeze hit count: 3 => 2 o script enemy
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Cooldown = 1f;
        Damage = 5f;
        Level = 4;
        // Explode
    }
    public override void SetUpgradeInformation()
    {
        characterUI.characterName.text = "Freezer";
        characterUI.characterImage.sprite = characterUI.characterImages[1];
        switch (Level)
        {
            case 1:
                {
                    characterUI.upgradeName.text = "Frost shot";
                    characterUI.Info1.text = "Range: 5 => 7";
                    characterUI.Info2.text = "Freeze time: 0.5s => 1s";
                    characterUI.Info3.text = "";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Ice Blade";
                    characterUI.Info1.text = "Freeze hit count: 3 => 2";
                    characterUI.Info2.text = "+ Strikethrough";
                    characterUI.Info3.text = "";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Snowy Deluge";
                    characterUI.Info1.text = "Bullets now explode";
                    characterUI.Info2.text = "Damage: 2 => 5";
                    characterUI.Info3.text = "Cooldown: 2 => 1";
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
                    characterUI.upgradeName.text = "Flash Freeze";
                    characterUI.Info1.text = "Cooldown: 3 => 2";
                    characterUI.Info2.text = "Damage: 1 => 2";
                    characterUI.Info3.text = "";
                    break;
                }
        }
        base.SetUpgradeInformation();
    }

}
