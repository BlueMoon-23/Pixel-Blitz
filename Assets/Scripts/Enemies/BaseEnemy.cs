using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    // Stats gốc nằm ở EnemyStats, bao gồm những thứ sẽ bị reset, còn găm từ prefab xuống thì ở đây là đr
    protected EnemyStats enemyStats;
    protected float HP;
    protected float Speed;
    protected EnemyModifiers enemyModifiers;
    [SerializeField] protected bool isHidden;
    [SerializeField] protected bool isArmored;
    public GameObject Center; // đây là chỗ để các character nhắm bắn vào
    [SerializeField] protected float incomingDamage = 0; // damage ảo, dùng để check xem nếu mục tiêu sắp chết rồi thì nhắm vào con khác
    protected float lastrecordedDamage = 0f;
    // Move
    public GameObject[] Waypoints;
    public int Waypoint_SelectedIndex; // Thằng gamemode sẽ truyền cái này cho enemy để nó biết nó ở waypoint nào
    public int Waypoint_CurrentIndex; // thằng này sẽ chỉ enemy đi đâu
    // Move animation
    protected SPUM_Prefabs SPUM_Prefabs;
    public Dictionary<PlayerState, int> IndexPair = new();
    // Distance
    protected float _Distance;
    public float Distance
    {
        get { return _Distance; }
        set { if (value >= 0) _Distance = value; }
    }
    public bool isSummoned = false; // phòng trường hợp summon lấy từ pooling, pooling get xong distance bị reset = 0 thì k đáng
    // HP Bar;
    public GameObject HP_RedBar;
    // Rotate
    public GameObject EnemyRoot;
    // FreezeEffect
    public GameObject FreezeEffect; // MagicChargeBlue
    protected int FreezeStack = 3;
    protected int FreezeCurrentStack = 0;
    protected bool isFrozen = false;
    // Boss
    protected bool isFinalBoss = false;
    // Stun effect
    public GameObject StunEffect;
    protected bool isStunned = false;
    // Kiểm tra đã reset hay chưa
    public bool StatsReseted = false; // PHẢI MẶC ĐỊNH LÀ FALSE, TRUE LÀ UPDATE SẼ CHẠY TRƯỚC LÀ SẼ BÁO NULL
    protected void OnEnable()
    {
        StartCoroutine(ResetStats());
    }
    protected virtual IEnumerator ResetStats()
    {
        // Awake
        StatsReseted = false; // khóa update/move cho đến khi xong stats
        yield return null;
        enemyStats = GetComponent<EnemyStats>();
        enemyModifiers = GetComponent<EnemyModifiers>();
        if (enemyStats != null)
        {
            if (!isSummoned) Waypoint_CurrentIndex = 1; // xem lại tình huống ở necromancer và mystery enemies
            // Move animation
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
            // HP Bar
            enemyStats.Original_x_HPScale = 3.5f;
            // Start
            yield return null;
            // Move road
            if (WaypointManager.instance != null)
            {
                Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
            }
            // Reset Stats thiệt nè
            HP = enemyStats.MaxHP;
            Speed = enemyStats.OldSpeed;
            isHidden = enemyStats.isHidden;
            isArmored = enemyStats.isArmored;
            incomingDamage = 0f;
            lastrecordedDamage = Time.time;
            FreezeCurrentStack = 0;
            if (!isSummoned) Distance = 0f;
            isFrozen = false;
            isStunned = false;
            StatsReseted = true;
            // Reset lại effect nữa
            StunEffect.SetActive(false);
            FreezeEffect.SetActive(false);
            // Reset modifiers
            if (enemyModifiers != null)
            {
                enemyModifiers.ResetModifiers();
            }
        }
        yield return null;
        if (enemyStats != null) // sau thêm modifiers thì thêm ở đây
        {
            HP_RedBar.transform.localScale = new Vector3(enemyStats.Original_x_HPScale * HP / enemyStats.MaxHP, HP_RedBar.transform.localScale.y, HP_RedBar.transform.localScale.z);
        }
    }
    // Update is called once per frame
    protected void Update()
    {
        if (StatsReseted)
        {
            Move();
            Die();
            ResetIncomingDamage();
        }
    }
    public bool isHiddenOrNot()
    {
        return isHidden;
    }
    public float GetHP() { return HP; }
    public void TakeIncomingDamage(float Damage, bool canStrikethrough)
    {
        if ((isArmored && !canStrikethrough))
        {
            //
        }
        else
        {
            incomingDamage += Damage;
            lastrecordedDamage = Time.time;
        }
    }
    protected void ResetIncomingDamage()
    {
        if (incomingDamage > 0 && Time.time - lastrecordedDamage > 0.25f)
        {
            incomingDamage = 0;
        }
    }
    public virtual void TakeDamage(float Damage, bool canStrikethrough) // Boss còn phải cập nhật lên text nên để virtual
    {
        // Hidden: nếu không có hidden detection thì KHÔNG NHẮM VÀO
        // Armored: nếu không xuyên giáp được thì KHÔNG TRỪ MÁU
        if ((isArmored && !canStrikethrough))
        {
            //
        }
        else
        {
            if (HP - Damage <= 0) { HP = 0; }
            else { HP -= Damage; }
            if (incomingDamage - Damage <= 0) { incomingDamage = 0; }
            else { incomingDamage -= Damage; }
            if (enemyStats != null)
            {
                HP_RedBar.transform.localScale = new Vector3(enemyStats.Original_x_HPScale * HP / enemyStats.MaxHP, HP_RedBar.transform.localScale.y, HP_RedBar.transform.localScale.z);
            }
        }
    }
    protected virtual void Die()
    {
        if (HP <= 0)
        {
            if (VFXPooler.instance != null)
            {
                BaseVFX EarnCoin = VFXPooler.instance.GetVFX(0); // 0 la hieu ung Earn coin
                EarnCoin.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
                EarnCoin.transform.rotation = Quaternion.identity;
                EarnCoinVFX earnCoinVFX = EarnCoin.GetComponent<EarnCoinVFX>();
                if (earnCoinVFX != null)
                {
                    earnCoinVFX.SetEarnCoinText(this.enemyStats.MaxHP);
                }
            }
            if (EconomyManager.instance != null)
            {
                EconomyManager.instance.AddCoin(this.enemyStats.MaxHP);
                EconomyManager.instance.Change_CurrentCoin();
            }
            //Destroy(this.gameObject);
            if (enemyStats != null)
            {
                HP_RedBar.transform.localScale = new Vector3(enemyStats.Original_x_HPScale, HP_RedBar.transform.localScale.y, HP_RedBar.transform.localScale.z);
            }
            if (EnemyManager.instance != null)
            {
                EnemyManager.instance.ReturnEnemy(this);
            }
            //Reset lại hp redbar, do x của nó = 0 nên khi dùng lại thì bị gán ngu
        }
    }
    public bool isDieOrNot()
    {
        return (HP - incomingDamage <= 0f);
    }
    protected void Move()
    {
        // bỏ cái này lên chỗ reset stats vẫn còn bị lỗi thanh máu mất trắng, nên bỏ vào đây cho chắc
        //HP_RedBar.transform.localScale = new Vector3(enemyStats.Original_x_HPScale * HP / enemyStats.MaxHP, HP_RedBar.transform.localScale.y, HP_RedBar.transform.localScale.z);
        if (!isFrozen && !isStunned)
        {
            if (SPUM_Prefabs != null)
            {
                SPUM_Prefabs.PlayAnimation(PlayerState.MOVE, 0);
                SPUM_Prefabs._anim.speed = 25 * Speed / 38 + 7 / 38;
            }
            if (Waypoint_CurrentIndex != Waypoints.Length)
            {
                if (Vector3.Distance(transform.position, Waypoints[Waypoint_CurrentIndex].transform.position) >= 0.05f)
                {
                    // Không được so sánh tuyệt đối bởi vì time.deltatime gây ra 1 độ lệch (1/fps)
                    Vector3 Direction = (Waypoints[Waypoint_CurrentIndex].transform.position - transform.position).normalized;
                    // Di chuyển bình thường: bị lỗi khi fps quá thấp, các con quái có tốc độ cao sẽ đi qua, thỏa mãn khoảng cách đạt được và bị kẹt quanh
                    // Giải pháp: dùng hàm MoveTowards (không bao giờ đi quá đích). bản chất là nếu quá lố đích thì sẽ bị teleport về đích
                    float step = Speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, Waypoints[Waypoint_CurrentIndex].transform.position, step);
                    if (Direction.x >= 0)
                    {
                        EnemyRoot.transform.localScale = new Vector3(-1f * Mathf.Abs(EnemyRoot.transform.localScale.x), Mathf.Abs(EnemyRoot.transform.localScale.y), Mathf.Abs(EnemyRoot.transform.localScale.z));
                    }
                    else
                    {
                        EnemyRoot.transform.localScale = new Vector3(Mathf.Abs(EnemyRoot.transform.localScale.x), Mathf.Abs(EnemyRoot.transform.localScale.y), Mathf.Abs(EnemyRoot.transform.localScale.z));
                    }
                }
                else
                {
                    Waypoint_CurrentIndex++;
                }
            }
            else // == nghia la da cham nha chinh
            {
                BaseHealth.instance.BaseGetHit(HP);
                if (EnemyManager.instance != null)
                {
                    EnemyManager.instance.ReturnEnemy(this);
                }
                // Remove enemy from enemy manager
            }
            // Distance
            _Distance += Speed * Time.deltaTime;
        }
        else
        {
            if (SPUM_Prefabs != null)
            {
                SPUM_Prefabs.PlayAnimation(PlayerState.IDLE, 0);
                SPUM_Prefabs._anim.speed = 0f;
            }
        }
    }
    public void GetFreeze(float FreezeTime, int FreezeCount)
    {
        if (!isFinalBoss)
        {
            FreezeCurrentStack++;
            FreezeStack = FreezeCount;
            if (FreezeCurrentStack == FreezeStack)
            {
                StartCoroutine(BeFrozen(FreezeTime));
            }
        }
    }
    public IEnumerator GetStunned(float StunDuration)
    {
        if (!isFinalBoss)
        {
            isStunned = true;
            StunEffect.SetActive(true);
            yield return new WaitForSeconds(StunDuration);
            isStunned = false;
            StunEffect.SetActive(false);
        }
    }
    private IEnumerator BeFrozen(float FreezeTime)
    {
        isFrozen = true;
        FreezeEffect.SetActive(true);
        yield return new WaitForSeconds(FreezeTime);
        isFrozen = false;
        FreezeCurrentStack = 0;
        FreezeEffect.SetActive(false);
        yield break;
    }
    public void GetHealed(float amount)
    {
        if (enemyStats != null)
        {
            HP += amount;
            incomingDamage -= amount;
            if (HP >= enemyStats.MaxHP) { HP = enemyStats.MaxHP; }
        }
    }
    public void ModifySpeed(float percent)
    {
        if (!isFinalBoss)
        {
            // mong muốn: speed = oldspeed * min của mảng slowmodifier * mã cua mảng boostmodifier
            float slow_factor = 1f;
            float boost_factor = 1f;
            if (enemyModifiers != null)
            {
                if (percent >= 1)
                {
                    enemyModifiers.AddSpeedUpModifier(percent);
                }
                else
                {
                    enemyModifiers.AddSlowModifier(percent);
                }
                slow_factor = enemyModifiers.GetMinSlowPercent();
                boost_factor = enemyModifiers.GetMaxBoostPercent();
            }
            Speed = enemyStats.OldSpeed * slow_factor * boost_factor;
        }
        else
        {
            Speed = enemyStats.OldSpeed;
        }
    }
}
