using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPooler : MonoBehaviour
{
    //Chỉ cần 1
    #region Singleton
    public static VFXPooler instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton
    public List<BaseVFX> VFXObject;
    public Transform poolParent; // nơi lưu trữ pool
    //List<T> với T là kiểu dữ liệu trong list
    //Chuẩn bị kệ chén
    //ID int = index của prefab hoặc ID của đạn
    //Stack<GameObject> = stack data của đạn đó
    private Dictionary<int, Stack<BaseVFX>> dictPools;
    // Start is called before the first frame update
    //Bắt đầu nhà mới
    //Mua chén để dành
    void Start()
    {
        //mua kệ = chuẩn bị kệ
        dictPools = new Dictionary<int, Stack<BaseVFX>>();
        //mua chén = tạo chén
        //nhu cầu giả sử bằng 10 chén
        for (int id = 0; id < VFXObject.Count; id++)
        {
            for (int i = 0; i < 10; i++)
            {
                CreateVFX(id);
            }
        }
    }


    //Sinh ra 1 mẫu
    public BaseVFX CreateVFX(int id)
    {
        BaseVFX VFXPrefab = VFXObject[id];
        BaseVFX VFX = Instantiate(VFXPrefab, poolParent);
        //tắt chén
        VFX.gameObject.SetActive(false);
        //bỏ chén mới vào kệ stack
        //lấy từ dictionary
        //nếu đã có stack -> push vào
        if (dictPools.TryGetValue(id, out Stack<BaseVFX> pool))
        {
            pool.Push(VFX);
        }
        else
        {
            pool = new Stack<BaseVFX>();
            pool.Push(VFX);
            dictPools[id] = pool;
        }
        return VFX;
    }

    //Lấy object ra sử dụng
    //lấy đạn nào
    public BaseVFX GetVFX(int id)
    {
        //ktra trong dictionary có stack của id này ko?
        //dictPools.TryGetValue(id, out Stack<GameObject> pool)
        Stack<BaseVFX> pool = null;
        if (dictPools.ContainsKey(id))
        {
            pool = dictPools[id];
        }
        else
        {
            //tạo item & tạo luôn stack
            CreateVFX(id);
            pool = dictPools[id];
        }
        //ktra trên kệ còn item nào đang sẵn không
        //Nếu có -> return ra dùng
        if (pool.Count > 0)
        {
            //lấy ra
            BaseVFX VFX = pool.Pop();
            VFX.gameObject.SetActive(true);
            return VFX;
        }
        //Nếu không:? -? tạo mới và return ra dùng
        else
        {
            BaseVFX VFX = CreateVFX(id);
            VFX.gameObject.SetActive(true);
            return VFX;
        }
    }
    //Xài xong thì trả về
    public void ReturnVFX(BaseVFX VFX)
    {
        VFX.transform.SetParent(poolParent);
        VFX.gameObject.SetActive(false);
        //trả chén về lại kệ tương ứng
        //ktra có stack trong dict
        if (dictPools.TryGetValue(VFX.VFX_ID, out Stack<BaseVFX> pool))
        {
            if (!pool.Contains(VFX))
            pool.Push(VFX);
        }
        //Viết thế này nguy hiểm
        //dictPools[bullet.VFX_ID].Push(bullet);
    }

    //tiêu hủy toàn bộ
    public void ClearPool()
    {
        // 1. Xóa sạch các Stack trong Dictionary
        foreach (var pool in dictPools.Values)
        {
            pool.Clear();
        }

        // 2. Xóa sạch tất cả Object thực tế trong Scene
        // Chúng ta duyệt ngược từ cuối danh sách con để tránh lỗi index khi Destroy
        for (int i = poolParent.childCount - 1; i >= 0; i--)
        {
            Destroy(poolParent.GetChild(i).gameObject);
        }
    }
}
