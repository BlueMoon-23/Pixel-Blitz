using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Authenticator : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField username_Login;
    public TMP_InputField password_Login;
    public TMP_InputField username_Signup;
    public TMP_InputField password_Signup;
    public GameObject Text_Wrong;
    private UserData newUser;
    void Start()
    {
        Text_Wrong.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SignUp()
    {
        newUser = new UserData(); // khi tạo user mới thì cho nó bộ nhớ
        // Kiểm tra trống
        if (string.IsNullOrWhiteSpace(username_Signup.text))
        {
            username_Signup.text = "Can't sign up with an empty name!";
            return;
        }
        if (string.IsNullOrWhiteSpace(password_Signup.text))
        {
            password_Signup.text = "Can't sign up with an empty password!";
            return;
        }
        newUser.Username = username_Signup.text;
        newUser.Password = password_Signup.text;
        // Kiểm tra trùng lặp
        for (int i = 0; i < AccountSaveManager.instance.UserAccounts.userDatas.Count; i++)
        {
            if (newUser.Username == AccountSaveManager.instance.UserAccounts.userDatas[i].Username)
            {
                username_Signup.text = "This username has already existed!";
                return; // Kết thúc hàm, nếu không thì phần dưới sẽ được thực thi
            }
        }
        // lưu vào save manager: lưu vào cái mảng trước rồi lưu vào json
        AccountSaveManager.instance.UserAccounts.userDatas.Add(newUser);
        AccountSaveManager.instance.SaveAccounts();
        // Trực tiếp bước vào main menu với acc mới đăng nhập luôn
        AccountSaveManager.CurrentAccount = newUser;
        // Lưu vào playerprefs để lần sau khỏi phải đăng nhập lại
        PlayerPrefs.SetString(UserDataKey.USERNAME_KEY, username_Signup.text);
        SceneManager.LoadScene(SceneKey.MainMenu);
    }
    public void LogIn()
    {
        for (int i = 0; i < AccountSaveManager.instance.UserAccounts.userDatas.Count; i++)
        {
            if (username_Login.text == AccountSaveManager.instance.UserAccounts.userDatas[i].Username && password_Login.text == AccountSaveManager.instance.UserAccounts.userDatas[i].Password)
            {
                AccountSaveManager.CurrentAccount = AccountSaveManager.instance.UserAccounts.userDatas[i]; // không tạo checkuser rồi compare nhé
                // Lưu vào playerprefs để lần sau khỏi phải đăng nhập lại
                PlayerPrefs.SetString(UserDataKey.USERNAME_KEY, username_Login.text);
                SceneManager.LoadScene(SceneKey.MainMenu);
                return;
            }
        }
        // Khi nhập sai tên / pass
        username_Login.text = "";
        password_Login.text = "";
        Text_Wrong.SetActive(true);
    }
}
