using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Giftcode
{
    [SerializeField] private string Code;
    [SerializeField] private int Gem_Reward;
    [SerializeField] private int Diamond_Reward;
    public Giftcode(string code = "", int gem = 0, int diamond = 0)
    {
        Code = code;
        Gem_Reward = gem;
        Diamond_Reward = diamond;
    }
    public string getCode()
    {
        return Code;
    }
    private bool hasRedeemed()
    {
        if (AccountSaveManager.instance != null)
        {
            foreach (var redeemedCode in AccountSaveManager.CurrentAccount.redeemedCodes)
            {
                if (redeemedCode.Code == Code) return redeemedCode.hasRedeemed;
            }
        }
        return false;
    }
    public void Redeem()
    {
        if (!hasRedeemed())
        {
            AccountSaveManager.CurrentAccount.redeemedCodes.Add(new UserCodeData(Code));
            if (CurrencySaveManager.instance != null)
            {
                CurrencySaveManager.instance.AddGem(Gem_Reward);
                CurrencySaveManager.instance.AddDiamonds(Diamond_Reward);
            }
            if (GiftcodeSaveManager.instance != null)
            {
                GiftcodeSaveManager.instance.announcement.color = new Color32(57, 255, 57, 200);
                GiftcodeSaveManager.instance.announcement.text = "Earned " + Gem_Reward + " gems";
                if (Diamond_Reward > 0)
                {
                    GiftcodeSaveManager.instance.announcement.text += " and " + Diamond_Reward + " diamonds!";
                }
                else
                {
                    GiftcodeSaveManager.instance.announcement.text += "!";
                }
                GiftcodeSaveManager.instance.StartCoroutine(GiftcodeSaveManager.instance.ResetAnnouncement());
            }
        }
        else
        {
            if (GiftcodeSaveManager.instance != null)
            {
                GiftcodeSaveManager.instance.announcement.color = new Color32(255, 57, 57, 200);
                GiftcodeSaveManager.instance.announcement.text = "Already redeemed.";
                GiftcodeSaveManager.instance.StartCoroutine(GiftcodeSaveManager.instance.ResetAnnouncement());
            }
        }
    }
}
