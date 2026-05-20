using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    public bool isOn;
    // Setting Popup
    public GameObject SettingPopUp;
    public static GameSetting instance;
    [Header("Sliders")]
    public Slider MusicSlider;
    public Slider SoundEffectSlider;
    public Slider UISoundSlider;
    public Slider FPSSlider;
    [Header("Toggles")]
    public ToggleGroup AutoSkip;
    public Toggle DoAutoSkip;
    public Toggle DontAutoskip;
    public bool _autoSkip;
    public ToggleGroup ShakeEffect;
    public Toggle DoShakeEffect;
    public Toggle DontShakeEffect;
    public bool _shakeEffect;
    public ToggleGroup ShowExplosion;
    public Toggle DoShowExplosion;
    public Toggle DontShowExplosion;
    public bool _showExplosion;
    public ToggleGroup ShowMuzzle;
    public Toggle DoShowMuzzle;
    public Toggle DontShowMuzzle;
    public bool _showMuzzle;
    
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
        isOn = false;
        // Add lệnh chỉnh một cách tự động
        if (SoundManager.Instance != null)
        {
            MusicSlider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
            SoundEffectSlider.onValueChanged.AddListener(SoundManager.Instance.SetSoundEffectsVolume);
            UISoundSlider.onValueChanged.AddListener(SoundManager.Instance.SetUISoundsVolume);
        }
        // Sử dụng PlayerPrefs để load lại thông tin về volume hiện tại của người chơi. Có key nào thêm key đó
        if (PlayerPrefs.HasKey(UserDataKey.MUSICVOLUME)) { MusicSlider.value = PlayerPrefs.GetFloat(UserDataKey.MUSICVOLUME); }
        else { MusicSlider.value = 1f; }
        if (PlayerPrefs.HasKey(UserDataKey.UISOUNDSVOLUME)) { UISoundSlider.value = PlayerPrefs.GetFloat(UserDataKey.UISOUNDSVOLUME); }
        else { UISoundSlider.value = 1f; }
        if (PlayerPrefs.HasKey(UserDataKey.SOUNDEFFECTSVOLUME)) { SoundEffectSlider.value = PlayerPrefs.GetFloat(UserDataKey.SOUNDEFFECTSVOLUME); }
        else { SoundEffectSlider.value = 1f; }
        if (PlayerPrefs.HasKey(UserDataKey.FPS)) { FPSSlider.value = PlayerPrefs.GetFloat(UserDataKey.FPS); }
        else { FPSSlider.value = 0f; }
        ApplyFPS(FPSSlider.value);
        // khi lưu, cài 1 = true và 0 = false. GetInt("key", defaultvalue) tương đương với kiểm tra key, không có thì = defaultvalue
        _autoSkip = PlayerPrefs.GetInt(UserDataKey.AUTOSKIP, 0) == 1;
        _shakeEffect = PlayerPrefs.GetInt(UserDataKey.SHAKEEFFECT, 0) == 1;
        _showExplosion = PlayerPrefs.GetInt(UserDataKey.SHOWEXPLOSION, 1) == 1;
        _showMuzzle = PlayerPrefs.GetInt(UserDataKey.SHOWMUZZLE, 1) == 1;
        DoAutoSkip.isOn = _autoSkip;
        DontAutoskip.isOn = !_autoSkip;
        DoShakeEffect.isOn = _shakeEffect;
        DontShakeEffect.isOn = !_shakeEffect;
        DoShowExplosion.isOn = _showExplosion;
        DontShowExplosion.isOn = !_showExplosion;
        DoShowMuzzle.isOn = _showMuzzle;
        DontShowMuzzle.isOn = !_showMuzzle;
    }
    public void Setting()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Setting_Sound);
        SettingPopUp.SetActive(true);
        isOn = true;
        Time.timeScale = 0f;
    }
    public void CloseSetting()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Setting_Sound);
        isOn = false;
        SettingPopUp.SetActive(false);
        if (TimeSpeed.instance != null) Time.timeScale = TimeSpeed.instance.CurrentSpeed();
        else Time.timeScale = 1f;
    }
    public void Surrender()
    {
        CloseSetting();
        BaseHealth.instance.currentBaseHealth = 0;
    }
    public void OnAutoSkipToggleChanged(bool isOn)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Skip_Sound);
        // Kiểm tra toggle nào đang bật
        Toggle activeToggle = AutoSkip.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            if (activeToggle == DoAutoSkip)
            {
                _autoSkip = true;
            }
            else
            {
                _autoSkip = false;
            }
            PlayerPrefs.SetInt(UserDataKey.AUTOSKIP, (_autoSkip == true) ? 1 : 0);
        }
    }
    public void OnShakeEffectToggleChanged(bool isOn)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Skip_Sound);
        // Kiểm tra toggle nào đang bật
        Toggle activeToggle = ShakeEffect.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            if (activeToggle == DoShakeEffect)
            {
                _shakeEffect = true;
            }
            else
            {
                _shakeEffect = false;
            }
            PlayerPrefs.SetInt(UserDataKey.SHAKEEFFECT, (_shakeEffect == true) ? 1 : 0);
        }
    }
    public void OnShowExplosionToggleChanged(bool isOn)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Skip_Sound);
        // Kiểm tra toggle nào đang bật
        Toggle activeToggle = ShowExplosion.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            if (activeToggle == DoShowExplosion)
            {
                _showExplosion = true;
            }
            else
            {
                _showExplosion = false;
            }
            PlayerPrefs.SetInt(UserDataKey.SHOWEXPLOSION, (_showExplosion == true) ? 1 : 0);
        }
    }
    public void OnShowMuzzleToggleChanged(bool isOn)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Skip_Sound);
        // Kiểm tra toggle nào đang bật
        Toggle activeToggle = ShowMuzzle.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            if (activeToggle == DoShowMuzzle)
            {
                _showMuzzle = true;
            }
            else
            {
                _showMuzzle = false;
            }
            PlayerPrefs.SetInt(UserDataKey.SHOWMUZZLE, (_showMuzzle == true) ? 1 : 0);
        }
    }
    public void OnFPSSliderValueChanged(float value)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Skip_Sound);
        ApplyFPS(value);
        PlayerPrefs.SetFloat(UserDataKey.FPS, value);
        PlayerPrefs.Save();
    }
    private void ApplyFPS(float sliderValue)
    {
        int fps = 30 + ((int)sliderValue * 15);
        Application.targetFrameRate = fps;
        QualitySettings.vSyncCount = 0; // Luôn tắt VSync khi dùng targetFrameRate
        Debug.Log($"FPS Applied: {fps}");
    }
}
