using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotGroupControll : MonoBehaviour
{
    // Script này sẽ liên kết với singleton CharacterLoadout để lấy được prefab và truyền xuống cho các character slot
    public DragCharacter[] CharacterSlots = new DragCharacter[4];
    public Image[] CharacterImages = new Image[4];
    public TextMeshProUGUI[] CharacterCosts = new TextMeshProUGUI[4];
    private void Awake()
    {
        int index = 0;
        if (CharacterLoadout.instance != null)
        {
            foreach (Transform child in CharacterLoadout.instance.transform)
            {
                CharacterData cData = child.GetComponent<CharacterInfomation>().characterData;
                CharacterSlots[index].gameObject.SetActive(true);
                CharacterSlots[index].SetCharacterPrefab(child.gameObject);
                CharacterImages[index].sprite = cData.characterProfile.CharacterImage;
                CharacterCosts[index].gameObject.SetActive(true);
                CharacterCosts[index].text = "$" + WeaponCalculator.CalculateCost(cData.characterProfile.CostStat, cData.WeaponEquippedData).ToString();
                index++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
