using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiftcodeSaveManager : MonoBehaviour
{
    public static GiftcodeSaveManager instance;
    public TMP_InputField inputField;
    public TextMeshProUGUI announcement;
    public List<Giftcode> giftcodes;
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
    public IEnumerator ResetAnnouncement()
    {
        yield return new WaitForSeconds(2);
        announcement.text = "";
    }
    public void RedeemCode()
    {
        bool doExist = false;
        foreach (var giftcode in giftcodes)
        {
            if (inputField.text == giftcode.getCode())
            {
                if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.EarnCoin_Sound);
                doExist = true;
                giftcode.Redeem();
                break;
            }
        }
        if (!doExist)
        {
            announcement.color = new Color32(255, 57, 57, 200);
            announcement.text = "Invalid code.";
            StartCoroutine(ResetAnnouncement());
        }
        if (AccountSaveManager.instance != null)
        {
            AccountSaveManager.instance.SaveAccounts();
        }
        if (MainMenu.instance != null)
        {
            MainMenu.instance.ReloadAccount();
        }
    }
}
