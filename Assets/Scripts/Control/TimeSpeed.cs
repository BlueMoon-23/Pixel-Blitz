using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeSpeed : MonoBehaviour
{
    public float[] AvailableSpeeds; // 0.5 - 1 - 1.5 - 2
    private int CurrentIndex = 1;
    public TextMeshProUGUI SpeedText;
    void Start()
    {
        Time.timeScale = 1f;
        SpeedText.text = "1x";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpeedUp()
    {
        CurrentIndex++;
        if (CurrentIndex >= AvailableSpeeds.Length) CurrentIndex = 0;
        Time.timeScale = AvailableSpeeds[CurrentIndex];
        SpeedText.text = AvailableSpeeds[CurrentIndex].ToString() + "x";
    }
}
