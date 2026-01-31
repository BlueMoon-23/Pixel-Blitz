using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MinigunnerClone : Minigunner
{
    void Start()
    {
        Cooldown = 0.1f;
        Damage = 10f;
        Range = 12f;
        Level = 3;
        Cost = 0f;
        hasHiddenDetection = true;
        canStrikethrough = false;
        UpgradeCost = new float[] { 0, 0, 0, 0 };
        SellCost = 0;
        _hasAbility = false;
    }
    public override float GetRange()
    {
        if (Range <= 12f) { return 12f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 0f) { return 0; }
        else return Cost;
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            characterUI.characterName.text = "Minigunner";
            characterUI.characterImage.sprite = characterUI.characterImages[2];
            characterUI.upgradeName.text = "";
            characterUI.Info1.text = "";
            characterUI.Info2.text = "";
            characterUI.Info3.text = "";
            characterUI.upgradeCost.text = "Can't Upgrade";
            characterUI.sellCost.text = "Sell ($0)";
            characterUI.RangeStats.text = Range.ToString();
            characterUI.DamageStats.text = Damage.ToString();
            characterUI.CooldownStats.text = Cooldown.ToString();
            if (hasHiddenDetection)
            {
                characterUI.HiddenDetectionIcon.alpha = 1f;
            }
            else
            {
                characterUI.HiddenDetectionIcon.alpha = 0f;
            }
            if (canStrikethrough)
            {
                characterUI.StrikethroughIcon.alpha = 1f;
            }
            else
            {
                characterUI.StrikethroughIcon.alpha = 0f;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isStunned) { AttackWithoutAnimation(); }
        // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
    }
}
