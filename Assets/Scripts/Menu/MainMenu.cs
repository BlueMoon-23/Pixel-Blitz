using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public TextMeshProUGUI Username_Text;
    public TextMeshProUGUI Gems_Text;
    public TextMeshProUGUI Diamonds_Text;
    public GameObject Setting_Popup;
    public GameObject Giftcode_Popup;
    public GameObject Tutorial_Popup;
    public Slider musicSlider;
    public Slider UISlider;
    // Tutorial
    public MapData tutorialMap;
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
        ReloadAccount();
        Setting_Popup.SetActive(false);
        Giftcode_Popup.SetActive(false);
        if (PlayerPrefs.HasKey(UserDataKey.PLAYEDTUTORIAL))
        {
            Tutorial_Popup.SetActive(false);
        }
        else
        {
            Tutorial_Popup.SetActive(true);
        }
        if (SoundManager.Instance != null)
        {
            musicSlider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
            UISlider.onValueChanged.AddListener(SoundManager.Instance.SetUISoundsVolume);
        }
        if (PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("UISoundsVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            UISlider.value = PlayerPrefs.GetFloat("UISoundsVolume");
        }
        else
        {
            musicSlider.value = 1f;
            UISlider.value = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReloadAccount()
    {
        if (AccountSaveManager.instance != null)
        {
            Username_Text.text = AccountSaveManager.CurrentAccount.Username;
            Gems_Text.text = AccountSaveManager.CurrentAccount.CurrencyData.UserGems.ToString();
            Diamonds_Text.text = AccountSaveManager.CurrentAccount.CurrencyData.UserDiamonds.ToString();
        }
    }
    public void Shop()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.OpenButton_Sound);
        SceneKey.targetScene = SceneKey.ShopScene;
        SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
    }
    public void Tutorial()
    {
        if (ModeManager.instance != null) ModeManager.instance.Play(tutorialMap);
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.OpenButton_Sound);
        SceneKey.targetScene = SceneKey.TutorialScene;
        SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
    }
    public void Play()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.OpenButton_Sound);
        SceneKey.targetScene = SceneKey.MapChoose;
        SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
    }
    public void Setting()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Setting_Sound);
        Setting_Popup.SetActive(true);
    }
    public void Giftcode()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Setting_Sound);
        Giftcode_Popup.SetActive(true);
    }
    public void TurnOffSetting()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Setting_Sound);
        Setting_Popup.SetActive(false);
    }
    public void TurnOffGiftcode()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Setting_Sound);
        Giftcode_Popup.SetActive(false);
    }
    public void TurnOffTutorial()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        Tutorial_Popup.SetActive(false);
    }
    public void LogOut()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        SceneKey.targetScene = SceneKey.UserRegister;
        SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
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
