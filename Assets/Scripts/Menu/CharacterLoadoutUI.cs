using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLoadoutUI : MonoBehaviour
{
    // Script gắn lên 4 hộp CharacterLoadout trong MapChoose, sử dụng để tự cập nhật giá
    public TextMeshProUGUI CharacterLoadoutCost;
    public Image CharacterLoadoutImage;
    public CharacterInfomation characterInfo;
    public void Setup(CharacterInfomation info)
    {
        characterInfo = info;
        RefreshUI();
    }
    private void OnEnable()
    {
        RefreshUI();
    }
    public void RefreshUI()
    {
        if (characterInfo != null)
        {
            CharacterLoadoutImage.gameObject.SetActive(true);
            CharacterLoadoutCost.gameObject.SetActive(true);
            CharacterLoadoutImage.sprite = characterInfo.characterData.characterProfile.CharacterImage;
            CharacterLoadoutCost.text = "$" + WeaponCalculator.CalculateCost(characterInfo.characterData.characterProfile.CostStat, characterInfo.characterData.WeaponEquippedData);
        }
    }
    public void ResetToNull()
    {
        characterInfo = null;
        CharacterLoadoutImage.sprite = null;
        CharacterLoadoutCost.text = null;
    }
    public void SelfSetup()
    {
        Setup(characterInfo);
    }
    public void ChooseCharacter()
    {
        CharacterEquip.instance.ChooseCharacter(characterInfo);
    }
}
