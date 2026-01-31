using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    // Singleton
    public static BulletPooler instance;
    public List<BaseBullets> bulletPrefabs; // mẫu đạn
    public Transform poolParent; // nơi lưu trữ
    // Chuẩn bị kệ chén
    // ID int = index của loại đạn
    private Dictionary<int, Stack<BaseBullets>> pools;
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
        pools = new Dictionary<int, Stack<BaseBullets>>();
        for (int id = 0; id < bulletPrefabs.Count; id++)
        {
            for (int i = 0; i < 10; i++)
            {
                CreateBullet(id);
            }
        }
    }
    public BaseBullets CreateBullet(int id)
    {
        BaseBullets bullet = Instantiate(bulletPrefabs[id], poolParent);
        bullet.BulletID = id; // đảm bảo đạn mới sinh ra sẽ có id chính xác để còn thu hồi
        bullet.gameObject.SetActive(false);
        // Bỏ chén mới vào kệ
        // Lấy từ dictionary
        // nếu đã có stack => push vào
        if (pools.TryGetValue(id, out Stack<BaseBullets> pool))
        {
            pool.Push(bullet);
        }
        else
        {
            pool = new Stack<BaseBullets>();
            pool.Push(bullet);
            pools[id] = pool;
        }
        return bullet;
    }
    // Lấy bullet ra dùng
    public BaseBullets GetBullet(int id)
    {
        //ktra trong dictionary có stack của id này ko?
        //dictPools.TryGetValue(id, out Stack<BaseBullet> pool)
        Stack<BaseBullets> pool = null;
        if (pools.ContainsKey(id))
        {
            pool = pools[id];
        }
        else
        {
            //tạo item & tạo luôn stack
            CreateBullet(id);
            pool = pools[id];
        }
        //ktra trên kệ còn item nào đang sẵn không
        //Nếu có -> return ra dùng
        if (pool.Count > 0)
        {
            //lấy ra
            BaseBullets bullet = pool.Pop();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        //Nếu không:? -? tạo mới và return ra dùng
        else
        {
            BaseBullets bullet = CreateBullet(id);
            bullet.gameObject.SetActive(true);
            return bullet;
        }
    }
    //Xài xong thì trả về
    public void ReturnBullet(BaseBullets bullet)
    {
        //reset bullet
        if (bullet != null)
        {
            bullet.transform.SetParent(poolParent);
            bullet.gameObject.SetActive(false);
            //trả chén về lại kệ tương ứng
            //ktra có stack trong dict
            if (pools.TryGetValue(bullet.BulletID, out Stack<BaseBullets> pool))
            {
                if (!pool.Contains(bullet))
                {
                    pool.Push(bullet);
                }
            }
        }
    }
    public IEnumerator ReturnBulletWithDelay(BaseBullets bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnBullet(bullet);
    }
    //tiêu hủy toàn bộ
    public void ClearPool()
    {
        // 1. Xóa sạch các Stack trong Dictionary
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
    // Hàm dành riêng cho rocketeer bullet
    public IEnumerator DestroyCluster(RocketeerBullet ClusterRocketBullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (ClusterRocketBullet.isCluster)
        {
            ClusterRocketBullet.Explode();
        }
        ReturnBullet(ClusterRocketBullet);
        yield break;
    }
}
