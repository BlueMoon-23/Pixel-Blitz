using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wizard : BaseCharacter
{
    // Start is called before the first frame update
    // Base
    public GameObject BaseFireball_Prefab;
    public GameObject BaseFireball_Muzzle;
    // Star Sequence
    public GameObject StarSequence_Prefab;
    public GameObject LowGraphic_StarSequence_Prefab;
    public GameObject StarSequence_Muzzle;
    // Astral Vortex
    public GameObject Vortex_Prefab;
    public GameObject Vortex_Muzzle;
    // Fiery Wrath
    public GameObject SharpFireball_Prefab;
    public GameObject SharpFireball_Muzzle;
    // Combo Serialization
    public int[] SkillOrderID = { 1, 1, 1 }; // 3 giá trị chỉ nhận trong khoảng 1, 2, 3
    protected override void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < SkillOrderID.Length; i++)
        {
            SkillOrderID[i] = 1;
        }
    }
    public override float GetDamage()
    {
        return WeaponCalculator.CalculateDamage(profile.characterLevelDatas[Level].DamageStat, characterWeapon.WeaponEquipped); // damage con này khi lên lv4 sẽ hiện lên rất kỳ, nên phải dùng cơ chế đa hình để đảm bảo sát thương của fireball
    }
    public void SetDamage(float UIDamage) { Damage = UIDamage; }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            // Chưa nghĩ ra cách nào khác để đưa đúng tinh thần open/closed cho việc cài description current ability theo đúng damage
            profile.characterLevelDatas[1].Special = "Next Ability: cast a line of 5 stars under enemy's feet which deals " + WeaponCalculator.CalculateDamage(profile.characterLevelDatas[1].DamageStat, characterWeapon.WeaponEquipped) + " damage and stun for 1s.";
            profile.characterLevelDatas[2].Special = "Next Ability: create a vortex at enemy that exists in 3 seconds, each 0.1s deals " + WeaponCalculator.CalculateDamage(profile.characterLevelDatas[2].DamageStat, characterWeapon.WeaponEquipped) + " damage.";
            profile.characterLevelDatas[3].Special = "Next Ability: cast 3 fireballs in burst. Each fireball explodes and deals " + WeaponCalculator.CalculateDamage(profile.characterLevelDatas[3].DamageStat, characterWeapon.WeaponEquipped) + " damage to all enemies hit.";
            profile.characterLevelDatas[4].Special = "Next Ability: use 3 abilities chosen by the player in burst!";
            base.SetUpgradeInformation();
        }
    }
    public override IEnumerator AttackWithAnimation(float Attack_Duration)
    {
        BaseEnemy first_enemy = characterAttack.FindFirstEnemy();
        if (first_enemy != null)
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
        //Instantiate(StarSequence_Prefab, SpawnPosition, Angle_in_Quaternion);
        GameObject chosenExplosion_SFX = StarSequence_Prefab;
        if (GameSetting.instance != null && !GameSetting.instance._showExplosion)
        {
            chosenExplosion_SFX = LowGraphic_StarSequence_Prefab;
        }
        if (ExplosionPooler.instance != null)
        {
            BaseExplosion stars = ExplosionPooler.instance.GetExplosion(chosenExplosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
            if (stars != null)
            {
                stars.transform.position = SpawnPosition;
                stars.transform.rotation = Angle_in_Quaternion;
                StarSequence starSequence = stars.GetComponent<StarSequence>();
                if (starSequence != null)
                {
                    starSequence.Initialize(this, WeaponCalculator.CalculateDamage(profile.characterLevelDatas[1].DamageStat, characterWeapon.WeaponEquipped));
                }
            }
        }
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
        //Instantiate(Vortex_Prefab, SpawnPosition, Quaternion.identity);
        if (ExplosionPooler.instance != null)
        {
            BaseExplosion vortex = ExplosionPooler.instance.GetExplosion(Vortex_Prefab.GetComponent<BaseExplosion>().ExplosionID);
            if (vortex != null)
            {
                vortex.transform.position = SpawnPosition;
                vortex.transform.rotation = Quaternion.identity;
                WizardVortex wizardVortex = vortex.GetComponent<WizardVortex>();
                if (wizardVortex != null)
                {
                    wizardVortex.Initialize(this, WeaponCalculator.CalculateDamage(profile.characterLevelDatas[2].DamageStat, characterWeapon.WeaponEquipped));
                }
            }
            else
            {
                Debug.Log("vortex = null");
            }
        }
        GameObject muzzle = Instantiate(Vortex_Muzzle, Bullet_StartPosition.transform.position, Angle_in_Quaternion);
        Destroy(muzzle, 0.25f);
    }
    private IEnumerator CastBurstFireball()
    {
        bullet_Prefab = SharpFireball_Prefab;
        BulletMuzzle = SharpFireball_Muzzle;
        for (int i = 1; i <= 3; i++)
        {
            BaseEnemy first_enemy = characterAttack.FindFirstEnemy();
            if (first_enemy != null)
            {
                SelfRotate(first_enemy);
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
            BaseEnemy first_enemy = characterAttack.FindFirstEnemy();
            if (first_enemy != null)
            {
                SelfRotate(first_enemy);
                //ChooseRandomAbility(first_enemy);
                DoAbilityByID(first_enemy, SkillOrderID[i]);
            }
            yield return new WaitForSeconds(0.35f);
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
        DragAbility.instance.SetDragType(DragAbility.AbilityDragType.None);
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
