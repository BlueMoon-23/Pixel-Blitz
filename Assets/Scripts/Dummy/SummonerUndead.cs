using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SummonerUndead : MonoBehaviour, IStunnable, ISide
{
    private BaseCharacter summoner;
    [SerializeField] protected float HP;
    [SerializeField] protected float MaxHP;
    [SerializeField] protected float Speed;
    public int ID;
    private bool isProtected;
    // Move
    [SerializeField] private GameObject[] Waypoints;
    [SerializeField] private int Waypoint_CurrentIndex; // thằng này sẽ chỉ enemy đi đâu
    [SerializeField] private List<WaypointInformation> ValidWaypoint; // summoner undead phải tự biết được mình sẽ đi waypoint nào => mới đúng cho random path và multi path
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
        isProtected = false;
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
        ValidWaypoint.Clear(); // clear kết quả trước đó
        if (WaypointManager.instance != null)
        {
            foreach (WaypointInformation Path in WaypointManager.instance.List_of_Waypoints)
            {
                for (int i = 0; i < Path.Waypoints.Length - 1; i++)
                {
                    if (x_Between_2_Waypoints(i, transform.position.x, Path.Waypoints) && y_Between_2_Waypoints(i, transform.position.y, Path.Waypoints))
                    {
                        ValidWaypoint.Add(Path);
                        break;
                    }
                }
            }
            int selected_index = UnityEngine.Random.Range(0, ValidWaypoint.Count);
            Waypoints = ValidWaypoint[selected_index].Waypoints;
        }
        // Cơ chế xác định mình đang ở giữa 2 waypoint index nào
        for (int i = 0; i < Waypoints.Length - 1; i++)
        {
            if (x_Between_2_Waypoints(i, transform.position.x, Waypoints) && y_Between_2_Waypoints(i, transform.position.y, Waypoints))
            {
                Waypoint_CurrentIndex = i;
                break;
            }
        }
        StatsReseted = true;
    }
    public void ReduceWaypoint()
    {
        Waypoint_CurrentIndex--;
    }
    public void SetCharacter(BaseCharacter character)
    {
        summoner = character;
    }
    private bool x_Between_2_Waypoints(int index, float x_position, GameObject[] Waypoints)
    {
        return ((Waypoints[index].transform.position.x <= x_position && x_position <= Waypoints[index + 1].transform.position.x) || (Waypoints[index].transform.position.x >= x_position && x_position >= Waypoints[index + 1].transform.position.x));
    }
    private bool y_Between_2_Waypoints(int index, float y_position, GameObject[] Waypoints)
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
            HP_RedBar.transform.localScale = new Vector3(Original_x_HPScale * HP / MaxHP, HP_RedBar.transform.localScale.y, HP_RedBar.transform.localScale.z);
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
            baseEnemy.TakeDamage(summoner, damage, true); // mình gây 50 damage lên enemy
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
    public IEnumerator GetStunned(float duration)
    {
        yield return null;
        if (SummonerUndeadPooler.instance != null)
        {
            SummonerUndeadPooler.instance.ReturnUndead(this);
        }
    }
    public void ApplyStun(float StunDuration)
    {
        StartCoroutine(GetStunned(StunDuration));
    }
    public void SetStunImmunity()
    {
        isProtected = true;
    }
    public void RemoveStunImmunity()
    {
        isProtected = false;
    }
    public bool IsStunImmunity()
    {
        return isProtected;
    }
    public SIDE GetSide()
    {
        return SIDE.Dummy;
    }
}
