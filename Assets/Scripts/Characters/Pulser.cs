using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pulser : GroundCharacter
{
    public GameObject PulseBar;
    private float currentPulse = 0f;
    private float MaxPulse;
    private float Original_x_PulseBarScale;
    private GameObject currentLaser;
    void Start()
    {
        Range = 8f;
        Damage = 5f;
        Cooldown = 0.1f;
        Cost = 4250f;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = true;
        UpgradeCost = new float[] { 1500, 7000, 16700, 33000 };
        SellCost = (int)(Cost / 3);
        MaxPulse = 250f;
        // Grave Bar
        Original_x_PulseBarScale = PulseBar.transform.localScale.x;
        // Update
        PulseBar.transform.localScale = new Vector3(Original_x_PulseBarScale * currentPulse / MaxPulse, PulseBar.transform.localScale.y, PulseBar.transform.localScale.z);
        // Instantiate cục laser và tắt nó đi. Tái chế nó
        currentLaser = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, Quaternion.identity);
        currentLaser.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned) { AttackWithoutAnimation(); }
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
        hasHiddenDetection = true;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        MaxPulse = 350f;
        Damage = 10f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Range = 12;
        MaxPulse = 1000f;
        Damage = 25f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Range = 14;
        MaxPulse = 5000f;
        Damage = 100f;
        Level = 4;
    }
    public override void SetUpgradeInformation()
    {
        characterUI.characterName.text = "Pulser";
        characterUI.characterImage.sprite = characterUI.characterImages[6];
        switch (Level)
        {
            case 0:
                {
                    characterUI.upgradeName.text = "Radar Integration";
                    characterUI.Info1.text = "Range: 8 => 9";
                    characterUI.Info2.text = "+ Hidden detection";
                    characterUI.Info3.text = "";
                    break;
                }
            case 1:
                {
                    characterUI.upgradeName.text = "Searing Stream";
                    characterUI.Info1.text = "Max Pulse: 250 => 350";
                    characterUI.Info2.text = "Damage: 5 => 10";
                    characterUI.Info3.text = "";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Lethal Rays";
                    characterUI.Info1.text = "Range: 9 => 12";
                    characterUI.Info2.text = "Damage: 10 => 25";
                    characterUI.Info3.text = "Max Pulse: 800 => 1000";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Jade Rain";
                    characterUI.Info1.text = "Range: 12 => 14";
                    characterUI.Info2.text = "Damage: 25 => 100";
                    characterUI.Info3.text = "Max Pulse: 1000 => 5000";
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
                if (first_enemy != null && !first_enemy.isDieOrNot())
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
        isStunned = true;
        currentLaser.SetActive(false);
        yield return new WaitForSeconds(4f);
        isStunned = false;
    }
    private void OnDestroy()
    {
        if (currentLaser.gameObject != null)
        {
            Destroy(currentLaser.gameObject);
        }
    }
}
