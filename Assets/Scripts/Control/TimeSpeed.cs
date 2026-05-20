using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TimeSpeed : MonoBehaviour
{
    public static TimeSpeed instance;
    public float[] AvailableSpeeds; // 0.5 - 1 - 1.5 - 2
    private int CurrentIndex = 1;
    public TextMeshProUGUI SpeedText;
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
    }
    void Start()
    {
        Time.timeScale = 1f;
        SpeedText.text = "1x";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float CurrentSpeed()
    {
        return AvailableSpeeds[CurrentIndex];
    }
    public void SpeedUp()
    {
        CurrentIndex++;
        if (CurrentIndex >= AvailableSpeeds.Length) CurrentIndex = 0;
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.SpeedUp_Sound);
        if (GameSetting.instance != null && !GameSetting.instance.isOn) Time.timeScale = AvailableSpeeds[CurrentIndex];
        SpeedText.text = AvailableSpeeds[CurrentIndex].ToString() + "x";
    }
}
