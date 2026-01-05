using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wizard : GroundCharacter
{
    // Start is called before the first frame update
    // Base
    public GameObject BaseFireball_Prefab;
    public GameObject BaseFireball_Muzzle;
    // Star Sequence
    public GameObject StarSequence_Prefab;
    public GameObject StarSequence_Muzzle;
    // Astral Vortex
    public GameObject Vortex_Prefab;
    public GameObject Vortex_Muzzle;
    // Fiery Wrath
    public GameObject SharpFireball_Prefab;
    public GameObject SharpFireball_Muzzle;
    // Combo Serialization
    public int[] SkillOrderID = { 1, 2, 3 }; // 3 giá trị chỉ nhận trong khoảng 1, 2, 3
    void Start()
    {
        Range = 7f;
        Damage = 6f;
        Cooldown = 4f;
        Cost = 600;
        Level = 0;
        hasHiddenDetection = true;
        canStrikethrough = true;
        UpgradeCost = new float[] { 3650, 12500, 24000, 50000 };
        SellCost = (int)(Cost / 3);
        _hasAbility = false;
        Staff_Attack_Duration = 0.417f;
    }

    // Update is called once per frame
    void Update()
    {
        float min_duration = Staff_Attack_Duration < Cooldown ? Staff_Attack_Duration : Cooldown;
        if (!isStunned) { AttackWithCooldown(min_duration); }
        // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
    }
    public override float GetRange()
    {
        if (Range <= 7f) { return 7f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 600) { return 600; }
        else return Cost;
    }
    public override void UpgradeToLevel1()
    {
        Damage = 50f;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Range = 9f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Damage = 500f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Range = 11f;
        _hasAbility = true;
        Level = 4;
    }
    public override void SetUpgradeInformation()
    {
        characterUI.characterName.text = "Wizard";
        characterUI.characterImage.sprite = characterUI.characterImages[7]; // Copy paste nhớ chỉnh ở đây dùm con
        switch (Level)
        {
            case 0:
                {
                    characterUI.upgradeName.text = "Star Sequence";
                    characterUI.Info1.text = "Damage: 6 => 25";
                    characterUI.Info2.text = "Current Ability: cast a line of 5 stars under enemy's feet which deals 50 damage and stun for 1s.";
                    characterUI.Info3.text = "";
                    break;
                }
            case 1:
                {
                    characterUI.upgradeName.text = "Astral Vortex";
                    characterUI.Info1.text = "Range: 7 => 9";
                    characterUI.Info2.text = "Current Ability: create a vortex at enemy that exists in 3 seconds, each 0.1s deals 50 damage.";
                    characterUI.Info3.text = "";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Fiery Wrath";
                    characterUI.Info1.text = "Damage: 50 => 500, only apply for fireballs";
                    characterUI.Info2.text = "Current Ability: cast 3 fireballs in burst. Each fireball explodes and deals 500 damage to all enemies hit.";
                    characterUI.Info3.text = "Its explosion extends the duration of Astral Vortex by 0.5s";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Combo Serialization";
                    characterUI.Info1.text = "Range: 9 => 11";
                    characterUI.Info2.text = "Current Ability: use 3 abilities chosen by the player in burst!";
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
    public override IEnumerator AttackWithAnimation(float Attack_Duration)
    {
        BaseEnemy first_enemy = FindFirstEnemy();
        if (first_enemy != null && !first_enemy.isDieOrNot())
        {
            Vector3 SpawnPosition = first_enemy.transform.position; // Spawn dưới chân nên không cần center
            SelfRotate(first_enemy);
            PlayAttackAmination(Attack_Duration);
            yield return new WaitForSeconds(Attack_Duration / 2);
            // Phần chính: thực hiện Ability theo level
            switch (Level)
            {
                case 1:
                    {
                        CastStarSequence(SpawnPosition);
                        break;
                    }
                case 2:
                    {
                        CastVortex(SpawnPosition);
                        break;
                    }
                case 3:
                    {
                        StartCoroutine(CastBurstFireball()); // mỗi lần burst cần tính toán lại first_enemy
                        break;
                    }
                case 4:
                    {
                        StartCoroutine(BurstAbility());
                        break;
                    }
                default: // base level ở đây
                    {
                        CastBaseFireball(first_enemy);
                        break;
                    }
            }
            yield return new WaitForSeconds(Attack_Duration / 2);
            SPUM_Prefabs._anim.speed = 1;
        }
    }
    private void CastBaseFireball(BaseEnemy first_enemy) // Để tránh phải copy logic gốc thì ta truyền loại đạn cần chơi qua bullet_prefab và muzzle_prefab
    {
        bullet_Prefab = BaseFireball_Prefab;
        BulletMuzzle = BaseFireball_Muzzle;
        Quaternion Angle_in_Quaternion = Shoot(first_enemy);
        MuzzleEffect(Angle_in_Quaternion);
    }
    private void CastStarSequence(Vector3 SpawnPosition) // Phải copy do instantiate ở dưới chân enemy chứ không phải từ headgun
    {
        Quaternion Angle_in_Quaternion = Quaternion.identity;
        if (SpawnPosition != Vector3.zero)
        {
            float Angle_in_Radian = Mathf.Atan2(SpawnPosition.y - transform.position.y, SpawnPosition.x - transform.position.x);
            Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
        }
        Instantiate(StarSequence_Prefab, SpawnPosition, Angle_in_Quaternion);
        GameObject muzzle = Instantiate(StarSequence_Muzzle, Bullet_StartPosition.transform.position, Angle_in_Quaternion);
        Destroy(muzzle, 0.25f);
    }
    private void CastVortex(Vector3 SpawnPosition) // Phải copy do instantiate ở dưới chân enemy chứ không phải từ headgun
    {
        Quaternion Angle_in_Quaternion = Quaternion.identity;
        if (SpawnPosition != Vector3.zero)
        {
            float Angle_in_Radian = Mathf.Atan2(SpawnPosition.y - transform.position.y, SpawnPosition.x - transform.position.x);
            Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
        }
        Instantiate(Vortex_Prefab, SpawnPosition, Quaternion.identity);
        GameObject muzzle = Instantiate(Vortex_Muzzle, Bullet_StartPosition.transform.position, Angle_in_Quaternion);
        Destroy(muzzle, 0.25f);
    }
    private IEnumerator CastBurstFireball()
    {
        for (int i = 1; i <= 3; i++)
        {
            BaseEnemy first_enemy = FindFirstEnemy();
            if (first_enemy != null && !first_enemy.isDieOrNot())
            {
                SelfRotate(first_enemy);
                bullet_Prefab = SharpFireball_Prefab;
                BulletMuzzle = SharpFireball_Muzzle;
                Quaternion Angle_in_Quaternion = Shoot(first_enemy);
                MuzzleEffect(Angle_in_Quaternion);
            }
            yield return new WaitForSeconds(0.25f);
        }
        yield break;
    }
    private IEnumerator BurstAbility()
    {
        for (int i = 0; i < 3; i++)
        {
            BaseEnemy first_enemy = FindFirstEnemy();
            if (first_enemy != null && !first_enemy.isDieOrNot())
            {
                SelfRotate(first_enemy);
                //ChooseRandomAbility(first_enemy);
                DoAbilityByID(first_enemy, SkillOrderID[i]);
            }
            yield return new WaitForSeconds(0.25f);
        }
        yield break;
    }
    private void DoAbilityByID(BaseEnemy first_enemy, int ID)
    {
        Vector3 SpawnPosition = first_enemy.transform.position; // Spawn dưới chân nên không cần center
        switch (ID)
        {
            case 1:
                {
                    CastStarSequence(SpawnPosition);
                    break;
                }
            case 2:
                {
                    CastVortex(SpawnPosition);
                    break;
                }
            case 3:
                {
                    StartCoroutine(CastBurstFireball()); // mỗi lần burst cần tính toán lại first_enemy
                    break;
                }
        }
    }
    public override void SetAbilityIcon()
    {
        characterUI.AbilityCurrentIcon.sprite = characterUI.AbilityIcons[2];
        DragAbility.instance.currentDragType = DragAbility.AbilityDragType.None;
        DragAbility.instance.enabled = false;
    }
    public override void Ability(Vector3 position)
    {
        if (WizardComboCustomizer.instance != null)
        {
            WizardComboCustomizer.instance.SetCurrentWizard(this);
            CanvasGroup canvasGroup = WizardComboCustomizer.instance.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            WizardComboCustomizer.instance.ShowCurrentWizardSkillOrder();
        }
        else
        {
            Debug.Log("WizardComboCustomizer.instance = null");
        }
    }
}
