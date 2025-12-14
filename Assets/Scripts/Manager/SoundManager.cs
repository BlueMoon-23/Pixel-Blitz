using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource audioSource;
    // Cac sound
    public AudioClip Upgrade_Sound;
    public AudioClip Place_Sound;
    public AudioClip Sell_Sound;
    public AudioClip EarnCoin_Sound;
    public AudioClip BaseGetHit_Sound;
    public AudioClip Skip_Sound;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

