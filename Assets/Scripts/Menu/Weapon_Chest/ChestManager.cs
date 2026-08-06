using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour
{
    public List<WeaponChest> WeaponChests = new List<WeaponChest>();
    private WeaponDropService service;
    [SerializeField] private List<Image> ButtonImages = new List<Image>();
    [SerializeField] private Sprite SelectFrame;
    [SerializeField] private Sprite NotSelectFrame;
    private int CurrentChestIndex;
    //
    private Sequence weaponSequence;
    [SerializeField] private Image CurrentChest;
    [SerializeField] private GameObject DropListLayoutGroup;
    [Header("Các nhóm UI")]
    [SerializeField] private List<GameObject> UIGroups = new List<GameObject>();
    [Header("Tiền người chơi")]
    [SerializeField] private TextMeshProUGUI Gems_Text;
    [SerializeField] private TextMeshProUGUI Diamonds_Text;
    [SerializeField] private TextMeshProUGUI GemRequire;
    [SerializeField] private TextMeshProUGUI DiamondRequire;
    [SerializeField] private GameObject purchaseButton;
    [SerializeField] private GameObject cantpurchaseButton;
    [SerializeField] private GameObject IsProcessingButton;
    private bool isProcessing;
    public bool hasEnhanced { get; private set; }
    [Header("Nâng cấp rương")]
    [SerializeField] private Image EnhanceChestCheckBox;
    [SerializeField] private Sprite DoEnhanceSprite;
    [SerializeField] private Sprite DoNotEnhanceSprite;
    public static ChestManager instance;
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
        service = GetComponent<WeaponDropService>();
        hasEnhanced = false;
    }
    private void Start()
    {
        UpdateCurrencyTexts();
        ShowHSChest();
    }
    public void ShowDropList(WeaponChest Chest)
    {
        CurrentChest.sprite = Chest.GetComponent<SpriteRenderer>().sprite;
        Dictionary<WeaponRarity, int> UnlockedWeaponCountByRarity = new Dictionary<WeaponRarity, int>
        {
            [WeaponRarity.Common] = 0,
            [WeaponRarity.Rare] = 0,
            [WeaponRarity.Legendary] = 0
        };
        Dictionary<WeaponRarity, float> ChanceByRarity = new Dictionary<WeaponRarity, float>()
        {
            [WeaponRarity.Common] = 0,
            [WeaponRarity.Rare] = 0,
            [WeaponRarity.Legendary] = 0
        };
        // Tạo HashSet 1 lần duy nhất để tra cứu O(1)
        HashSet<string> ownedCharacterNames = new HashSet<string>(
            AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters
                .Select(c => c.characterProfile.CharacterName)
        );
        List<WeaponProfile> dropList = Chest.GetWeaponDropListFromService();
        // Lưu kết quả hasOwner của từng profile để dùng lại ở vòng lặp dưới
        Dictionary<WeaponProfile, bool> hasOwnerCache = new Dictionary<WeaponProfile, bool>();
        foreach (WeaponProfile profile in dropList)
        {
            bool hasOwner = ownedCharacterNames.Contains(profile.WeaponOwner.CharacterName);
            hasOwnerCache[profile] = hasOwner;
            if (hasOwner)
            {
                UnlockedWeaponCountByRarity[profile.WeaponRarity.weaponRarity]++;
                if (!hasEnhanced) ChanceByRarity[profile.WeaponRarity.weaponRarity] = profile.WeaponRarity.NormalDropChance;
                else ChanceByRarity[profile.WeaponRarity.weaponRarity] = profile.WeaponRarity.EnhancedDropChance;
            }
        }
        float TotalChance = 0;
        foreach (var chance in ChanceByRarity)
        {
            TotalChance += chance.Value;
        }
        // Tránh lỗi chia cho 0 (DivideByZeroException / NaN)
        if (TotalChance > 0)
        {
            foreach (var key in ChanceByRarity.Keys.ToList())
            {
                ChanceByRarity[key] /= TotalChance; // duyệt bằng key để sửa
            }
        }
        foreach (WeaponProfile profile in dropList)
        {
            WeaponBoxUI weaponBoxUI = null;
            if (WeaponBoxPooler.instance != null)
            {
                weaponBoxUI = WeaponBoxPooler.instance.GetWeaponBox(profile.WeaponRarity.weaponRarity);
            }
            else
            {
                Debug.Log("pooler bi null");
                return;
            }
            if (weaponBoxUI != null)
            {
                weaponBoxUI.transform.position = DropListLayoutGroup.transform.position;
                weaponBoxUI.transform.SetParent(DropListLayoutGroup.transform);
                weaponBoxUI.transform.localScale = new Vector3(1, 1, 1);
                weaponBoxUI.transform.rotation = Quaternion.identity;
                weaponBoxUI.Weapon.weaponData.weaponProfile = profile;
                weaponBoxUI.UpdateWeapon();
                // Dùng lại kết quả đã tính ở vòng lặp trên, không tìm lại nữa
                bool hasOwner = hasOwnerCache[profile];
                WeaponPreviewBox weaponPreviewBox = weaponBoxUI as WeaponPreviewBox;
                if (weaponPreviewBox != null)
                {
                    weaponPreviewBox.InitializeWeapon(profile, ChanceByRarity[profile.WeaponRarity.weaponRarity], UnlockedWeaponCountByRarity[profile.WeaponRarity.weaponRarity], hasOwner);
                }
            }
        }
        ShowChestCost();
    }
    private void ShowChestCost()
    {
        // Kiểm tra xem đã có đủ tiền để mua character chưa
        int playergem = AccountSaveManager.CurrentAccount.CurrencyData.UserGems;
        int playerdiamonds = AccountSaveManager.CurrentAccount.CurrencyData.UserDiamonds;
        if (playergem < 1000 || (hasEnhanced && playerdiamonds < 100))
        {
            purchaseButton.SetActive(false);
            cantpurchaseButton.SetActive(true);
        }
        else
        {
            purchaseButton.SetActive(true);
            cantpurchaseButton.SetActive(false);
        }
    }
    public void ShowWeaponDropped(WeaponProfile weaponProfile, List<Perk> weaponPerks)
    {
        service.ShowWeaponDropped(weaponProfile, weaponPerks);
        // Reset tween cũ nếu có
        if (weaponSequence != null && weaponSequence.IsActive())
        {
            weaponSequence.Kill();
        }
        weaponSequence = DOTween.Sequence();
        weaponSequence.Append(service.frameCanvasGroup.DOFade(1f, 1f).From(0f))
                      .AppendInterval(1f)
                      .Append(service.frameCanvasGroup.DOFade(0f, 1f))
                      .AppendCallback(() =>
                      {
                          service.frameCanvasGroup.gameObject.SetActive(false);
                          WeaponUIControll.instance.WeaponUI.gameObject.SetActive(false);
                      });
        // Bật SFX riêng theo rarity
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.UISource.PlayOneShot(weaponProfile.WeaponRarity.RaritySFX);
        }
    }
    public void BuyChest()
    {
        StartCoroutine(DoOpenChest());
    }
    public IEnumerator DoOpenChest()
    {
        isProcessing = true;
        // Trừ tiền và cập nhật lên 2 thanh tiền trên đầu
        CurrencySaveManager.instance.RemoveGem(1000);
        if (!hasEnhanced) CurrencySaveManager.instance.RemoveDiamonds(0);
        else CurrencySaveManager.instance.RemoveDiamonds(100);
        UpdateCurrencyTexts();
        IsProcessingButton.SetActive(true);
        purchaseButton.SetActive(false);
        cantpurchaseButton.SetActive(false);
        CurrentChest.gameObject.SetActive(false);
        // Tắt các UI bên ngoài
        foreach (GameObject group in UIGroups)
        {
            group.SetActive(false);
        }
        // Dừng BGM, bật SFX chest
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PauseBGM();
            SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Chest_Opening);
        }
        GameObject Chest = Instantiate(WeaponChests[CurrentChestIndex].gameObject, new Vector3(0, -0.8f, 0), Quaternion.identity);
        Destroy(Chest, 1.5f);
        yield return new WaitForSeconds(1.5f);
        isProcessing = false;
        IsProcessingButton.SetActive(false);
        CurrentChest.gameObject.SetActive(true);
        // Bật các UI bên ngoài
        foreach (GameObject group in UIGroups)
        {
            group.SetActive(true);
        }
        // Tiếp tục BGM
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ResumeBGM();
        }
        ShowChestCost();
    }
    public void ShowChest()
    {
        WeaponBoxPooler.instance.ReturnAllWeaponBox();
        ShowDropList(WeaponChests[CurrentChestIndex]);
        ButtonMove(CurrentChestIndex);
    }
    public void ShowHSChest()
    {
        if (isProcessing) return;
        CurrentChestIndex = 0;
        ShowChest();
    }
    public void ShowRFChest()
    {
        if (isProcessing) return;
        CurrentChestIndex = 1;
        ShowChest();
    }
    public void ShowDIChest()
    {
        if (isProcessing) return;
        CurrentChestIndex = 2;
        ShowChest();
    }
    private void ButtonMove(int index)
    {
        for (int i = 0; i < ButtonImages.Count; i++)
        {
            if (i == index)
            {
                ButtonImages[i].sprite = SelectFrame;
            }
            else
            {
                ButtonImages[i].sprite = NotSelectFrame;
            }
        }
    }
    public void TickEnhanceCheckBox()
    {
        if (!hasEnhanced)
        {
            hasEnhanced = true;
            EnhanceChestCheckBox.sprite = DoEnhanceSprite;
            DiamondRequire.text = 100f.ToString();
        }
        else
        {
            hasEnhanced = false;
            EnhanceChestCheckBox.sprite = DoNotEnhanceSprite;
            DiamondRequire.text = 0f.ToString();
        }
        ShowChestCost();
        ShowChest();
    }
    public void BackToMainMenu()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        SceneManager.LoadScene(SceneKey.MainMenu);
    }
    private void UpdateCurrencyTexts()
    {
        if (AccountSaveManager.instance != null)
        {
            Gems_Text.text = AccountSaveManager.CurrentAccount.CurrencyData.UserGems.ToString();
            Diamonds_Text.text = AccountSaveManager.CurrentAccount.CurrencyData.UserDiamonds.ToString();
        }
    }
}
