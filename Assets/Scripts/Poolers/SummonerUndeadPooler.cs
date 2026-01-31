using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerUndeadPooler : MonoBehaviour
{
    public static SummonerUndeadPooler instance;
    public List<SummonerUndead> undead_Prefabs;
    public Transform poolParent;
    private Dictionary<int, Stack<SummonerUndead>> pools;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        poolParent = this.transform;
        pools = new Dictionary<int, Stack<SummonerUndead>>();
        for (int id = 0; id < undead_Prefabs.Count; id++)
        {
            CreateUndead(id);
        }
    }
    public SummonerUndead CreateUndead(int id)
    {
        SummonerUndead undead = Instantiate(undead_Prefabs[id], poolParent);
        undead.ID = id;
        undead.gameObject.SetActive(false);
        if (pools.TryGetValue(id, out Stack<SummonerUndead> pool))
        {
            pool.Push(undead);
        }
        else
        {
            pool = new Stack<SummonerUndead>();
            pool.Push(undead);
            pools[id] = pool;
        }
        return undead;
    }
    public SummonerUndead GetUndead(int id)
    {
        Stack<SummonerUndead> pool = null;
        if (pools.ContainsKey(id))
        {
            pool = pools[id];
        }
        else
        {
            CreateUndead(id);
            pool = pools[id];
        }
        if (pool.Count > 0)
        {
            SummonerUndead undead = pool.Pop();
            undead.gameObject.SetActive(true);
            return undead;
        }
        else
        {
            SummonerUndead undead = CreateUndead(id);
            undead.gameObject.SetActive(true);
            pool.Pop();
            return undead;
        }
    }
    public void ReturnUndead(SummonerUndead undead)
    {
        undead.transform.SetParent(poolParent);
        undead.gameObject.SetActive(false);
        if (pools.TryGetValue(undead.ID, out Stack<SummonerUndead> pool))
        {
            if (!pool.Contains(undead))
            {
                pool.Push(undead);
            }
        }
    }
    public IEnumerator ReturnUndeadWithDelay(SummonerUndead undead, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnUndead(undead);
    }
    public void ClearPool()
    {
        foreach (var pool in pools.Values)
        {
            pool.Clear();
        }
        for (int i = poolParent.childCount - 1; i >= 0; i--)
        {
            if (poolParent.GetChild(i).gameObject != null)
            {
                Destroy(poolParent.GetChild(i).gameObject);
            }
        }
        pools.Clear();
    }
}
