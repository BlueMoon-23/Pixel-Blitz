using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    // Script này dùng để gán lên mấy cục UI mapinfo trong phần available maps
    // Trường dữ liệu
    public MapData mapData;
    // Trường hiển thị (UI)
    public Image mapImage;
    public TextMeshProUGUI mapStarRate;
    public TextMeshProUGUI mapDifficulty;
    public GameObject UnlockFill;
    public TextMeshProUGUI mapReq;
    void Start()
    {
        mapImage.sprite = mapData.mapInformation.MapImage;
        mapStarRate.text = mapData.mapInformation.StarRate.ToString();
        mapDifficulty.text = mapData.gamemode.name;
        mapDifficulty.color = mapData.gamemode.getColor();
        if (AccountSaveManager.CurrentAccount != null)
        {
            if (AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters.Count >= mapData.CharacterRequirement())
            {
                UnlockFill.SetActive(false);
                mapReq.gameObject.SetActive(false);
            }
            else
            {
                mapReq.text = "Require " + mapData.CharacterRequirement().ToString() + " characters in your inventory to unlock!";
            }
        }
    }
    public void ChooseFromMapIndex()
    {
        if (MapChoose.instance != null)
        {
            MapChoose.instance.CompareMapData(mapData);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
