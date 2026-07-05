using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;
    // Mode manager
    public GameObject[] GamemodePrefabs;
    public Gamemodes currentGamemode; // Khi người chơi nhấn nút play, phải truyền cái currentGamemode vô
    public GameObject[] MapPrefabs;
    public MapInformation currentMap;
    public int MaxWave;
    public float Star;
    // Truyền nhạc xuống đây
    private AudioClip _currentMapBGM;
    public AudioClip currentMapBGM
    {
        get { return _currentMapBGM; }
    }
    // Current Enemy Prefabs
    public List<BaseEnemy> enemy_Prefabs = new List<BaseEnemy>();
    private Dictionary<string, GameObject> Gamemode_Dict = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> Map_Dict = new Dictionary<string, GameObject>();
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
        foreach (GameObject gamemode in GamemodePrefabs)
        {
            Gamemode_Dict.Add(gamemode.name, gamemode);
        }
        foreach (GameObject map in MapPrefabs)
        {
            Map_Dict.Add(map.name, map);
        }
        transform.position = Vector3.zero;
    }
    public void LoadGamemode()
    {
        MaxWave = currentGamemode.getMaxWave();
        currentGamemode = this.gameObject.GetComponentInChildren<Gamemodes>();
        // Không có dòng này sẽ báo lỗi coroutine không chạy vì "easy is inactive"
        // Lý do: coroutine gọi từ modemanager.instance.currentGamemode, nhưng currentGamemode lại trỏ về cái script trong prefab
        // Nên coroutine không công nhận nó (prefab = inactive)
        // Giải pháp: phải tạo ra cục gamemode object thật rồi găm lên currentGamemode

        // Enemy pooler: mode manager phải làm danh sách prefab enemy để enemy pooler làm việc
        if (currentGamemode != null)
        {
            for (int i = 0; i < currentGamemode.enemyEntries.Count; i++)
            {
                enemy_Prefabs.Add(currentGamemode.enemyEntries[i].Enemy_Prefab);
            }
        }
    }
    public void Play(MapData mapData)
    {
        currentGamemode = mapData.gamemode;
        GameObject gamemode_object = Instantiate(Gamemode_Dict[currentGamemode.GetType().ToString()], transform.position, Quaternion.identity);
        GameObject map_object = Instantiate(Map_Dict[mapData.mapInformation.MapName.Replace(" ", "")], transform.position, Quaternion.identity);
        gamemode_object.transform.SetParent(transform, false);
        currentMap = map_object.GetComponent<MapInformation>();
        map_object.transform.SetParent(transform, false);
        Star = mapData.mapInformation.StarRate;
        _currentMapBGM = mapData.mapInformation.MapBGM;
    }
    public void DestroyGamemodeObject()
    {
        foreach (Transform child in this.transform)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void ClearEnemyPrefab()
    {
        if (enemy_Prefabs.Count > 0)
        {
            enemy_Prefabs.Clear();
        }
    }
}
