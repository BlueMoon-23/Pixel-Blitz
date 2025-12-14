using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : GroundCharacter
{
    private float _FreezeTime;
    public float FreezeTime
    {
        get { return _FreezeTime; }
    }
    private int _FreezeCount;
    public int FreezeCount
    {
        get { return _FreezeCount; }
    }
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
        _FreezeTime = 0.5f;
        _FreezeCount = 3;
    }

    // Update is called once per frame
    void Update()
    {
        AttackWithoutAnimation();
    }
    public override float GetRange()
    {
        if (Range <= 5f) { return 5f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 650f) { return 650f; }
        else return Cost;
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
        _FreezeTime = 1f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        canStrikethrough = true;
        _FreezeCount = 2;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
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
                    characterUI.upgradeName.text = "Piercing Shards";
                    characterUI.Info1.text = "Freeze hit count: 3 => 2";
                    characterUI.Info2.text = "+ Strikethrough";
                    characterUI.Info3.text = "";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Glacial Glowing";
                    characterUI.Info1.text = "Bullets now explode";
                    characterUI.Info2.text = "Damage: 2 => 5";
                    characterUI.Info3.text = "";
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
    public override void AttackWithoutAnimation()
    {
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            StartCoroutine(Burst());
            Clock = 0f;
        }
    }
    private IEnumerator Burst()
    {
        for (int i = 1; i <= 3; i++)
        {
            BaseEnemy first_enemy = FindFirstEnemy();
            if (first_enemy != null)
            {
                if (first_enemy.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                else
                {
                    transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                GameObject newBullet = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, transform.rotation);
                if (Level < 3)
                {
                    newBullet.transform.localScale *= 0.5f;
                }
                BaseBullets bullet = newBullet.GetComponent<BaseBullets>();
                bullet.SetCharacter(this);
                bullet.SetEnemy(first_enemy);
            }
            yield return new WaitForSeconds(0.25f);
        }
        yield break;
    }
}
