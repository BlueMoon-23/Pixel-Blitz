using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class SummonerUndead : MonoBehaviour
{
    [SerializeField] protected float HP;
    [SerializeField] protected float MaxHP;
    [SerializeField] protected float Speed;
    public int ID;
    // Move
    [SerializeField] private GameObject[] Waypoints;
    [SerializeField] private int Waypoint_CurrentIndex; // thằng này sẽ chỉ enemy đi đâu
    // Move animation
    protected SPUM_Prefabs SPUM_Prefabs;
    public Dictionary<PlayerState, int> IndexPair = new();
    // HP Bar;
    public GameObject HP_RedBar;
    protected float Original_x_HPScale;
    // Rotate
    public GameObject EnemyRoot;
    // ResetStats
    public bool StatsReseted = false; // mặc định phải là false, true là update sẽ chạy trước
    protected void OnEnable()
    {
        StartCoroutine(ResetStats());
    }
    private IEnumerator ResetStats()
    {
        StatsReseted = false;
        yield return null;
        // Awake
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
        yield return null;
        // Start
        if (WaypointManager.instance != null)
        {
            Waypoints = WaypointManager.instance.GetWaypoints(out int Waypoints_index);
            Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoints_index);
        }
        // Cơ chế xác định mình đang ở giữa 2 waypoint index nào
        for (int i = 0; i < Waypoints.Length - 1; i++)
        {
            if (x_Between_2_Waypoints(i, transform.position.x) && y_Between_2_Waypoints(i, transform.position.y))
            {
                Waypoint_CurrentIndex = i;
                break;
            }
        }
        StatsReseted = true;
    }
    private bool x_Between_2_Waypoints(int index, float x_position)
    {
        return ((Waypoints[index].transform.position.x <= x_position && x_position <= Waypoints[index + 1].transform.position.x) || (Waypoints[index].transform.position.x >= x_position && x_position >= Waypoints[index + 1].transform.position.x));
    }
    private bool y_Between_2_Waypoints(int index, float y_position)
    {
        return ((Waypoints[index].transform.position.y <= y_position && y_position <= Waypoints[index + 1].transform.position.y) || (Waypoints[index].transform.position.y >= y_position && y_position >= Waypoints[index + 1].transform.position.y));
    }
    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            Move();
            Die();
        }
    }
    private void Die()
    {
        if (HP <= 0)
        {
            if (SummonerUndeadPooler.instance != null)
            {
                SummonerUndeadPooler.instance.ReturnUndead(this);
            }
            //Reset lại hp
            HP = MaxHP;
        }
    }
    private void Move()
    {
        SPUM_Prefabs.PlayAnimation(PlayerState.MOVE, IndexPair[PlayerState.MOVE]);
        SPUM_Prefabs._anim.speed = 25 * Speed / 38 + 7 / 38;
        if (Waypoint_CurrentIndex >= 0)
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
                Waypoint_CurrentIndex--;
            }
        }
        else
        {
            if (SummonerUndeadPooler.instance != null)
            {
                SummonerUndeadPooler.instance.ReturnUndead(this);
            }
            //Reset lại hp
            HP = MaxHP;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cơ chế tông: mình 800 máu, enemy 50 máu => mình sẽ còn lại 750 máu, gây 800 damage lên enemy
        // mình 50 máu, enemy 800 máu => mình hết máu, chết, còn enemy còn lại 750 máu
        if (this.HP <= 0) return; // Nếu đã chết thì không tông thêm ai nữa
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            float damage = this.HP; // 50
            float enemyHP = baseEnemy.GetHP(); // 800
            baseEnemy.TakeDamage(damage, true); // mình gây 50 damage lên enemy
            if (enemyHP > 0) // tránh tình huống 1 con tông vào con kia, con kia âm máu, chưa kịp chết thì con thứ 2 đã tông vào nó, lấy máu âm cộng lại
            {
                this.HP -= enemyHP; // mình mất 800 máu, thành -750 máu
            }
            if (this.HP <= 0)
            {
                Die();
            }
            HP_RedBar.transform.localScale = new Vector3(Original_x_HPScale * HP / MaxHP, HP_RedBar.transform.localScale.y, HP_RedBar.transform.localScale.z);
        }
    }
}
