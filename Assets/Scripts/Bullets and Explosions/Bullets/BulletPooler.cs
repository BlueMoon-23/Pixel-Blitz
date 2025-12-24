using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    // Singleton
    public static BulletPooler instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    // Mẫu chén
    public BaseBullets bulletPrefab; // => chuyển qua list thành các mẫu chén cần dùng
    // Nơi lưu trữ chén
    public Transform poolParent;
    // Chuẩn bị kệ chén
    // ID int = index của loại đạn
    private Stack<BaseBullets> pool = new Stack<BaseBullets>();
    // => Chuyển qua dictionary
    private Dictionary<int, Stack<BaseBullets>> pools;
    // Bullet Pooling
    /* Khởi tạo: dùng stack / queue, no list, no array. Khởi tạo ở start / awake
     * 
     * 
     * 
     */
    // 
    private void Start()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            CreateBullet();
        }
    }
    public BaseBullets CreateBullet() // Truyền id vào
    {
        //BaseBullets bulletPrefab = bulletPrefabs[id]
        BaseBullets bullet = Instantiate(bulletPrefab, poolParent);
        // Tắt chén đi
        bullet.gameObject.SetActive(false);
        // Bỏ chén mới vào kệ
        // Lấy từ dictionary
        // nếu đã có stack => push vào
        pool.Push(bullet);
        return bullet;
    }
    // Lấy bullet ra dùng
    public BaseBullets GetBullet()
    {
        // Kiểm tra trên kệ còn bullet nào đang có sẵn không
        if (pool.Count > 0)
        {
            BaseBullets bullet = pool.Pop();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        // Nếu có => return ra dùng
        else
        {
            return CreateBullet();
        }
        // Nếu không: tạo mới và return ra dùng
    }
    // Xài xong thì trả về
    public void ReturnBullet(BaseBullets bullet)
    {
        // ResetBullet
        bullet.transform.SetParent(poolParent);
        bullet.gameObject.SetActive(false);
        // Trả chén về lại kệ
        pool.Push(bullet);
    }
    // Tiêu hủy toàn bộ
    public void PoolClear()
    {
        foreach (BaseBullets bullet in pool)
        {
            Destroy(bullet.gameObject);
        }
        pool.Clear();
    }
}
