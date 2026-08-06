using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterUIControll : MonoBehaviour
{
    public static CharacterUIControll instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private GameObject[] Range_Prefab;
    public Sprite[] characterImages;
    public BaseCharacter CurrentCharacter;
    //
    [Header("Character")]
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public Image characterGlow;
    [Header("Total Damage")]
    public TextMeshProUGUI characterTotalDamage;
    [Header("Attack Priority")]
    public TextMeshProUGUI characterAttackPriority;
    [Header("Upgrade")]
    public RectTransform UpgradeContent;
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI upgradeCost;
    [Header("Stats")]
    public TextMeshProUGUI sellCost;
    public TextMeshProUGUI RangeStats;
    public TextMeshProUGUI DamageStats;
    public TextMeshProUGUI CooldownStats;
    //
    [Header("Icon")]
    public CanvasGroup HiddenDetectionIcon;
    public CanvasGroup StrikethroughIcon;
    //
    [Header("Button")]
    public Button UpgradeButton;
    public Button AbilityButton;
    public Image AbilityCurrentIcon;
    public Sprite[] AbilityIcons;
    [Header("Info Pool Settings")]
    [SerializeField] private TextMeshProUGUI infoPrefab; // Kéo Prefab dòng chữ vào đây
    [SerializeField] private Transform container;      // Kéo UI Panel chứa layout vào đây
    // Hash Map quản lý các dòng text theo chỉ số (Index)
    private Dictionary<int, TextMeshProUGUI> infoMap = new Dictionary<int, TextMeshProUGUI>();
    /// <summary>
    /// Lấy hoặc tạo mới một dòng Info dựa trên Index
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && UpgradeButton.isActiveAndEnabled) 
        {
            Upgrade();
        }
    }
    public void UI_Off()
    {
        gameObject.SetActive(false);
        Range_Prefab = GameObject.FindGameObjectsWithTag("Range");
        for (int i = 0; i < Range_Prefab.Length; i++)
        {
            Range_Prefab[i].GetComponent<Renderer>().enabled = false;
        }
        // Wizard tắt script dragability, nên phải bật lại ở nút close để character khác còn dùng
        if (DragAbility.instance != null)
        {
            DragAbility.instance.enabled = true;
        }
    }
    public void Upgrade()
    {
        if (EconomyManager.instance != null)
        {
            if (CurrentCharacter.GetUpgradeCost() <= EconomyManager.instance.PlayerCoin) 
            {
                EconomyManager.instance.Purchase(CurrentCharacter.GetUpgradeCost());
                EconomyManager.instance.Change_CurrentCoin();
                if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Upgrade_Sound);
                CurrentCharacter.Upgrade();
            }
            else
            {
                EconomyManager.instance.Announce_CantUpgrade(CurrentCharacter.GetUpgradeCost());
            }
        }
        // Ngoai if
        UI_Off();
    }
    public void Sell()
    {
        if (EconomyManager.instance != null) 
        { 
            EconomyManager.instance.AddCoin(CurrentCharacter.GetSellCost()); 
            EconomyManager.instance.Change_CurrentCoin();
        }
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Sell_Sound);
        // RemoveCharacter không chạy đúng với MinigunnerClone
        if (CurrentCharacter.GetType() == typeof(MinigunnerClone))
        {
            CharacterManager.instance.RemovePosition(CurrentCharacter.transform.position);
            Destroy(CurrentCharacter.gameObject);
        }
        else
        {
            CharacterManager.instance.RemoveCharacter(CurrentCharacter); // trong remove có sẵn returncharacter rồi
        }
        //Destroy(CurrentCharacter.gameObject);
        gameObject.SetActive(false);
    }
    public void UseAbility()
    {
        // Minigunner và Summoner có cách gọi ability riêng rồi. Cái này chỉ dành cho wizard
        Wizard wizard = CurrentCharacter.GetComponent<Wizard>();
        if (wizard != null)
        {
            wizard.Ability(Vector3.zero); // Position không quan trọng đâu
        }
        else
        {
            Debug.Log("Wizard = null");
        }
    }
    public TextMeshProUGUI GetOrCreateInfo(int index)
    {
        // Kiểm tra xem trong Hash Map đã có index này chưa
        if (!infoMap.ContainsKey(index))
        {
            // Nếu chưa có, tạo mới từ Prefab và thêm vào Map
            var newInfo = Instantiate(infoPrefab, container);
            newInfo.name = $"Info_{index}";
            infoMap.Add(index, newInfo);
        }

        return infoMap[index];
    }
    public void TurnOffAllInfo()
    {
        foreach (var info in infoMap.Values)
        {
            info.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Tắt hết info ngoài 5 info chính
    /// </summary>
    public void TurnOffExternalInfo()
    {
        for (int i = 6; i < infoMap.Count; i++)
        {
            infoMap[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Chỉnh Attack Priority: First - Last - Closest - Farthest - Strongest - Weakest - Random
    /// </summary>
    public void ChangeAttackPriority()
    {
        string PriorityName = CurrentCharacter.characterAttack.MoveAttackPriority();
        characterAttackPriority.text = $"<< {PriorityName} >>";
    }
    public void ShowAttackPriority()
    {
        string PriorityName = CurrentCharacter.characterAttack.GetAttackPriority();
        characterAttackPriority.text = $"<< {PriorityName} >>";
    }
}
