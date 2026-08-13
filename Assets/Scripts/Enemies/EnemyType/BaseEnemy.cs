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
    protected EnemyModifiers enemyModifiers;
    protected EnemyTeleport enemyTeleport;
    public EnemyEffect enemyEffect;
    protected EnemyHit enemyHit;
    protected float HP;
    protected float Speed;
    protected bool _isHidden;
    public bool isHidden
    {
        get { return _isHidden; }
        set { _isHidden = value; }
    }
    protected bool isArmored;
    public GameObject Center; // đây là chỗ để các character nhắm bắn vào
    protected float incomingDamage = 0; // damage ảo, dùng để check xem nếu mục tiêu sắp chết rồi thì nhắm vào con khác
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
    // Boss
    protected bool isFinalBoss = false;
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
        enemyStats = GetComponent<EnemyStats>();
        enemyModifiers = GetComponent<EnemyModifiers>();
        enemyEffect = GetComponent<EnemyEffect>();
        enemyHit = GetComponent<EnemyHit>();
        enemyTeleport = GetComponent<EnemyTeleport>();
        yield return null;
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
            enemyStats.Original_x_HPScale = 0.1f;
            // Start
            yield return null;
            // Move road
            if (WaypointManager.instance != null)
            {
                Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
            }
            // Reset Stats thiệt nè
            HP = enemyModifiers.ModifiedHP;
            Speed = enemyModifiers.ModifiedSpeed;
            _isHidden = enemyStats.enemyProfile.isHidden;
            isArmored = enemyStats.enemyProfile.isArmored;
            incomingDamage = 0f;
            lastrecordedDamage = Time.time;
            if (!isSummoned) Distance = 0f;
            StatsReseted = true;
            // Reset modifiers
            enemyModifiers.ResetModifiers();
            enemyEffect.ResetEnemyEffect();
        }
        yield return null;
        enemyHit.GetHit(enemyStats, enemyModifiers, this);
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
    public virtual void TakeDamage(BaseCharacter character, float Damage, bool canStrikethrough) // Boss còn phải cập nhật lên text nên để virtual
    {
        // Hidden: nếu không có hidden detection thì KHÔNG NHẮM VÀO
        // Armored: nếu không xuyên giáp được thì KHÔNG TRỪ MÁU
        if ((isArmored && !canStrikethrough))
        {

        }
        else
        {
            float oldHP = HP;
            if (HP - Damage <= 0) { HP = 0; }
            else { HP -= Damage; }
            if (incomingDamage - Damage <= 0) { incomingDamage = 0; }
            else { incomingDamage -= Damage; }
            if (enemyHit != null) enemyHit.GetHit(enemyStats, enemyModifiers, this);
            if (character != null) character.AddTotalDamage(oldHP - HP);
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
                    earnCoinVFX.SetEarnCoinText(enemyModifiers.ModifiedHP);
                }
            }
            if (ExplosionPooler.instance != null && GameSetting.instance != null && GameSetting.instance._showExplosion)
            {
                BaseExplosion explosion = ExplosionPooler.instance.GetExplosion(enemyHit.DieExplosion.GetComponent<BaseExplosion>().ExplosionID);
                if (explosion != null)
                {
                    explosion.transform.position = transform.position;
                    explosion.transform.rotation = transform.rotation * Quaternion.Euler(-90f, 0, 0);
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(explosion, 0.5f));
                }
            }
            if (EconomyManager.instance != null)
            {
                EconomyManager.instance.AddCoin(enemyModifiers.ModifiedHP);
                EconomyManager.instance.Change_CurrentCoin();
            }
            //Destroy(this.gameObject);
            enemyHit.GetHit(enemyStats, enemyModifiers, this);
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
        if (!enemyEffect.isFrozen && !enemyEffect.isStunned)
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
    public void GetHealed(float amount)
    {
        if (enemyStats != null)
        {
            HP += amount;
            incomingDamage -= amount;
            if (HP >= enemyModifiers.ModifiedHP) { HP = enemyModifiers.ModifiedHP; }
        }
    }
    public bool ContainsModifier(float percent)
    {
        return enemyModifiers.ContainsModifier(percent);
    }
    public void ModifySpeed(float percent)
    {
        if (!isFinalBoss)
        {
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
            }
        }
        Speed = enemyModifiers.ModifiedSpeed;
    }
    public void RemoveModifySpeed(float percent)
    {
        if (!isFinalBoss)
        {
            if (enemyModifiers != null)
            {
                if (percent >= 1)
                {
                    enemyModifiers.RemoveSpeedUpModifier(percent);
                }
                else
                {
                    enemyModifiers.RemoveSlowModifier(percent);
                }
            }
        }
        Speed = enemyModifiers.ModifiedSpeed;
    }
    /// <summary>
    /// có thể chọn làm mục tiêu khi: đang active và không teleport
    /// không thể chọn làm mục tiêu khi: đang không active hoặc đang active và đang teleport
    /// </summary>
    /// <returns></returns>
    public bool CanBeTargeted()
    {
        if (!gameObject.activeInHierarchy) return false;
        /*if (enemyTeleport != null && enemyTeleport.isTeleporting)
        {
            return false;
        }*/
        return true;
    }
    /// <summary>
    /// Tìm Waypoint nào vừa nằm trong portal vừa nằm trong danh sách waypoint của enemy
    /// </summary>
    public void TeleportToWaypoint(GameObject[] WaypointLocations)
    {
        foreach (GameObject PortalWaypoint in WaypointLocations)
        {
            for (int i = 0; i < Waypoints.Length; i++)
            {
                if (PortalWaypoint == Waypoints[i])
                {
                    Waypoint_CurrentIndex = i + 1;
                    return;
                }
            }
        }
    }
}
