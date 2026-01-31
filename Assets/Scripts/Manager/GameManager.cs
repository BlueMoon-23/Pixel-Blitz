using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        ModeManager.instance.LoadGamemode();
    }
    void Start()
    {
        StartCoroutine(StartGame());
    }
    private IEnumerator StartGame()
    {
        if (Ready_Skip.instance != null) yield return StartCoroutine(Ready_Skip.instance.GetReady());
        if (WaveManager.instance != null) yield return StartCoroutine(WaveManager.instance.SpawnEnemyWave(ModeManager.instance.currentGamemode, ModeManager.instance.MaxWave));
    }
    void Update()
    {
        // Defeat luon neu base health <= 0
        if (BaseHealth.instance.currentBaseHealth <= 0 && !Win_Lose.instance.Defeated)
        {
            Win_Lose.instance.Defeated = true;
            StopAllCoroutines();
            Win_Lose.instance.Defeat(); // win_lose
        }
    }
}
