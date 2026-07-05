using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MinigunnerClone : Minigunner
{
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            characterUI.characterName.text = "Minigunner";
            characterUI.characterImage.sprite = characterUI.characterImages[2];
            characterUI.upgradeName.text = "";
            CharacterUIControll.instance.TurnOffAllInfo();
            characterUI.upgradeCost.text = "Can't Upgrade";
            characterUI.sellCost.text = "Sell ($0)";
            characterUI.RangeStats.text = Range.ToString();
            characterUI.DamageStats.text = Damage.ToString();
            characterUI.CooldownStats.text = Cooldown.ToString();
            characterUI.HiddenDetectionIcon.alpha = (hasHiddenDetection) ? 1f : 0f;
            characterUI.StrikethroughIcon.alpha = (canStrikethrough) ? 1f : 0f;
        }
    }
}
