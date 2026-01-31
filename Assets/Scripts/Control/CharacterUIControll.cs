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
    //
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public RectTransform UpgradeContent;
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI Info1;
    public TextMeshProUGUI Info2;
    public TextMeshProUGUI Info3;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI sellCost;
    public TextMeshProUGUI RangeStats;
    public TextMeshProUGUI DamageStats;
    public TextMeshProUGUI CooldownStats;
    //
    public BaseCharacter CurrentCharacter;
    public CanvasGroup HiddenDetectionIcon;
    public CanvasGroup StrikethroughIcon;
    //
    public Button UpgradeButton;
    public Button AbilityButton;
    public Image AbilityCurrentIcon;
    public Sprite[] AbilityIcons;
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
            if (CurrentCharacter.GetUpgradeCost(CurrentCharacter.GetLevel()) <= EconomyManager.instance.PlayerCoin) 
            {
                CurrentCharacter.Upgrade();
                if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Upgrade_Sound);
                EconomyManager.instance.Purchase(CurrentCharacter.GetUpgradeCost(CurrentCharacter.GetLevel() - 1));
                EconomyManager.instance.Change_CurrentCoin();
            }
            else
            {
                EconomyManager.instance.Announce_CantUpgrade(CurrentCharacter.GetUpgradeCost(CurrentCharacter.GetLevel()));
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
}
