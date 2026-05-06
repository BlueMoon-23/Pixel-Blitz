using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class BaseCharacter : MonoBehaviour
{
    // Basic stats
    protected float Range = 1; // Range = 1 <=> tầm bắn là hình tròn nằm trong 1 ô tilemap
    protected float Damage;
    protected float Cooldown;
    protected float Clock;
    protected float Cost;
    protected float[] UpgradeCost;
    protected float SellCost;
    protected int Level;
    protected bool isCliff;
    protected bool hasHiddenDetection;
    protected bool canStrikethrough;
    protected bool _hasAbility = false;
    public bool hasAbility
    {
        get { return _hasAbility; }
    }
    // Other references
    public GameObject Range_Prefab;
    protected CharacterUIControll characterUI;
    protected Vector3 CircleScale;
    protected RangeScript range;
    public GameObject bullet_Prefab;
    // Max level Change
    public GameObject OriginalUnitRoot;
    public GameObject MaxLevelUnitRoot;
    // Attack animation
    protected SPUM_Prefabs SPUM_Prefabs;
    public Dictionary<PlayerState, int> IndexPair = new();
    protected float Bow_Attack_Duration;
    protected float Staff_Attack_Duration;
    public GameObject Bullet_StartPosition;
    public GameObject BulletMuzzle;
    // Stunned Effect
    public GameObject StunnedEffect;
    protected bool isStunned = false;
    protected float stunEndTime;
    // Kiểm tra đã reset hay chưa
    public bool StatsReseted = false; // PHẢI MẶC ĐỊNH LÀ FALSE, TRUE LÀ UPDATE SẼ CHẠY TRƯỚC LÀ SẼ BÁO NULL
    // Báo cáo nhiệm vụ
    public Action<int> OnLevelUp;
    protected virtual void OnEnable()
    {
        // Chỉnh lại một số stats
        StatsReseted = false; // khóa update/move cho đến khi xong stats
        isStunned = false;
        stunEndTime = Time.time;
        StartCoroutine(ResetStats());
    }
    protected IEnumerator ResetStats()
    {
        yield return null;
        CircleScale = new Vector3(0.25f, 0.25f, 0.25f);
        SetRangeCircle();
        characterUI = FindObjectOfType<CharacterUIControll>(true);
        range = Range_Prefab.GetComponent<RangeScript>();
        Clock = Cooldown - 0.1f;
        // Animation. Cấm xóa đoạn này
        SPUM_Prefabs = GetComponent<SPUM_Prefabs>();
        if (SPUM_Prefabs == null)
        {
            SPUM_Prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
            if (!SPUM_Prefabs.allListsHaveItemsExist())
            {
                SPUM_Prefabs.PopulateAnimationLists();
            }
        }
        SPUM_Prefabs.OverrideControllerInit();
        foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
        {
            IndexPair[state] = 0;
        }
        // TẮT CÁI HIỆU ỨNG MAXLEVEL ROOT. AI ĐÓ ÉP NÓ PHẢI BẬT TAO KHÔNG TÌM RA ĐƯỢC
        StartCoroutine(ResetShadow());
        StatsReseted = true;
    }
    // Abstract methods
    public abstract float GetCost();
    public abstract void UpgradeToLevel1();
    public abstract void UpgradeToLevel2();
    public abstract void UpgradeToLevel3();
    public abstract void UpgradeToLevel4();
    public abstract void SetAbilityIcon();
    public abstract void Ability(Vector3 position);
    // Normal methods
    public virtual float GetRange() { return 1f; }
    public bool hasHiddenDetectionOrNot()
    {
        return hasHiddenDetection;
    }
    public bool canStrikethroughOrNot()
    {
        return canStrikethrough;
    }
    public float GetDamage()
    {
        return Damage;
    }
    public float GetCooldown()
    {
        return Cooldown;
    }
    public int GetLevel()
    {
        return Level;
    }
    public float GetUpgradeCost(int level)
    {
        if (level < 4)
        {
            return UpgradeCost[level];
        }
        else return 0;
    }
    public float GetSellCost()
    {
        return SellCost;
    }
    protected void SetRangeCircle()
    {
        Range = GetRange();
        Range_Prefab.transform.localScale = CircleScale * Range;
        Range_Prefab.GetComponent<Renderer>().enabled = false;
    }
    public virtual void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            if (Level < 4)
            {
                characterUI.upgradeCost.text = "Upgrade (-$" + UpgradeCost[Level] + ")";
            }
            else
            {
                characterUI.upgradeCost.text = "Max Level";
            }
            characterUI.sellCost.text = "Sell (+$" + SellCost + ")";
            characterUI.RangeStats.text = Range.ToString();
            Wizard wizard = this as Wizard;
            if (wizard != null)
            {
                characterUI.DamageStats.text = wizard.GetVirtualDamage().ToString();
            }
            else
            {
                characterUI.DamageStats.text = Damage.ToString(); // damage bị lỗi từ bên wizard, nên sửa thêm
            }
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
    public void Upgrade()
    {
        switch (Level)
        {
            case 0:
                {
                    UpgradeToLevel1();
                    break;
                }
            case 1:
                {
                    UpgradeToLevel2();
                    break;
                }
            case 2:
                {
                    UpgradeToLevel3();
                    break;
                }
            case 3:
                {
                    UpgradeToLevel4();
                    break;
                }
            default:
                {
                    break;
                }
        }
        SetUpgradeInformation();
        Range = GetRange();
        Range_Prefab.transform.localScale = CircleScale * Range;
        for (int i = 0; i < Level; i++)
        {
            SellCost += (int)(UpgradeCost[i] / 3);
        }
        OnLevelUp?.Invoke(Level);
    }
    public BaseEnemy FindFirstEnemy()
    {
        float max_distance = 0f;
        for (int i = 0; i < range.enemies_in_range.Count; i++)
        {
            if (!range.enemies_in_range[i].isDieOrNot())
            {
                max_distance = Mathf.Max(max_distance, range.enemies_in_range[i].Distance);
            }
        }
        for (int i = 0; i < range.enemies_in_range.Count; i++)
        {
            if (max_distance == range.enemies_in_range[i].Distance && !range.enemies_in_range[i].isDieOrNot())
            {
                range.enemies_in_range[i].TakeIncomingDamage(Damage, canStrikethrough);
                return range.enemies_in_range[i];
            }
        }
        return null;
    }
    public List<BaseEnemy> FindThreeFirstEnemies()
    {
        // 2 1
        List<float> Enemy_Distances = new List<float>();
        for (int i = 0; i < range.enemies_in_range.Count; i++)
        {
            if (!range.enemies_in_range[i].isDieOrNot())
            {
                Enemy_Distances.Add(range.enemies_in_range[i].Distance);
            }
        }
        Enemy_Distances.Sort((a, b) => b.CompareTo(a));
        List<BaseEnemy> Enemies_Result = new List<BaseEnemy>();
        int Safe_Enemy_Distance_Index = 0;
        switch (Enemy_Distances.Count)
        {
            case 0:
            case 1:
            case 2:
                {
                    Safe_Enemy_Distance_Index = Enemy_Distances.Count; break;
                }
            default:
                {
                    Safe_Enemy_Distance_Index = 3;
                    break;
                }
        }
        for (int i = 0; i < Safe_Enemy_Distance_Index; i++)
        {
            for (int j = 0; j < range.enemies_in_range.Count; j++)
            {
                if (range.enemies_in_range[j].Distance == Enemy_Distances[i] && !range.enemies_in_range[j].isDieOrNot())
                {
                    Enemies_Result.Add(range.enemies_in_range[j]);
                    range.enemies_in_range[j].TakeIncomingDamage(Damage, canStrikethrough);
                }
            }
        }
        return Enemies_Result;
    }
    public virtual void AttackWithCooldown(float Attack_Duration)
    {
        // Logic cũ
        if (isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (range.enemies_in_range.Count != 0)
            {
                StartCoroutine(AttackWithAnimation(Attack_Duration));
                Clock = 0f;
            }
            else
            {
                Clock = Cooldown;
            }
        }
        // Vấn đề: khi ko có quái, không tấn công, nhưng bản chất attack vẫn được triển khai và lại quay về cooldown => không đúng
        // Mong muốn: quái khi chạm vào range thì sẽ tấn công, sau đó mới bắt đầu tính cooldown. nếu range hết quái thì clock tăng sau đó giữ nguyên ở cooldown, không kéo về 0f
    }
    public virtual IEnumerator AttackWithAnimation(float Attack_Duration)
    {
        BaseEnemy first_enemy = FindFirstEnemy();
        if (first_enemy != null)
        {
            SelfRotate(first_enemy);
            PlayAttackAmination(Attack_Duration);
            yield return new WaitForSeconds(Attack_Duration / 2);
            Quaternion Angle_in_Quaternion = Shoot(first_enemy);
            MuzzleEffect(Angle_in_Quaternion);
            yield return new WaitForSeconds(Attack_Duration / 2);
            SPUM_Prefabs._anim.speed = 1;
        }
    }
    public virtual void AttackWithoutAnimation()
    {
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
                    Quaternion Angle_in_Quaternion = Shoot(first_enemy);
                    MuzzleEffect(Angle_in_Quaternion);
                    Clock = 0f;
                }
            }
            else
            {
                Clock = Cooldown;
            }
        }
    }
    protected void PlayAttackAmination(float Attack_Duration)
    {
        SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, IndexPair[PlayerState.ATTACK]);
        if (Attack_Duration != Cooldown)
        {
            SPUM_Prefabs._anim.speed = 2 * Attack_Duration / Cooldown;
        }
        else
        {
            SPUM_Prefabs._anim.speed = 2 / Cooldown;
        }
    }
    protected void SelfRotate(BaseEnemy first_enemy)
    {
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
        }
    }
    protected Quaternion Shoot(BaseEnemy first_enemy)
    {
        // Bắn đạn: lưu ý là truyền góc là hướng bắn của mình luôn chứ không dùng transform.rotation hay quaternion.identity
        Quaternion Angle_in_Quaternion = Quaternion.identity;
        if (first_enemy != null)
        {
            float Angle_in_Radian = Mathf.Atan2(first_enemy.Center.transform.position.y - transform.position.y, first_enemy.Center.transform.position.x - transform.position.x);
            Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
        }
        /*
        GameObject newBullet = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, Angle_in_Quaternion);
        BaseBullets bullet = newBullet.GetComponent<BaseBullets>();
        */
        if (BulletPooler.instance != null)
        {
            BaseBullets bullet = BulletPooler.instance.GetBullet(bullet_Prefab.GetComponent<BaseBullets>().BulletID);
            if (bullet != null)
            {
                bullet.transform.position = Bullet_StartPosition.transform.position;
                bullet.transform.rotation = Angle_in_Quaternion;
                bullet.SetCharacter(this);
                if (first_enemy != null)
                {
                    bullet.SetEnemy(first_enemy);
                }
            }
        }
        return Angle_in_Quaternion; // trả về quaternion để truyền xuống cho muzzle
    }
    protected void MuzzleEffect(Quaternion Angle_in_Quaternion)
    {
        //GameObject muzzle = Instantiate(BulletMuzzle, Bullet_StartPosition.transform.position, Angle_in_Quaternion);
        //Destroy(muzzle, 0.25f);
        if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showMuzzle)
        {
            BaseExplosion muzzle = ExplosionPooler.instance.GetExplosion(BulletMuzzle.GetComponent<BaseExplosion>().ExplosionID);
            if (muzzle != null)
            {
                muzzle.transform.position = Bullet_StartPosition.transform.position;
                muzzle.transform.rotation = Angle_in_Quaternion;
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(muzzle, 0.25f));
            }
        }
    }
    public IEnumerator GetStunned(float duration) // LOGIC CŨ LÀ STOP COROUTINE THÌ LÒI RA LỖI CỦA UNITY, NÊN ĐỔI CHỨ K CÓ SAI NGHEN
    {
        isStunned = true;
        stunEndTime = Time.time + duration;
        GameObject newEffect = Instantiate(StunnedEffect, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        Destroy(newEffect, duration);
        // vòng lặp kiểm tra thời gian stun ngay trong chính hàm này
        while (Time.time < stunEndTime) { yield return null; }
        isStunned = false;
    }
    protected IEnumerator ResetShadow()
    {
        yield return new WaitForSeconds(0.01f);
        if (MaxLevelUnitRoot != null)
        {
            MaxLevelUnitRoot.SetActive(false);
        }
        if (OriginalUnitRoot != null)
        {
            OriginalUnitRoot.SetActive(true);
            Animator maxlevelanimator = OriginalUnitRoot.GetComponent<Animator>();
            if (maxlevelanimator != null)
            {
                SPUM_Prefabs._anim = maxlevelanimator;
            }
            // Animation
            SPUM_Prefabs = GetComponent<SPUM_Prefabs>();
            if (SPUM_Prefabs == null)
            {
                SPUM_Prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
                if (!SPUM_Prefabs.allListsHaveItemsExist())
                {
                    SPUM_Prefabs.PopulateAnimationLists();
                }
            }
            SPUM_Prefabs.OverrideControllerInit();
            foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
            {
                IndexPair[state] = 0;
            }
        }
    }
}
