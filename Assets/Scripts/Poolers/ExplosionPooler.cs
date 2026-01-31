using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPooler : MonoBehaviour
{
    public static ExplosionPooler instance;
    public List<BaseExplosion> explosion_Prefabs;
    public Transform poolParent;
    private Dictionary<int, Stack<BaseExplosion>> pools;
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
        pools = new Dictionary<int, Stack<BaseExplosion>>();
        for (int id = 0; id < explosion_Prefabs.Count; id++)
        {
            for (int i = 0; i < 5; i++)
            {
                CreateExplosion(id);
            }
        }
    }
    public BaseExplosion CreateExplosion(int id)
    {
        BaseExplosion explosion = Instantiate(explosion_Prefabs[id], poolParent);
        explosion.ExplosionID = id;
        explosion.gameObject.SetActive(false);
        if (pools.TryGetValue(id, out Stack<BaseExplosion> pool))
        {
            pool.Push(explosion);
        }
        else
        {
            pool = new Stack<BaseExplosion>();
            pool.Push(explosion);
            pools[id] = pool;
        }
        return explosion;
    }
    public BaseExplosion GetExplosion(int id)
    {
        Stack<BaseExplosion> pool = null;
        if (pools.ContainsKey(id))
        {
            pool = pools[id];
        }
        else
        {
            CreateExplosion(id);
            pool = pools[id];
        }
        if (pool.Count > 0)
        {
            BaseExplosion explosion = pool.Pop();
            explosion.gameObject.SetActive(true);
            return explosion;
        }
        else
        {
            BaseExplosion explosion = CreateExplosion(id);
            explosion.gameObject.SetActive(true);
            return explosion;
        }
    }
    public void ReturnExplosion(BaseExplosion explosion)
    {
        if (explosion != null)
        {
            explosion.transform.SetParent(poolParent);
            explosion.gameObject.SetActive(false);
            if (pools.TryGetValue(explosion.ExplosionID, out Stack<BaseExplosion> pool))
            {
                if (!pool.Contains(explosion))
                {
                    pool.Push(explosion);
                }
            }
        }
    }
    public IEnumerator ReturnExplosionWithDelay(BaseExplosion explosion, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnExplosion(explosion);
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
