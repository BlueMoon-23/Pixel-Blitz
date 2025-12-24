using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;
    // Mode manager
    public GameObject[] Gamemode_Prefabs;
    public Gamemodes currentGamemode; // Khi người chơi nhấn nút play, phải truyền cái currentGamemode vô
    public int MaxWave;
    public float Star;
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
    public void LoadGamemode()
    {
        if (currentGamemode != null)
        {
            if (currentGamemode.GetType() == typeof(Easy))
            {
                MaxWave = 25;
            }
            if (currentGamemode.GetType() == typeof(Medium))
            {
                MaxWave = 25;
            }
            if (currentGamemode.GetType() == typeof(Hard))
            {
                MaxWave = 25;
            }
        }
        currentGamemode = this.gameObject.GetComponentInChildren<Gamemodes>();
        // Không có dòng này sẽ báo lỗi coroutine không chạy vì "easy is inactive"
        // Lý do: coroutine gọi từ modemanager.instance.currentGamemode, nhưng currentGamemode lại trỏ về cái script trong prefab
        // Nên coroutine không công nhận nó (prefab = inactive)
        // Giải pháp: phải tạo ra cục gamemode object thật rồi găm lên currentGamemode
    }
    public void Play(MapData mapData)
    {
        currentGamemode = mapData.gamemode;
        int index = 0;
        if (currentGamemode.GetType() == typeof(Easy))
        {
            index = 0;
        }
        else if (currentGamemode.GetType() == typeof(Medium))
        {
            index = 1;
        }
        else if (currentGamemode.GetType() == typeof(Hard))
        {
            index = 2;
        }
        GameObject gamemode_object = Instantiate(Gamemode_Prefabs[index], transform.position, Quaternion.identity);
        gamemode_object.transform.SetParent(transform, false);
        Star = mapData.mapInformation.StarRate;
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
}
