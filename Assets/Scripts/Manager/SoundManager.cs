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
    [Header("Music / OST")]
    public AudioClip MenuMusic;
    public AudioClip BossThemeMusic;
    public AudioClip GreenlandMusic;
    public AudioClip OmittedCastleMusic;
    public AudioClip DeadShaftMusic;
    [Header("UI Sounds")]
    public AudioClip Upgrade_Sound;
    public AudioClip Place_Sound;
    public AudioClip Sell_Sound;
    public AudioClip EarnCoin_Sound;
    public AudioClip BaseGetHit_Sound;
    public AudioClip Skip_Sound;
    public AudioClip Victory_Sound; // pianoairy
    public AudioClip Defeat_Sound; // airystring
    public AudioClip OpenButton_Sound; // ui click open scifi, nhớ kiếm lại đó vì không thích cái này
    public AudioClip Play_Sound; // ui click open scifi, nhớ kiếm lại đó vì không thích cái này
    public AudioClip SpeedUp_Sound; 
    public AudioClip BuyCharacter_Sound; // Success_Point_big
    public AudioClip MoveButton_Sound; // swipe_screen_3
    public AudioClip NearEndWave_Sound; // countdown_cute_loop
    public AudioClip ClickOnCharacter_Sound; // UI_Screen_Swoosh_3, áp dụng cho select character ngoài map và ấn vào character
    public AudioClip CloseButton_Sound; // Click_Close_scifi, dùng cho close button và exit
    public AudioClip ChooseMap_Sound; // UI_Screen_Swoosh_4
    public AudioClip Setting_Sound; // UI_Click_Select_2
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
    public AudioClip PulserLaserStart;
    public AudioClip PulserLaserEnd;
    public AudioClip WizardFireballExplosion;
    public AudioClip WizardStarSequence;
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
    /// <summary>
    /// Truyền audioclip vào đây để chơi nhạc. Truyền null nếu muốn tắt nhạc
    /// </summary>
    /// <param name="nextClip"></param>
    public void PlayBGM(AudioClip nextClip)
    {
        if (nextClip == null)
        {
            if (MusicSource.isPlaying) MusicSource.Stop();
            MusicSource.clip = null;
            return;
        }
        if (MusicSource.clip == nextClip && MusicSource.isPlaying)
        {
            // Nếu trùng và nhạc đang phát, giữ nguyên
            return;
        }
        // Nếu là một bài nhạc hoàn toàn khác, đổi nhạc
        MusicSource.clip = nextClip;
        MusicSource.loop = true;
        MusicSource.Play();
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

