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
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
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
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.MoveButton_Sound);
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
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.MoveButton_Sound);
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
                    CurrencySaveManager.instance.RemoveGem(CharacterSaveManager.instance.allCharacters[currentIndex].characterProfile.GemRequire);
                    CurrencySaveManager.instance.RemoveDiamonds(CharacterSaveManager.instance.allCharacters[currentIndex].characterProfile.DiamondRequire);
                    UpdateCurrencyTexts();
                    // Thay đổi trạng thái nút
                    purchaseButton.SetActive(false);
                    cantpurchaseButton.SetActive(false);
                    ownedButton.SetActive(true);
                    if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.BuyCharacter_Sound);
                }
            }
        }
    }
    private string HiddenDetectionText(CharacterProfile character)
    {
        for (int level = 0; level < character.characterLevelDatas.Count; level++)
        {
            if (character.characterLevelDatas[level].hasHiddenDetection)
            {
                if (level == 0) return "Yes";
                else return "At level " + level;
            }
        }
        return "No";
    }
    private string StrikethroughText(CharacterProfile character)
    {
        for (int level = 0; level < character.characterLevelDatas.Count; level++)
        {
            if (character.characterLevelDatas[level].canStrikethrough)
            {
                if (level == 0) return "Yes";
                else return "At level " + level;
            }
        }
        return "No";
    }
    private void ShowCharacter(int allCharacter_index)
    {
        if (CharacterSaveManager.instance != null)
        {
            if (CharacterSaveManager.instance.allCharacters[allCharacter_index] != null)
            {
                CharacterName.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.CharacterName;
                CharacterImage.sprite = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.CharacterImage;
                RangeStat.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.characterLevelDatas[0].RangeStat.ToString();
                DamageStat.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.characterLevelDatas[0].DamageStat.ToString();
                CooldownStat.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.characterLevelDatas[0].CooldownStat.ToString();
                CostStat.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.CostStat.ToString();
                Tier.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.Tier;
                Description.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.Description;
                Special.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.Special;
                HiddenDetection.text = HiddenDetectionText(CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile);
                Strikethrough.text = StrikethroughText(CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile);
                GemRequire.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.GemRequire.ToString();
                DiamondRequire.text = CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.DiamondRequire.ToString();
            }
            // Kiểm tra xem đã có character hay chưa
            bool hasOwned = false;
            if (AccountSaveManager.CurrentAccount != null)
            {
                for (int i = 0; i < AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters.Count; i++)
                {
                    if (CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.CharacterName == AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters[i].characterProfile.CharacterName)
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
                if (playergem < CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.GemRequire || playerdiamonds < CharacterSaveManager.instance.allCharacters[allCharacter_index].characterProfile.DiamondRequire)
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
