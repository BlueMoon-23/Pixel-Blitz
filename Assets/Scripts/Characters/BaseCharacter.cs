using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
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
    // Other references
    public GameObject Range_Prefab;
    protected CharacterUIControll characterUI;
    protected Vector3 CircleScale;
    protected RangeScript range;
    public GameObject bullet_Prefab;
    // Attack animation
    protected SPUM_Prefabs SPUM_Prefabs;
    public Dictionary<PlayerState, int> IndexPair = new();
    protected float Bow_Attack_Duration = 0.833f;
    protected float Staff_Attack_Duration = 0.417f;
    public GameObject Bullet_StartPosition;
    private void Awake()
    {
        CircleScale = new Vector3(Range_Prefab.transform.localScale.x, Range_Prefab.transform.localScale.y, Range_Prefab.transform.localScale.z);
        SetRangeCircle();
        characterUI = FindObjectOfType<CharacterUIControll>(true);
        UpgradeCost = new float[] { 0, 0, 0, 0};
        range = Range_Prefab.GetComponent<RangeScript>();
        Clock = Cooldown;
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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
    public virtual float GetCost()
    {
        return Cost;
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
        if (Level < 4)
        {
            characterUI.upgradeCost.text = "Upgrade ($" + UpgradeCost[Level] + ")";
        }
        else
        {
            characterUI.upgradeCost.text = "Max Level";
        }
        characterUI.sellCost.text = "Sell ($" + SellCost + ")";
        characterUI.RangeStats.text = Range.ToString();
        characterUI.DamageStats.text = Damage.ToString();
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
    public virtual void UpgradeToLevel1()
    {
        //
    }
    public virtual void UpgradeToLevel2()
    {
        //
    }
    public virtual void UpgradeToLevel3()
    {
        //
    }
    public virtual void UpgradeToLevel4()
    {
        //
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
    }
    public BaseEnemy FindFirstEnemy()
    {
        float max_distance = 0f;
        for (int i = 0; i < range.enemies_in_range.Count; i++)
        {
            max_distance = Mathf.Max(max_distance, range.enemies_in_range[i].Distance);
        }
        for (int i = 0; i < range.enemies_in_range.Count; i++)
        {
            if (max_distance == range.enemies_in_range[i].Distance)
            {
                return range.enemies_in_range[i];
            }
        }
        return null;
    }
    public BaseEnemy[] FindThreeFirstEnemies()
    {
        // 2 1
        List<float> Enemy_Distances = new List<float>();
        for (int i = 0; i < range.enemies_in_range.Count; i++)
        {
            Enemy_Distances.Add(range.enemies_in_range[i].Distance);
        }
        Enemy_Distances.Sort((a, b) => b.CompareTo(a));
        BaseEnemy[] Enemies_Result = new BaseEnemy[3];
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
                if (range.enemies_in_range[j].Distance == Enemy_Distances[i])
                {
                    Enemies_Result[i] = range.enemies_in_range[j];
                }
            }
        }
        return Enemies_Result;
    }
    public virtual void AttackWithCooldown(float Attack_Duration)
    {
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            StartCoroutine(AttackWithAnimation(Attack_Duration));
            Clock = 0f;
        }
    }
    public virtual IEnumerator AttackWithAnimation(float Attack_Duration)
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
            // Play animation
            SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, IndexPair[PlayerState.ATTACK]);
            SPUM_Prefabs._anim.speed = 2 * Attack_Duration / Cooldown;
            yield return new WaitForSeconds(Attack_Duration / 2 + 0.1f);
            GameObject newBullet = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, transform.rotation);
            BaseBullets bullet = newBullet.GetComponent<BaseBullets>();
            bullet.SetCharacter(this);
            bullet.SetEnemy(first_enemy);
            yield return new WaitForSeconds(Attack_Duration - (Attack_Duration / 2 + 0.1f));
            SPUM_Prefabs._anim.speed = 1;
        }
    }
}
