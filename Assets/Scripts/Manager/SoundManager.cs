using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    //public AudioSource audioSource; // này là sound tổng, gán phòng hờ thôi
    public AudioSource MusicSource; // tự gán audio source, mỗi cái 1 cái output khác nhau
    public AudioSource UISource;
    public AudioSource SoundEffectSource;
    public AudioMixer audioMixer; // thằng này bắt buộc phải găm vào chứ không dùng getcomponent được vì nó là assets

    // Cac sound
    // Nhóm Sound FX / UI Sounds
    [Header("UI Sounds")]
    public AudioClip Upgrade_Sound;
    public AudioClip Place_Sound;
    public AudioClip Sell_Sound;
    public AudioClip EarnCoin_Sound;
    public AudioClip BaseGetHit_Sound;
    public AudioClip Skip_Sound;
    public AudioClip Victory_Sound; // pianoairy
    public AudioClip Defeat_Sound; // airystring
    public AudioClip Play_Sound; // ui click open scifi, nhớ kiếm lại đó vì không thích cái này
    // Nhóm Sound Effects
    [Header("Enemy Sound Effects")]
    public AudioClip StompGround_Sound;
    public AudioClip SpiralStun_Sound; // shoot magic etfx
    public AudioClip ChargerSound;
    public AudioClip UndeadSummonSound; // EngineScifi_1_Start
    [Header("Character Explosion Bullet")]
    public AudioClip ArcherBulletExplosion;
    public AudioClip FreezerBulletExplosion; // ê cái này chỉ áp dụng cho level 4 của freezer thôi á
    public AudioClip MinigunnerBulletExplosion;
    public AudioClip RangerBulletExplosion;
    public AudioClip RocketeerBulletExplosion;
    public AudioClip SummonerBulletExplosion;
    public AudioClip WizardFireballExplosion;
    public AudioClip WizardStarSequence;
    public AudioClip WizardVortex; // chưa có đâu, mốt tìm sau
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
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMusicVolume(float value)
    {
        // value từ slider (0–1), chuyển sang dB
        audioMixer.SetFloat(UserDataKey.MUSICVOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(UserDataKey.MUSICVOLUME, value);
    }
    public void SetUISoundsVolume(float value)
    {
        audioMixer.SetFloat(UserDataKey.UISOUNDSVOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(UserDataKey.UISOUNDSVOLUME, value);
    }
    public void SetSoundEffectsVolume(float value)
    {
        audioMixer.SetFloat(UserDataKey.SOUNDEFFECTSVOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(UserDataKey.SOUNDEFFECTSVOLUME, value);
    }
}

