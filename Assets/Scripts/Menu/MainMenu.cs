using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public TextMeshProUGUI Username_Text;
    public TextMeshProUGUI Gems_Text;
    public TextMeshProUGUI Diamonds_Text;
    public GameObject Setting_Popup;
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
        if (AccountSaveManager.instance != null)
        {
            Username_Text.text = AccountSaveManager.CurrentAccount.Username;
            Gems_Text.text = AccountSaveManager.CurrentAccount.CurrencyData.UserGems.ToString();
            Diamonds_Text.text = AccountSaveManager.CurrentAccount.CurrencyData.UserDiamonds.ToString();
        }
        Setting_Popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shop()
    {
        SceneManager.LoadScene(SceneKey.ShopScene);
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneKey.MapChoose);
    }
    public void Setting()
    {
        Setting_Popup.SetActive(true);
    }
    public void TurnOffSetting()
    {
        Setting_Popup.SetActive(false);
    }
    public void LogOut()
    {
        SceneManager.LoadScene(SceneKey.UserRegister);
    }
    public void QuitGame()
    {
        // 1. GỌI LỆNH LƯU DỮ LIỆU TẠI ĐÂY (Ví dụ)
        // SaveSystem.Save(playerData);
        Debug.Log("Đang lưu dữ liệu và thoát game...");
        // 2. THOÁT GAME SAU KHI ĐÃ BUILD (PC, Android, iOS)
        Application.Quit();
        // 3. THOÁT CHẾ ĐỘ PLAY TRONG UNITY EDITOR
        // Đoạn này giúp bạn test nút Quit ngay khi đang làm game
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
