using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigunner : GroundCharacter
{
    public GameObject ClonePrefab;
    private GameObject currentClone;
    void Start()
    {
        Range = 8f;
        Damage = 2f;
        Cooldown = 0.2f;
        Cost = 1850f;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = false;
        UpgradeCost = new float[] { 1150, 4500, 13500, 21500 };
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
        if (Range <= 8f) { return 8f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 1850f) { return 1850f; }
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
        Range = 12f;
        Damage = 8f;
        hasHiddenDetection = true;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Cooldown = 0.1f;
        Damage = 20f;
        Range = 15f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Damage = 30f;
        Level = 4;
        _hasAbility = true;
    }
    public override void SetUpgradeInformation()
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
                    characterUI.Info1.text = "Range: 8 => 12";
                    characterUI.Info2.text = "+ Hidden detection";
                    characterUI.Info3.text = "Damage: 4 => 8";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Optimized Caliber";
                    characterUI.Info1.text = "Range: 12 => 15";
                    characterUI.Info2.text = "Damage: 8 => 20";
                    characterUI.Info3.text = "Cooldown: 0.15s => 0.1s";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Futuristic Clone";
                    characterUI.Info1.text = "Damage: 20 => 30";
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
                if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Place_Sound);
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
                if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Place_Sound);
                currentClone = Instantiate(ClonePrefab, position, Quaternion.identity);
                CharacterManager.instance.AddPosition(position);
            }
        }
    }
    protected void OnDestroy()
    {
        if (currentClone != null) 
        {
            CharacterManager.instance.RemovePosition(currentClone.GetComponent<BaseCharacter>().transform.position);
            Destroy(currentClone); 
        }
    }
}
