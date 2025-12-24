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
            SceneManager.LoadScene(SceneKey.MainMenu);
        }
        else
        {
            SignUp_Popup.SetActive(true);
        }
    }
    public void CloseLoginPopup()
    {
        LogIn_Popup.SetActive(false);
    }
    public void CloseSignupPopup()
    {
        SignUp_Popup.SetActive(false);
    }
}
