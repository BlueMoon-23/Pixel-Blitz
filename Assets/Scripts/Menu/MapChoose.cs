using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapChoose : MonoBehaviour
{
    // Danh sách map
    [SerializeField] private List<MapData> Maps = new List<MapData>(); // Mảng 2 chiều kiểu C#
    public MapInformation[] mapInformation;
    public Gamemodes[] gamemodes;
    // Thông tin map được hiện trên cửa sổ
    [SerializeField] private int currentMapDataIndex;
    public Image MapImage;
    public TextMeshProUGUI MapName;
    public TextMeshProUGUI MapStarRate;
    public TextMeshProUGUI Gamemode;
    // Map Index
    public GameObject AvailableMapInfo;
    public GameObject InventoryInfo;
    // Singleton để truyền dữ liệu vào game scene ở hàm awake
    public static MapChoose instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        // Xóa object thừa ở mode manager: object gamemode cần để start coroutine
        if (ModeManager.instance != null)
        {
            ModeManager.instance.DestroyGamemodeObject();
        }
        // Tự động ghép mapinformation và gamemode tạo thành mapdata găm lên mảng 2 chiều Maps
        // i là chiều dọc (gamemode)
        // j là chiều ngang (map)
        // => Truy cập phần tử bằng công thức: Maps[i * gamemodes.Length + j]
        for (int i = 0; i < gamemodes.Length; i++)
        {
            for (int j = 0; j < mapInformation.Length; j++)
            {
                MapData mapData = new MapData();
                mapData.mapInformation = mapInformation[i];
                mapData.gamemode = gamemodes[j];
                Maps.Add(mapData);
            }
        }
        for (int i = 0; i < Maps.Count - 1; i++)
        {
            for (int j = i; j < Maps.Count; j++)
            {
                if (Maps[i].Difficulty() > Maps[j].Difficulty())
                {
                    MapData temp_Map = Maps[i];
                    Maps[i] = Maps[j];
                    Maps[j] = temp_Map;
                }
            }
        }
    }
    void Start()
    {
        ShowMapUI(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        // Tạo các cục prefab làm con của cục characterequip.instance.gameobject tương ứng với characcterequip.instance.characterloadout
        if (CharacterLoadout.instance != null)
        {
            CharacterLoadout.instance.Set_CharacterLoadout_Prefab();
        }
        // Xác nhận currentMapData
        MapData ChosenMap = Maps[currentMapDataIndex];
        // Cài data xuống mode manager: currentGamemode, Star (gọi hàm play của modemanager cũng được
        if (ModeManager.instance != null) {
            ModeManager.instance.Play(ChosenMap);
        }
        switch(ChosenMap.mapInformation.name)
        {
            case "OmittedCastle":
                {
                    SceneManager.LoadScene(SceneKey.OmittedCastle);
                    break;
                }
            case "DeadShaft":
                {
                    SceneManager.LoadScene(SceneKey.DeadShaft);
                    break;
                }
            default:
                {
                    SceneManager.LoadScene(SceneKey.Greenland);
                    break; ;
                }
        }
    }
    public void PreviousMap()
    {
        if (currentMapDataIndex > 0)
        {
            currentMapDataIndex--;
        }
        else
        {
            currentMapDataIndex = Maps.Count - 1;
        }
        ShowMapUI(currentMapDataIndex);
    }
    public void NextMap()
    {
        if (currentMapDataIndex < Maps.Count - 1)
        {
            currentMapDataIndex++;
        }
        else
        {
            currentMapDataIndex = 0;
        }
        ShowMapUI(currentMapDataIndex);
    }
    public void ShowMapUI(int index)
    {
        MapImage.sprite = Maps[index].mapInformation.MapImage;
        MapName.text = Maps[index].mapInformation.MapName;
        MapStarRate.text = Maps[index].mapInformation.StarRate.ToString();
        Gamemode.text = Maps[index].gamemode.name;
        switch(Maps[index].gamemode.name)
        {
            case "Medium":
                {
                    Gamemode.color = new Color32(255, 187, 73, 255);
                    break;
                }
            case "Hard":
                {
                    Gamemode.color = new Color32(255, 100, 76, 255);
                    break;
                }
            default:
                {
                    Gamemode.color = new Color32(165, 255, 107, 255);
                    break;
                }
        }
        if (AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters.Count < Maps[index].CharacterRequirement())
        {
            NextMap();
        }
    }
    public void Exit()
    {
        SceneManager.LoadScene(SceneKey.MainMenu);
    }
    public void ShowAvailableMaps()
    {
        AvailableMapInfo.SetActive(true);
        InventoryInfo.SetActive(false);
    }
    public void StopShowAvailableMaps()
    {
        AvailableMapInfo.SetActive(false);
        InventoryInfo.SetActive(true);
    }
}
