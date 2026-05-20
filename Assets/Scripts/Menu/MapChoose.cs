using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapChoose : MonoBehaviour
{
    // Danh sách map
    [SerializeField] private List<MapData> Maps = new List<MapData>(); // Mảng 2 chiều kiểu C#
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
        Maps.Sort((x, y) => x.Difficulty().CompareTo(y.Difficulty()));
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
        if (CharacterLoadout.instance.characterLoadout.Count == 0) 
        {
            StartCoroutine(ShowEquipAnnounce());
            return;
        }
        // Xác nhận currentMapData
        MapData ChosenMap = Maps[currentMapDataIndex];
        // Cài data xuống mode manager: currentGamemode, Star (gọi hàm play của modemanager cũng được
        if (ModeManager.instance != null) {
            ModeManager.instance.Play(ChosenMap);
        }
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Play_Sound);
        SceneKey.targetScene = ChosenMap.targetScene;
        Debug.Log(ChosenMap.targetScene);
        SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
    }
    private IEnumerator ShowEquipAnnounce()
    {
        if (CharacterEquip.instance != null)
        {
            CharacterEquip.instance.LoadoutGroup.SetActive(false);
            CharacterEquip.instance.EquipAnnounce.SetActive(true);
            if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
            yield return new WaitForSeconds(1f);
            CharacterEquip.instance.LoadoutGroup.SetActive(true);
            CharacterEquip.instance.EquipAnnounce.SetActive(false);
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
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.MoveButton_Sound);
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
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.MoveButton_Sound);
        ShowMapUI(currentMapDataIndex);
    }
    public void ShowMapUI(int index)
    {
        MapImage.sprite = Maps[index].mapInformation.MapImage;
        MapName.text = Maps[index].mapInformation.MapName;
        MapStarRate.text = Maps[index].mapInformation.StarRate.ToString();
        Gamemode.text = Maps[index].gamemode.name;
        Gamemode.color = Maps[index].gamemode.getColor();
        if (AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters.Count < Maps[index].CharacterRequirement())
        {
            NextMap();
        }
    }
    public void CompareMapData(MapData mapData)
    {
        int left = 0;
        int right = Maps.Count - 1;
        while (left <= right)
        {
            int i = (left + right) / 2;
            if (mapData.Difficulty() == Maps[i].Difficulty() && mapData.mapInformation.name == Maps[i].mapInformation.name && mapData.gamemode.name == Maps[i].gamemode.name)
            {
                currentMapDataIndex = i;
                break;
            }
            else if (mapData.Difficulty() < Maps[i].Difficulty()) right = i - 1;
            else left = i + 1;
        }
        ShowMapUI(currentMapDataIndex);
        StopShowAvailableMaps();
    }
    public void Exit()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.CloseButton_Sound);
        SceneManager.LoadScene(SceneKey.MainMenu);
    }
    public void ShowAvailableMaps()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.ChooseMap_Sound);
        AvailableMapInfo.SetActive(true);
        InventoryInfo.SetActive(false);
    }
    public void StopShowAvailableMaps()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.ChooseMap_Sound);
        AvailableMapInfo.SetActive(false);
        InventoryInfo.SetActive(true);
    }
}
