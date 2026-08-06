using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class BaseCharacter : MonoBehaviour
{
    // Basic stats
    [Header("Original Stats. Các references đến các character component khác không cần gắn")]
    public CharacterProfile profile;
    public CharacterWeapon characterWeapon;
    public CharacterAttack characterAttack;
    public CharacterEffect characterEffect;
    protected CharacterUIControll characterUI;
    protected float Range = 1; // Range = 1 <=> tầm bắn là hình tròn nằm trong 1 ô tilemap
    protected float Damage;
    protected float TotalDamage;
    protected float Cooldown;
    protected float Clock;
    protected float Cost;
    protected string SpecialDescription;
    protected bool isCliff;
    protected bool hasHiddenDetection;
    protected bool canStrikethrough;
    protected bool _hasAbility = false;
    public bool hasAbility
    {
        get { return _hasAbility; }
    }
    [SerializeField] protected bool _isProtected;
    public bool isProtected
    {
        get => _isProtected;
        set => _isProtected = value;
    }
    protected int Level;
    // Max level Change
    [Header("Unit Root")]
    public GameObject OriginalUnitRoot;
    protected SpriteRenderer[] rendereers;
    public Material OriginalMaterial;
    public Material MaxLevelMaterial;
    // Attack animation
    [Header("Bullet")]
    public GameObject bullet_Prefab;
    public GameObject Bullet_StartPosition;
    public GameObject BulletMuzzle;
    [Header("Weapon")]
    [SerializeField] protected Sprite WeaponSprite;
    protected SPUM_Prefabs SPUM_Prefabs;
    public Dictionary<PlayerState, int> IndexPair = new();
    
    // Kiểm tra đã reset hay chưa
    protected bool StatsReseted = false; // PHẢI MẶC ĐỊNH LÀ FALSE, TRUE LÀ UPDATE SẼ CHẠY TRƯỚC LÀ SẼ BÁO NULL
    // Báo cáo nhiệm vụ
    public Action<int> OnLevelUp;
    protected void Awake()
    {
        rendereers = OriginalUnitRoot.GetComponentsInChildren<SpriteRenderer>();
        characterAttack = GetComponent<CharacterAttack>();
        characterEffect = GetComponent<CharacterEffect>();
        characterWeapon = GetComponent<CharacterWeapon>();
        if (characterWeapon != null)
        {
            characterWeapon.AssignWeapon(profile);
        }
    }
    protected virtual void OnEnable()
    {
        // Chỉnh lại một số stats
        StatsReseted = false; // khóa update/move cho đến khi xong stats
        characterEffect.ResetCharacterEffect();
        //
        Range = WeaponCalculator.CalculateRange(profile.characterLevelDatas[0].RangeStat, characterWeapon.WeaponEquipped);
        Damage = WeaponCalculator.CalculateDamage(profile.characterLevelDatas[0].DamageStat, characterWeapon.WeaponEquipped);
        Cooldown = WeaponCalculator.CalculateCooldown(profile.characterLevelDatas[0].CooldownStat, characterWeapon.WeaponEquipped);
        Cost = WeaponCalculator.CalculateCost(profile.CostStat, characterWeapon.WeaponEquipped);
        hasHiddenDetection = profile.characterLevelDatas[0].hasHiddenDetection;
        canStrikethrough = profile.characterLevelDatas[0].canStrikethrough;
        _hasAbility = profile.characterLevelDatas[0].hasAbility;
        SpecialDescription = profile.characterLevelDatas[0].Special;
        Level = 0;
        TotalDamage = 0;
        _isProtected = false;
        StartCoroutine(ResetStats());
    }
    protected IEnumerator ResetStats()
    {
        yield return null;
        characterAttack.ResetCharacterAttack();
        characterUI = FindObjectOfType<CharacterUIControll>(true);
        Range = GetRange();
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
        StatsReseted = true;
        // Reset hiệu ứng cầu vòng cho level max

        foreach (SpriteRenderer sr in rendereers)
        {
            if (OriginalMaterial != null) sr.material = OriginalMaterial;
        }
    }
    // Normal methods
    public virtual void SetAbilityIcon()
    {
        // cài riêng
    }
    public virtual void Ability(Vector3 position)
    {
        // cài riêng
    }
    public float GetCost()
    {
        if (characterWeapon == null && Cost != profile.CostStat) 
        {
            return profile.CostStat;
        }
        else if (Cost != WeaponCalculator.CalculateCost(profile.CostStat, characterWeapon.WeaponEquipped)) 
        { 
            return WeaponCalculator.CalculateCost(profile.CostStat, characterWeapon.WeaponEquipped); 
        }
        else return Cost;
    }
    public float GetRange()
    {
        if (characterWeapon == null && Range <= profile.characterLevelDatas[0].RangeStat)
        {
            return profile.characterLevelDatas[0].RangeStat;
        }
        else if (Range <= WeaponCalculator.CalculateRange(profile.characterLevelDatas[0].RangeStat, characterWeapon.WeaponEquipped)) 
        { 
            return WeaponCalculator.CalculateRange(profile.characterLevelDatas[0].RangeStat, characterWeapon.WeaponEquipped); 
        } // <= la chua duoc khoi tao
        else return Range;
    }
    public bool hasHiddenDetectionOrNot()
    {
        return hasHiddenDetection;
    }
    public bool canStrikethroughOrNot()
    {
        return canStrikethrough;
    }
    public virtual float GetDamage()
    {
        return Damage;
    }
    public void AddTotalDamage(float damage)
    {
        TotalDamage += damage;
        if (characterUI != null)
        {
            if (characterUI.CurrentCharacter == this) characterUI.characterTotalDamage.text = "Total damage: " + TotalDamage;
        }
    }
    public float GetCooldown()
    {
        return Cooldown;
    }
    public int GetLevel()
    {
        return Level;
    }
    public float GetUpgradeCost()
    {
        return WeaponCalculator.CalculateCost(profile.characterLevelDatas[Level + 1].UpgradeCost, characterWeapon.WeaponEquipped);
    }
    public float GetSellCost()
    {
        float SellCost = (int)(Cost / 3);
        for (int i = 0; i <= Level; i++)
        {
            SellCost += (int)(WeaponCalculator.CalculateCost(profile.characterLevelDatas[i].UpgradeCost, characterWeapon.WeaponEquipped) / 3);
        }
        return SellCost;
    }
    protected void Update()
    {
        if (StatsReseted)
        {
            if (profile.AttackDuration != 0)
            {
                float min_duration = profile.AttackDuration < Cooldown ? profile.AttackDuration : Cooldown;
                if (!characterEffect.isStunned) { AttackWithCooldown(min_duration); }
            }
            else
            {
                if (!characterEffect.isStunned) AttackWithoutAnimation();
            }
        }
    }
    // Template để hỗ trợ hàm dưới
    protected void SetStatInfo<T>(int index, string label, T currentVal, T nextVal, bool isHigherBetter)
    {
        // So sánh giá trị hiện tại và giá trị kế tiếp
        bool isChanged = !EqualityComparer<T>.Default.Equals(currentVal, nextVal);
        // Lấy UI từ Hash Map (Tự động sinh nếu chưa có)
        var textUI = CharacterUIControll.instance.GetOrCreateInfo(index);
        textUI.gameObject.SetActive(isChanged);
        if (isChanged)
        {
            // Chuyển Color32 thành chuỗi Hex (VD: "A5FF6BFF") để dùng trong thẻ Rich Text
            string greenHex = $"#{ColorUtility.ToHtmlStringRGBA(new Color32(165, 255, 107, 255))}";
            string redHex = $"#{ColorUtility.ToHtmlStringRGBA(new Color32(255, 100, 76, 255))}";
            // Mặc định dùng màu xanh
            string colorHex = greenHex;
            // So sánh giá trị nếu loại T hỗ trợ IComparable (int, float, double, v.v.)
            int comparison = Comparer<T>.Default.Compare(currentVal, nextVal);
            if (comparison < 0) // currentVal < nextVal (Tăng)
            {
                colorHex = isHigherBetter ? greenHex : redHex;
            }
            else if (comparison > 0) // currentVal > nextVal (Giảm)
            {
                colorHex = isHigherBetter ? redHex : greenHex;
            }
            // Bọc duy nhất nextVal vào thẻ <color>
            textUI.text = $"{label}: {currentVal} => <color={colorHex}>{nextVal}</color>";
        }
    }
    protected void SetStatInfo(int index, string label, bool currentEffect, bool nextEffect)
    {
        var textUI = CharacterUIControll.instance.GetOrCreateInfo(index);
        if (!currentEffect && nextEffect)
        {
            textUI.gameObject.SetActive(true);
            textUI.text = label;
        }
        else
        {
            textUI.gameObject.SetActive(false);
        }
    }
    public virtual void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            characterUI.characterName.text = profile.CharacterName;
            characterUI.characterImage.sprite = profile.CharacterImage;
            characterUI.characterName.color = profile.CharacterColor;
            characterUI.characterGlow.color = profile.CharacterColor;
            characterUI.characterGlow.color = new Color32(profile.CharacterColor.r, profile.CharacterColor.g, profile.CharacterColor.b, 130);
            characterUI.characterTotalDamage.text = "Total damage: " + TotalDamage;
            if (Level < profile.characterLevelDatas.Count - 1)
            {
                // Chỉ hiện thông tin thăng cấp nếu có sự sai khác
                characterUI.upgradeName.text = profile.characterLevelDatas[Level + 1].UpgradeName;
                characterUI.upgradeName.color = profile.CharacterColor;
                SetStatInfo(0, "Range", Range, WeaponCalculator.CalculateRange(profile.characterLevelDatas[Level + 1].RangeStat, characterWeapon.WeaponEquipped), true);
                SetStatInfo(1, "Damage", Damage, WeaponCalculator.CalculateDamage(profile.characterLevelDatas[Level + 1].DamageStat, characterWeapon.WeaponEquipped), true);
                SetStatInfo(2, "Cooldown", Cooldown, WeaponCalculator.CalculateCooldown(profile.characterLevelDatas[Level + 1].CooldownStat, characterWeapon.WeaponEquipped), false);
                SetStatInfo(3, "+ Hidden Detection", hasHiddenDetection, profile.characterLevelDatas[Level + 1].hasHiddenDetection);
                SetStatInfo(4, "+ Strikethrough", canStrikethrough, profile.characterLevelDatas[Level + 1].canStrikethrough);
                var textUI = CharacterUIControll.instance.GetOrCreateInfo(5);
                if (SpecialDescription != profile.characterLevelDatas[Level + 1].Special)
                {
                    textUI.gameObject.SetActive(true);
                    textUI.text = profile.characterLevelDatas[Level + 1].Special;
                }
                else
                {
                    textUI.gameObject.SetActive(false);
                }
                CharacterUIControll.instance.TurnOffExternalInfo();
                characterUI.upgradeCost.text = "Upgrade (-$" + WeaponCalculator.CalculateCost(profile.characterLevelDatas[Level + 1].UpgradeCost, characterWeapon.WeaponEquipped) + ")";
            }
            else
            {
                characterUI.upgradeName.text = "";
                CharacterUIControll.instance.TurnOffAllInfo();
                characterUI.upgradeCost.text = "Max Level";
            }
            characterUI.sellCost.text = "Sell (+$" + GetSellCost() + ")";
            characterUI.RangeStats.text = Range.ToString();
            characterUI.DamageStats.text = Damage.ToString(); // damage bị lỗi từ bên wizard, nên sửa thêm
            characterUI.CooldownStats.text = Cooldown.ToString();
            characterUI.HiddenDetectionIcon.alpha = (hasHiddenDetection) ? 1f : 0f;
            characterUI.StrikethroughIcon.alpha = (canStrikethrough) ? 1f : 0f;
        }
    }
    public virtual void Upgrade()
    {
        Level++;
        Range = WeaponCalculator.CalculateRange(profile.characterLevelDatas[Level].RangeStat, characterWeapon.WeaponEquipped);
        Damage = WeaponCalculator.CalculateDamage(profile.characterLevelDatas[Level].DamageStat, characterWeapon.WeaponEquipped);
        Cooldown = WeaponCalculator.CalculateCooldown(profile.characterLevelDatas[Level].CooldownStat, characterWeapon.WeaponEquipped);
        SpecialDescription = profile.characterLevelDatas[Level].Special;
        hasHiddenDetection = profile.characterLevelDatas[Level].hasHiddenDetection;
        canStrikethrough = profile.characterLevelDatas[Level].canStrikethrough;
        _hasAbility = profile.characterLevelDatas[Level].hasAbility;
        SetUpgradeInformation();
        Range = GetRange();
        characterAttack.Range_Prefab.transform.localScale = characterAttack.GetCircleScale() * Range;
        // Thêm hiệu ứng cầu vòng cho level max
        if (Level == profile.characterLevelDatas.Count - 1)
        {
            foreach (SpriteRenderer sr in rendereers)
            {
                if (MaxLevelMaterial != null) sr.material = MaxLevelMaterial;
            }
        }
        OnLevelUp?.Invoke(Level);
    }

    public void AttackWithCooldown(float Attack_Duration)
    {
        // Logic cũ
        if (characterEffect.isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (characterAttack.GetEnemyCountInRange() != 0)
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
        BaseEnemy first_enemy = characterAttack.FindFirstEnemy();
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
        if (characterEffect.isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (characterAttack.GetEnemyCountInRange() != 0)
            {
                BaseEnemy first_enemy = characterAttack.FindFirstEnemy();
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
}
