using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserRegister : MonoBehaviour
{
    public static UserRegister instance;
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
    public void OpenLoginPopup()
    {
        LogIn_Popup.SetActive(true);
    }
    public void OpenSignupPopup()
    {
        SignUp_Popup.SetActive(true);
    }
    public void CloseLoginPopup()
    {
        LogIn_Popup.SetActive(false);
    }
    public void CloseSignupPopup()
    {
        SignUp_Popup.SetActive(false);
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
