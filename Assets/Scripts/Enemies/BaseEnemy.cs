using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float HP;
    [SerializeField] protected float MaxHP;
    [SerializeField] protected float Speed;
    [SerializeField] protected bool isHidden;
    [SerializeField] protected bool isArmored;
    // Move
    public GameObject[] Waypoints;
    public int Waypoint_CurrentIndex;
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
    // HP Bar;
    [SerializeField] protected GameObject HP_RedBar;
    protected float Original_x_HPScale;
    // Rotate
    public GameObject EnemyRoot;
    // FreezeEffect
    public GameObject FreezeEffect; // MagicChargeBlue
    protected int FreezeStack = 3;
    [SerializeField] protected int FreezeCurrentStack = 0;
    protected bool isFrozen = false;
    // Boss
    protected bool isFinalBoss = false;
    // Stun effect
    public GameObject StunEffect;
    protected bool isStunned = false;
    protected void Awake()
    {
        // Move road
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        Waypoints = Waypoints.OrderBy(go  => go.name).ToArray();
        Waypoint_CurrentIndex = 1;
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
        Original_x_HPScale = HP_RedBar.transform.localScale.x;
        // Distance
        _Distance = 0f;
        // EnemyManager
        if (EnemyManager.instance != null) { EnemyManager.instance.AddEnemy(this); }
    }
    void Start()
    {
        /*// Bỏ vào awake thì bị lỗi nếu instantiate từ necromancer và boss mystery
        if (Waypoint_CurrentIndex == 0) // Đảm bảo việc gán từ bên ngoài
        {
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Die();
    }
    public bool isHiddenOrNot()
    {
        return isHidden;
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
            HP -= Damage;
            HP_RedBar.transform.localScale = new Vector3(Original_x_HPScale * HP / MaxHP, HP_RedBar.transform.localScale.y, HP_RedBar.transform.localScale.z);
        }
    }
    public virtual void Die()
    {
        if (HP <= 0)
        {
            if (EconomyManager.instance != null)
            {
                EconomyManager.instance.AddCoin(this.MaxHP);
                EconomyManager.instance.Change_CurrentCoin();
            }
            Destroy(this.gameObject);
        }
    }
    public bool isDieOrNot()
    {
        return (HP <= 0);
    }
    public void Move()
    {
        if (!isFrozen && !isStunned)
        {
            SPUM_Prefabs.PlayAnimation(PlayerState.MOVE, IndexPair[PlayerState.MOVE]);
            SPUM_Prefabs._anim.speed = 25 * Speed / 38 + 7 / 38;
            if (Waypoint_CurrentIndex != Waypoints.Length)
            {
                if (Vector3.Distance(transform.position, Waypoints[Waypoint_CurrentIndex].transform.position) >= 0.05f)
                {
                    // Không được so sánh tuyệt đối bởi vì time.deltatime gây ra 1 độ lệch (1/fps)
                    Vector3 Direction = (Waypoints[Waypoint_CurrentIndex].transform.position - transform.position).normalized;
                    transform.position += Direction * Speed * Time.deltaTime;
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
                GameManager.instance.BaseGetHit(HP);
                Destroy(this.gameObject);
                // Remove enemy from enemy manager
            }
            // Distance
            _Distance += Speed * Time.deltaTime;
        }
        else
        {
            SPUM_Prefabs.PlayAnimation(PlayerState.IDLE, IndexPair[PlayerState.IDLE]);
            SPUM_Prefabs._anim.speed = 0f;
        }
    }
    private void OnDestroy()
    {
        if (EnemyManager.instance != null) { EnemyManager.instance.RemoveEnemy(this); }
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
        GameObject effect = Instantiate(FreezeEffect, transform.position, Quaternion.identity);
        Destroy(effect, FreezeTime);
        yield return new WaitForSeconds(FreezeTime);
        isFrozen = false;
        FreezeCurrentStack = 0;
        yield break;
    }
}
