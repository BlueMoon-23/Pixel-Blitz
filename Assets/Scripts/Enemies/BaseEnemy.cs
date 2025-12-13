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
    // HP Bar;
    [SerializeField] protected GameObject HP_RedBar;
    protected float Original_x_HPScale;
    public float Distance
    {
        get { return _Distance; }
    }
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
    }
    void Start()
    {

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
    public void TakeDamage(float Damage, bool canStrikethrough)
    {
        // Hidden: neu khong co hidden detection thi KHONG NHAM
        // Armored: neu khong xuyen giap duoc thi KHONG TRU MAU
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
    public void Die()
    {
        if (HP <= 0)
        {
            if (EconomyManager.instance != null)
            {
                EconomyManager.instance.AddCoin(this.MaxHP);
                EconomyManager.instance.Change_CurrentCoin();
            }
            Destroy(this.gameObject);
            // Remove enemy from enemy manager
        }
    }
    public void Move()
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
                    transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
                }
                else
                {
                    transform.localScale = new Vector3( Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
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
        _Distance = Speed * Time.deltaTime;
    }
}
