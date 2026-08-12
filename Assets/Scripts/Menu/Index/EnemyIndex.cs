using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class EnemyIndex : MonoBehaviour
{
    // Pooler cho enemy index box
    public static EnemyIndex instance;
    public EnemyBoxUI EnemyBoxPrefab;
    public Transform poolParent;
    public Gamemodes DefaultMode;
    private Queue<EnemyBoxUI> Pool = new Queue<EnemyBoxUI>(); // dùng queue để không bị lật danh sách khi gọi load 2 lần
    private HashSet<EnemyBoxUI> ActivePool = new HashSet<EnemyBoxUI>();
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI EnemyName;
    [SerializeField] private Image EnemyImage;
    [SerializeField] private TextMeshProUGUI EnemyHP;
    [SerializeField] private TextMeshProUGUI EnemySpeed;
    [SerializeField] private TextMeshProUGUI EnemyHiddenTag;
    [SerializeField] private TextMeshProUGUI EnemyArmoredTag;
    [SerializeField] private TextMeshProUGUI EnemyDescription;
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
    void Start()
    {
        poolParent = this.transform;
        LoadIndexByGamemode(DefaultMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public EnemyBoxUI CreateEnemyBoxUI(EnemyProfile profile)
    {
        EnemyBoxUI boxUI = Instantiate(EnemyBoxPrefab, poolParent);
        boxUI.SetInformation(profile);
        Pool.Enqueue(boxUI);
        return boxUI;
    }
    public EnemyBoxUI GetEnemyBoxUI(EnemyProfile profile)
    {
        if (Pool.Count == 0)
        {
            CreateEnemyBoxUI(profile);
        }
        EnemyBoxUI boxUI = Pool.Dequeue();
        boxUI.gameObject.SetActive(true);
        boxUI.SetInformation(profile);
        ActivePool.Add(boxUI);
        boxUI.transform.SetAsLastSibling(); // đưa về cuối danh sách, đảm bảo k bị xáo trộn
        return boxUI;
    }
    public void ReturnEnemyBoxUI(EnemyBoxUI boxUI)
    {
        boxUI.gameObject.SetActive(false);
        boxUI.transform.SetParent(poolParent);
        if (!Pool.Contains(boxUI))
        {
            Pool.Enqueue(boxUI);
            if (ActivePool.Contains(boxUI)) ActivePool.Remove(boxUI);
        }
    }
    public void ReturnAllBoxUI()
    {
        // Tạo danh sách tạm để foreach, không lo bị lỗi modified collection
        foreach (EnemyBoxUI BoxUI in ActivePool.ToList())
        {
            ReturnEnemyBoxUI(BoxUI);
        }
    }
    public void LoadIndexByGamemode(Gamemodes mode) 
    {
        List<EnemyProfile> ProfileList = new List<EnemyProfile>();
        List<EnemyEntry> EntryList = mode.enemyEntries.OrderBy(entry => entry.Enemy_Prefab.name).ToList();
        foreach (EnemyEntry entry in EntryList)
        {
            if (entry.Enemy_Prefab != null)
            {
                EnemyStats stats = entry.Enemy_Prefab.GetComponent<EnemyStats>();
                if (stats != null)
                {
                    ProfileList.Add(stats.enemyProfile);
                }
            }
        }
        ReturnAllBoxUI();
        foreach (EnemyProfile profile in ProfileList)
        {
            GetEnemyBoxUI(profile);
        }
    }
    public void ShowInformation(EnemyProfile profile)
    {
        EnemyName.text = profile.Name;
        EnemyImage.sprite = profile.EnemyImage;
        EnemyHP.text = profile.MaxHP.ToString();
        EnemySpeed.text = profile.OldSpeed.ToString();
        EnemyHiddenTag.text = profile.isHidden ? "Yes" : "No";
        EnemyArmoredTag.text = profile.isArmored ? "Yes" : "No";
        EnemyDescription.text = profile.Description;
    }
}
