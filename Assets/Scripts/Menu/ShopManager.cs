using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // Tiền người chơi
    public TextMeshProUGUI Gems_Text;
    public TextMeshProUGUI Diamonds_Text;
    // Thông tin character
    // Cơ bản
    public TextMeshProUGUI CharacterName;
    public Image CharacterImage;
    public TextMeshProUGUI RangeStat;
    public TextMeshProUGUI DamageStat;
    public TextMeshProUGUI CooldownStat;
    public TextMeshProUGUI CostStat;
    // Của UI
    public TextMeshProUGUI Tier;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Special;
    public TextMeshProUGUI HiddenDetection;
    public TextMeshProUGUI Strikethrough;
    // Giá tiền
    public TextMeshProUGUI GemRequire;
    public TextMeshProUGUI DiamondRequire;
    private int currentIndex = 0;
    public GameObject purchaseButton;
    public GameObject cantpurchaseButton;
    public GameObject ownedButton;
    public static ShopManager instance;
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
        if (AccountSaveManager.instance != null)
        {
            AccountSaveManager.instance.LoadAccounts();
        }
    }
    void Start()
    {
        UpdateCurrencyTexts();
        ShowCharacter(currentIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneKey.MainMenu);
    }
    public void Previous()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
        }
        else
        {
            currentIndex = CharacterSaveManager.instance.allCharacters.Count - 1;
        }
        ShowCharacter(currentIndex);
    }
    public void Next()
    {
        if (currentIndex < CharacterSaveManager.instance.allCharacters.Count - 1)
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
        ShowCharacter(currentIndex);
    }
    public void Purchase()
    {
        if (CharacterSaveManager.instance != null)
        {
            if (AccountSaveManager.CurrentAccount != null)
            {
                if (CurrencySaveManager.instance != null)
                {
                    // Thêm character vào túi đồ
                    CharacterSaveManager.instance.BuyCharacter(CharacterSaveManager.instance.allCharacters[currentIndex]);
                    // Trừ tiền và cập nhật lên 2 thanh tiền trên đầu
                    CurrencySaveManager.instance.RemoveGem(CharacterSaveManager.instance.allCharacters[currentIndex].GemRequire);
                    CurrencySaveManager.instance.RemoveDiamonds(CharacterSaveManager.instance.allCharacters[currentIndex].DiamondRequire);
                    UpdateCurrencyTexts();
                    // Thay đổi trạng thái nút
                    purchaseButton.SetActive(false);
                    cantpurchaseButton.SetActive(false);
                    ownedButton.SetActive(true);
                }
            }
        }
    }
    private void ShowCharacter(int allCharacter_index)
    {
        if (CharacterSaveManager.instance != null)
        {
            if (CharacterSaveManager.instance.allCharacters[allCharacter_index] != null)
            {
                CharacterName.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].CharacterName;
                CharacterImage.sprite = CharacterSaveManager.instance.allCharacters[allCharacter_index].CharacterImage;
                RangeStat.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].RangeStat.ToString();
                DamageStat.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].DamageStat.ToString();
                CooldownStat.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].CooldownStat.ToString();
                CostStat.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].CostStat.ToString();
                Tier.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].Tier;
                Description.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].Description;
                Special.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].Special;
                HiddenDetection.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].HiddenDetection;
                Strikethrough.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].Strikethrough;
                GemRequire.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].GemRequire.ToString();
                DiamondRequire.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].DiamondRequire.ToString();
            }
            // Kiểm tra xem đã có character hay chưa
            bool hasOwned = false;
            if (AccountSaveManager.CurrentAccount != null)
            {
                for (int i = 0; i < AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters.Count; i++)
                {
                    if (CharacterSaveManager.instance.allCharacters[allCharacter_index].CharacterName == AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters[i].CharacterName)
                    {
                        hasOwned = true;
                        break;
                    }
                }
            }
            purchaseButton.SetActive(!hasOwned);
            cantpurchaseButton.SetActive(!hasOwned);
            ownedButton.SetActive(hasOwned);
            // Kiểm tra xem đã có đủ tiền để mua character chưa
            if (!ownedButton.activeInHierarchy) // Nghĩa là chưa có, mới xem có đủ tiền chưa
            {
                int playergem = AccountSaveManager.CurrentAccount.CurrencyData.UserGems;
                int playerdiamonds = AccountSaveManager.CurrentAccount.CurrencyData.UserDiamonds;
                if (playergem < CharacterSaveManager.instance.allCharacters[allCharacter_index].GemRequire || playerdiamonds < CharacterSaveManager.instance.allCharacters[allCharacter_index].DiamondRequire)
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
        }
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
