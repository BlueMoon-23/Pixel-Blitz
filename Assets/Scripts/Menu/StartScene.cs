using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public static StartScene instance;
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
    public GameObject SignUp_Popup;
    public GameObject LogIn_Popup;
    void Start()
    {
        SignUp_Popup.SetActive(false);
        LogIn_Popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartPlay()
    {
        // Người chơi mới: register cái đã rồi chơi
        if (PlayerPrefs.HasKey(UserDataKey.ACCOUNT_KEY))
        {
            // SceneManager.LoadScene(SceneKey.MainMenu);
            if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Play_Sound);
            SceneKey.targetScene = SceneKey.MainMenu;
            SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
        }
        else
        {
            SignUp_Popup.SetActive(true);
        }
    }
    public void CloseLoginPopup()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        LogIn_Popup.SetActive(false);
    }
    public void CloseSignupPopup()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        SignUp_Popup.SetActive(false);
    }
}
