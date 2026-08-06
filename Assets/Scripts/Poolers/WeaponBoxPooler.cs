using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WeaponBox
{
    public WeaponRarity Rarity;
    public WeaponBoxUI boxUI;
}

public class WeaponBoxPooler : MonoBehaviour
{
    // Singleton
    public static WeaponBoxPooler instance;
    public List<WeaponBox> WeaponBoxPrefabs;
    public Transform poolParent; // Chỗ để về
    private Dictionary<WeaponRarity, WeaponBoxUI> WeaponBoxByRarity = new Dictionary<WeaponRarity, WeaponBoxUI>();
    private Dictionary<WeaponRarity, Stack<WeaponBoxUI>> pools = new Dictionary<WeaponRarity, Stack<WeaponBoxUI>>();
    // HashSet chứa các WeaponBoxUI đang hiển thị bên ngoài. Sử dụng HashSet để khi return còn tìm WeaponBoxUI
    private HashSet<WeaponBoxUI> ActivePool = new HashSet<WeaponBoxUI>(); 
    private WeaponRarity[] Rarities = { WeaponRarity.Common, WeaponRarity.Rare, WeaponRarity.Legendary };
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
        foreach (WeaponBox wb in WeaponBoxPrefabs)
        {
            WeaponBoxByRarity.Add(wb.Rarity, wb.boxUI);
        }
    }
    private void Start()
    {
        poolParent = this.transform;
        for (int i = 0; i <  WeaponBoxPrefabs.Count; i++)
        {
            CreateWeaponBox(Rarities[i]);
        }
    }
    public WeaponBoxUI CreateWeaponBox(WeaponRarity rarity)
    {
        WeaponBoxUI weaponBoxUI = Instantiate(WeaponBoxByRarity[rarity], poolParent);
        weaponBoxUI.gameObject.SetActive(false);
        // Nếu dictionary có tồn tại key rarity, pool = stack và rarity trỏ về. Nếu không pool = null
        if (pools.TryGetValue(rarity, out Stack<WeaponBoxUI> pool))
        {
            pool.Push(weaponBoxUI);
        }
        else
        {
            pool = new Stack<WeaponBoxUI>();
            pool.Push(weaponBoxUI);
            pools.Add(rarity, pool);
        }
        return weaponBoxUI;
    }
    public WeaponBoxUI GetWeaponBox(WeaponRarity rarity)
    {
        Stack<WeaponBoxUI> pool = null;
        if (pools.ContainsKey(rarity))
        {
            pool = pools[rarity];
        }
        else
        {
            CreateWeaponBox(rarity);
            pool = pools[rarity];
        }
        if (pool.Count == 0) CreateWeaponBox(rarity);
        WeaponBoxUI weaponBoxUI = pool.Pop();
        weaponBoxUI.gameObject.SetActive(true);
        ActivePool.Add(weaponBoxUI);
        return weaponBoxUI;
    }
    public void ReturnWeaponBox(WeaponBoxUI weaponBoxUI)
    {
        if (weaponBoxUI != null)
        {
            weaponBoxUI.transform.SetParent(poolParent);
            weaponBoxUI.gameObject.SetActive(false);
            if (pools.TryGetValue(weaponBoxUI.Rarity, out Stack<WeaponBoxUI> pool))
            {
                if (!pool.Contains(weaponBoxUI))
                {
                    pool.Push(weaponBoxUI);
                    if (ActivePool.Contains(weaponBoxUI)) ActivePool.Remove(weaponBoxUI);
                }
            }
        }
    }
    public void ReturnAllWeaponBox()
    {
        // Tạo danh sách tạm để foreach, không lo bị lỗi modified collection
        foreach (WeaponBoxUI weaponBoxUI in ActivePool.ToList())
        {
            ReturnWeaponBox(weaponBoxUI);
        }
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
