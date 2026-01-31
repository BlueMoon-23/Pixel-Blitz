using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigunner : GroundCharacter
{
    public GameObject ClonePrefab;
    private GameObject currentClone;
    protected override void OnEnable()
    {
        base.OnEnable();
        Range = 8f;
        Damage = 2f;
        Cooldown = 0.2f;
        Cost = 1850f;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = false;
        UpgradeCost = new float[] { 2800, 3900, 10000, 25500 };
        SellCost = (int)(Cost / 3);
        _hasAbility = false;
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
        if (Range <= 8f) { return 8f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 1850) { return 1850; }
        else return Cost;
    }
    public override void UpgradeToLevel1()
    {
        Cooldown = 0.15f;
        Damage = 4f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Range = 10f;
        Damage = 7f;
        hasHiddenDetection = true;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Cooldown = 0.1f;
        Damage = 10f;
        Range = 12f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Damage = 15f;
        Level = 4;
        _hasAbility = true;
        base.UpgradeToLevel4();
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            characterUI.characterName.text = "Minigunner";
            characterUI.characterImage.sprite = characterUI.characterImages[2];
            switch (Level)
            {
                case 0:
                    {
                        characterUI.upgradeName.text = "Weight Adaptation";
                        characterUI.Info1.text = "Damage: 2 => 4";
                        characterUI.Info2.text = "Cooldown: 0.2s => 0.15s";
                        characterUI.Info3.text = "";
                        break;
                    }
                case 1:
                    {
                        characterUI.upgradeName.text = "Eye Spy";
                        characterUI.Info1.text = "Range: 8 => 10";
                        characterUI.Info2.text = "Damage: 4 => 7";
                        characterUI.Info3.text = "+ Hidden detection";
                        break;
                    }
                case 2:
                    {
                        characterUI.upgradeName.text = "Optimized Caliber";
                        characterUI.Info1.text = "Range: 10 => 12";
                        characterUI.Info2.text = "Damage: 7 => 10";
                        characterUI.Info3.text = "Cooldown: 0.15s => 0.1s";
                        break;
                    }
                case 3:
                    {
                        characterUI.upgradeName.text = "Futuristic Clone";
                        characterUI.Info1.text = "Damage: 10 => 15";
                        characterUI.Info2.text = "+ Clone Ability: spawn a minigunner with futuristic equipment.";
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
    public override void SetAbilityIcon()
    {
        characterUI.AbilityCurrentIcon.sprite = characterUI.AbilityIcons[0];
        DragAbility.instance.currentDragType = DragAbility.AbilityDragType.GroundPlacement;
    }
    public override void Ability(Vector3 position)
    {
        if (currentClone == null)
        {
            BaseCharacter character = ClonePrefab.GetComponent<BaseCharacter>();
            if (position != Vector3.zero && !(CharacterManager.instance.hasCharacterinPosition(position)))
            {
                if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
                currentClone = Instantiate(ClonePrefab, position, Quaternion.identity);
                CharacterManager.instance.AddPosition(position);
            }
        }
        else
        {
            CharacterManager.instance.RemovePosition(currentClone.GetComponent<BaseCharacter>().transform.position);
            Destroy(currentClone);
            if (position != Vector3.zero && !(CharacterManager.instance.hasCharacterinPosition(position)))
            {
                if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
                currentClone = Instantiate(ClonePrefab, position, Quaternion.identity);
                CharacterManager.instance.AddPosition(position);
            }
        }
    }
    protected void OnDisable()
    {
        if (currentClone != null) 
        {
            CharacterManager.instance.RemovePosition(currentClone.GetComponent<BaseCharacter>().transform.position);
            Destroy(currentClone); 
        }
    }
}
