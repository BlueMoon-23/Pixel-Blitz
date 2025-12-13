using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterUIControll : MonoBehaviour
{
    public static CharacterUIControll instance;
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
    private GameObject[] Range_Prefab;
    public Sprite[] characterImages;
    //
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI Info1;
    public TextMeshProUGUI Info2;
    public TextMeshProUGUI Info3;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI sellCost;
    public TextMeshProUGUI RangeStats;
    public TextMeshProUGUI DamageStats;
    public TextMeshProUGUI CooldownStats;
    //
    public BaseCharacter CurrentCharacter;
    public CanvasGroup HiddenDetectionIcon;
    public CanvasGroup StrikethroughIcon;
    public void UI_Off()
    {
        gameObject.SetActive(false);
        Range_Prefab = GameObject.FindGameObjectsWithTag("Range");
        for (int i = 0; i < Range_Prefab.Length; i++)
        {
            Range_Prefab[i].GetComponent<Renderer>().enabled = false;
        }
    }
    public void Upgrade()
    {
        if (EconomyManager.instance != null)
        {
            if (CurrentCharacter.GetUpgradeCost(CurrentCharacter.GetLevel()) <= EconomyManager.instance.PlayerCoin) 
            {
                CurrentCharacter.Upgrade();
                SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Upgrade_Sound);
                EconomyManager.instance.Purchase(CurrentCharacter.GetUpgradeCost(CurrentCharacter.GetLevel() - 1));
                EconomyManager.instance.Change_CurrentCoin();
            }
            else
            {
                EconomyManager.instance.Announce_CantUpgrade(CurrentCharacter.GetUpgradeCost(CurrentCharacter.GetLevel()));
            }
        }
        // Ngoai if
        UI_Off();
    }
    public void Sell()
    {
        if (EconomyManager.instance != null) 
        { 
            EconomyManager.instance.AddCoin(CurrentCharacter.GetSellCost()); 
            EconomyManager.instance.Change_CurrentCoin();
        }
        CharacterManager.instance.RemoveCharacter(CurrentCharacter);
        Destroy(CurrentCharacter.gameObject);
        gameObject.SetActive(false);
    }
}
