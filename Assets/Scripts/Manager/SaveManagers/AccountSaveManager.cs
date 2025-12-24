using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountSaveManager : MonoBehaviour
{
    public static AccountSaveManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAccounts();
            // LoadAccounts có 3 công dụng
            // 1: lấy lại data người chơi từ json
            // 2: xác định danh tính người chơi
            // 3: tránh ghi đè dữ liệu
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Lưu data account
    public UserList UserAccounts;
    public static UserData CurrentAccount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveAccounts()
    {
        string jsonData = JsonUtility.ToJson(UserAccounts);
        PlayerPrefs.SetString(UserDataKey.ACCOUNT_KEY, jsonData);
        PlayerPrefs.Save();
    }
    public void LoadAccounts()
    {
        if (PlayerPrefs.HasKey(UserDataKey.ACCOUNT_KEY))
        {
            string jsonData = PlayerPrefs.GetString(UserDataKey.ACCOUNT_KEY);
            UserAccounts = JsonUtility.FromJson<UserList>(jsonData);
        }
        else
        {
            UserAccounts = new UserList();
        }
        // Cài current account = account đã vào trước đó
        if (PlayerPrefs.HasKey(UserDataKey.USERNAME_KEY))
        {
            for (int i = 0; i < UserAccounts.userDatas.Count; i++)
            {
                if (UserAccounts.userDatas[i].Username == PlayerPrefs.GetString(UserDataKey.USERNAME_KEY))
                {
                    CurrentAccount = UserAccounts.userDatas[i];
                    break;
                }
            }
        }
    }
}
