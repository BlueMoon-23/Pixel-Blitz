using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pulser : GroundCharacter
{
    public GameObject PulseBar;
    private float currentPulse = 0f;
    private float MaxPulse;
    private bool reachedMaxPulse;
    private float ChargeTime = 4f;
    private float Original_x_PulseBarScale;
    private GameObject currentLaser;
    protected override void OnEnable()
    {
        base.OnEnable();
        Range = 8f;
        Damage = 3f;
        Cooldown = 0.1f;
        Cost = 4250;
        Level = 0;
        currentPulse = 0f;
        ChargeTime = 4f;
        hasHiddenDetection = false;
        canStrikethrough = true;
        reachedMaxPulse = false;
        UpgradeCost = new float[] { 2500, 7000, 20700, 57000 };
        SellCost = (int)(Cost / 3);
        MaxPulse = 150f;
        // Grave Bar
        Original_x_PulseBarScale = 4.5f;
        // Update
        PulseBar.transform.localScale = new Vector3(Original_x_PulseBarScale * currentPulse / MaxPulse, PulseBar.transform.localScale.y, PulseBar.transform.localScale.z);
        // Instantiate cục laser và tắt nó đi. Tái chế nó
        currentLaser = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, Quaternion.identity);
        currentLaser.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            if (!isStunned && !reachedMaxPulse) 
            { 
                AttackWithoutAnimation(); 
            }
            else
            {
                currentLaser.SetActive(false);
            }
        }
    }
    public override float GetRange()
    {
        if (Range <= 8f) { return 8f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 4250) { return 4250; }
        else return Cost;
    }
    public override void UpgradeToLevel1()
    {
        Range = 9;
        Damage = 5f;
        hasHiddenDetection = true;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        MaxPulse = 600f;
        Damage = 10f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Range = 12;
        MaxPulse = 1250f;
        Damage = 25f;
        ChargeTime = 3f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Range = 14;
        MaxPulse = 4000f;
        Damage = 75f;
        Level = 4;
        base.UpgradeToLevel4();
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            characterUI.characterName.text = "Pulser";
            characterUI.characterImage.sprite = characterUI.characterImages[6];
            switch (Level)
            {
                case 0:
                    {
                        characterUI.upgradeName.text = "Radar Integration";
                        characterUI.Info1.text = "Range: 8 => 9";
                        characterUI.Info2.text = "Damage: 3 => 5";
                        characterUI.Info3.text = "+ Hidden detection";
                        break;
                    }
                case 1:
                    {
                        characterUI.upgradeName.text = "Searing Stream";
                        characterUI.Info1.text = "Max Pulse: 150 => 600";
                        characterUI.Info2.text = "Damage: 5 => 10";
                        characterUI.Info3.text = "";
                        break;
                    }
                case 2:
                    {
                        characterUI.upgradeName.text = "Corrupted Raycaster";
                        characterUI.Info1.text = "Range: 9 => 12";
                        characterUI.Info2.text = "Damage: 10 => 25";
                        characterUI.Info3.text = "Max Pulse: 600 => 1250\nCharge Time: 4s => 3s";
                        break;
                    }
                case 3:
                    {
                        characterUI.upgradeName.text = "Jade Apocalypse";
                        characterUI.Info1.text = "Range: 12 => 14";
                        characterUI.Info2.text = "Damage: 25 => 75";
                        characterUI.Info3.text = "Max Pulse: 1250 => 4000";
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
        // Chỉ instantiate cái laser khi đang đánh. khi stop attack thì destroy cái đó đi
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
                    // Bắn đạn: lưu ý là truyền góc là hướng bắn của mình luôn chứ không dùng transform.rotation hay quaternion.identity
                    float Angle_in_Radian = Mathf.Atan2(first_enemy.Center.transform.position.y - transform.position.y, first_enemy.Center.transform.position.x - transform.position.x);
                    Quaternion Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
                    // Bật laser
                    currentLaser.SetActive(true);
                    // Gán headgun, gắn enemy cho laser
                    PulserLaser pulserLaser = currentLaser.GetComponent<PulserLaser>();
                    pulserLaser.HeadGun = Bullet_StartPosition;
                    pulserLaser.SetCharacter(this);
                    pulserLaser.SetEnemy(first_enemy);
                    // Tạo hiệu ứng nổ đạn (muzzle)
                    MuzzleEffect(Angle_in_Quaternion);
                    Clock = 0f;
                }
            }
            else
            {
                Clock = Cooldown;
                currentLaser.SetActive(false);
            }
        }
    }
    public void StackPulse(float damage)
    {
        currentPulse += damage;
        if (currentPulse >= MaxPulse)
        {
            currentPulse = 0f;
            StartCoroutine(StopAttack());
        }
        PulseBar.transform.localScale = new Vector3(Original_x_PulseBarScale * currentPulse / MaxPulse, PulseBar.transform.localScale.y, PulseBar.transform.localScale.z);
    }
    public IEnumerator StopAttack()
    {
        // Dừng tấn công trong 4s
        reachedMaxPulse = true;
        currentLaser.SetActive(false);
        yield return new WaitForSeconds(ChargeTime);
        reachedMaxPulse = false;
    }
    private void OnDisable()
    {
        if (currentLaser != null)
        {
            Destroy(currentLaser);
        }
    }
}
