using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfomation : MonoBehaviour
{
    public CharacterData characterData;
    // Người chơi sở hữu hay chưa
    private bool _hasOwned = false;
    public bool hasOwned
    {
        get { return _hasOwned; }
        set { _hasOwned = value; }
    }
    private void Start()
    {
        if (AccountSaveManager.CurrentAccount != null)
        {
            for (int i = 0; i < AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters.Count; i++)
            {
                if (this.characterData.CharacterName == AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters[i].CharacterName)
                {
                    hasOwned = true;
                    break;
                }
            }
        }
        if (!hasOwned)
        {
            foreach (Transform child in this.transform) // vòng lặp duyệt qua các phần tử con trong phần tử cha trên hierarchy
            {
                child.gameObject.SetActive(false);
            }
            Button thisButton = GetComponent<Button>();
            if (thisButton != null)
            {
                thisButton.enabled = false;
            }
        }
        else
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }
            Button thisButton = GetComponent<Button>();
            if (thisButton != null)
            {
                thisButton.enabled = true;
            }
        }
    }
}
