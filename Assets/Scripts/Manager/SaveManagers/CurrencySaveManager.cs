using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySaveManager : MonoBehaviour
{
    public static CurrencySaveManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddGem(int amount)
    {
        if (AccountSaveManager.CurrentAccount != null)
        {
            AccountSaveManager.CurrentAccount.CurrencyData.UserGems += amount;
        }
        if (AccountSaveManager.instance != null)
        {
            AccountSaveManager.instance.SaveAccounts();
        }
    }
    public void AddDiamonds(int amount)
    {
        if (AccountSaveManager.CurrentAccount != null)
        {
            AccountSaveManager.CurrentAccount.CurrencyData.UserDiamonds += amount;
        }
        if (AccountSaveManager.instance != null)
        {
            AccountSaveManager.instance.SaveAccounts();
        }
    }
    public void RemoveGem(int amount)
    {
        if (AccountSaveManager.CurrentAccount != null)
        {
            AccountSaveManager.CurrentAccount.CurrencyData.UserGems -= amount;
        }
        if (AccountSaveManager.instance != null)
        {
            AccountSaveManager.instance.SaveAccounts();
        }
    }
    public void RemoveDiamonds(int amount)
    {
        if (AccountSaveManager.CurrentAccount != null)
        {
            AccountSaveManager.CurrentAccount.CurrencyData.UserDiamonds -= amount;
        }
        if (AccountSaveManager.instance != null)
        {
            AccountSaveManager.instance.SaveAccounts();
        }
    }
}
