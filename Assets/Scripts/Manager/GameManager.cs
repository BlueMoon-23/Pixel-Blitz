using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Basic information => lôi ra từ modemanager
    // Wave Text
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI WaveText;
    private int currentWave;
    // Time text
    public TextMeshProUGUI currentTimeLeft;
    // Base Health Text
    private float BaseHealth;
    public TextMeshProUGUI BaseHealthText;
    public Image BaseHealthBar;
    public Image BaseHealthWhiteBar;
    // Skip UI
    private bool WantToSkip = false;
    public CanvasGroup SkipUI;
    // GetReady UI
    private bool isReady = false;
    public CanvasGroup ReadyUI;
    public GameObject WaypointArrows;
    // Victory UI
    public CanvasGroup TitleBar;
    public Image VictoryDimed;
    public CanvasGroup VictoryInfo;
    public CanvasGroup VictoryOptions;
    public TextMeshProUGUI Victory_TimePlayedText;
    public TextMeshProUGUI Victory_GemRewardText;
    public TextMeshProUGUI Victory_DiamondRewardText;
    // Defeat UI
    private bool Defeated = false;
    public CanvasGroup TitleBar1;
    public Image DefeatDimed;
    public CanvasGroup DefeatInfo;
    public CanvasGroup DefeatOptions;
    public TextMeshProUGUI Defeat_TimePlayedText;
    public TextMeshProUGUI Defeat_GemRewardText;
    public TextMeshProUGUI Defeat_DiamondRewardText;
    // Boss HP
    public GameObject BossHPGroup;
    public TextMeshProUGUI BossName;
    public TextMeshProUGUI BossHPText;
    public Image BossHPBar;
    // Setting Popup
    public GameObject SettingPopUp;
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
        BaseHealth = 100;
        StartCoroutine(StartGame());
    }
    private IEnumerator StartGame()
    {
        yield return StartCoroutine(GetReady());
        yield return StartCoroutine(SpawnEnemyWave(ModeManager.instance.currentGamemode, ModeManager.instance.MaxWave));
    }
    void Update()
    {
        // Defeat luon neu base health <= 0
        if (BaseHealth <= 0 && !Defeated)
        {
            Defeated = true;
            StopAllCoroutines();
            Defeat();
        }
    }
    IEnumerator SpawnEnemyWave(Gamemodes mode, int MaxWave)
    {
        ReadyUI.gameObject.SetActive(false);
        Coroutine coroutine = TimeManager.instance.StartCoroutine(TimeManager.instance.TimeCount());
        TimeManager.instance.SetCoroutine(coroutine);
        for (int i = 1; i <= MaxWave; i++)
        {
            if (!Defeated) // khong co if nay thi vong while bi break, wavetext bi lap lai nhieu lan
            {
                currentWave = i;
                WaveText.text = "Wave " + i + " is coming!";
                yield return new WaitForSeconds(1f);
                WantToSkip = false; // khong co lenh nay la neu skip roi thi wanttoskip = true, break vong while
                // Wave handle
                WaveText.text = "Base Health";
                if (EconomyManager.instance != null) EconomyManager.instance.EarnCoinEachWave(i);
                mode.StartCoroutine(mode.SpawnEnemyWave(i));
                if (i < MaxWave) { StartCoroutine(Skip()); }
                currentWaveText.text = i.ToString() + " / " + MaxWave.ToString();
                // Time handle
                int time = 60;
                if (i == MaxWave) { time = 300; }
                currentTimeLeft.text = (time / 60).ToString("D2") + " : " + (time % 60).ToString("D2");
                do
                {
                    time--;
                    currentTimeLeft.text = (time / 60).ToString("D2") + " : " + (time % 60).ToString("D2");
                    yield return new WaitForSeconds(1f);
                    // Skip luon neu khong co enemy
                    if (EnemyManager.instance != null && EnemyManager.instance.GetEnemyListCount() == 0)
                    {
                        DoSkip();
                    }
                }
                while (time > 0 && !WantToSkip && !Defeated);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield break;
            }
        }
        yield return new WaitForSeconds(5f);
        if (!Defeated) Victory();
        yield break;
    }
    IEnumerator Skip()
    {
        yield return new WaitForSeconds(14f);
        // Show Skip annouonce
        //DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            SkipUI.gameObject.SetActive(true);
            SkipUI.DOFade(1f, 0.5f).From(0f);
        });
        yield return new WaitForSeconds(43f);
        sequence.AppendCallback(() =>
        {
            SkipUI.DOFade(0f, 0.5f).From(1f);
        });
        sequence.AppendInterval(0.5f).AppendCallback(() => {
            SkipUI.gameObject.SetActive(false);
        });
    }
    public void DoSkip()
    {
        WantToSkip = true;
        SkipUI.gameObject.SetActive(false);
        if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Skip_Sound);
    }
    public void DontSkip()
    {
        WantToSkip = false;
        SkipUI.gameObject.SetActive(false);
        if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Skip_Sound);
    }
    public void Ready()
    {
        isReady = true;
    }
    public void Setting()
    {
        SettingPopUp.SetActive(true);
    }
    public void CloseSetting()
    {
        SettingPopUp.SetActive(false);
    }
    public void Surrender()
    {
        CloseSetting();
        BaseHealth = 0;
    }
    public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void Exit()
    {
        SceneManager.LoadScene(SceneKey.MapChoose);
    }
    public void BaseGetHit(float Damage)
    {
        BaseHealth -= Damage;
        if (BaseHealth < 0) BaseHealth = 0;
        if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.BaseGetHit_Sound);
        BaseHealthText.text = BaseHealth.ToString() + " / 100";
        //DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            BaseHealthWhiteBar.gameObject.SetActive(true);
            BaseHealthBar.transform.DOScaleX(BaseHealth / 100, 0.5f);
        });
        sequence.AppendInterval(0.25f).AppendCallback(() =>
        {
            BaseHealthWhiteBar.transform.DOScaleX(BaseHealth / 100, 0.125f);
        });
        sequence.AppendInterval(0.125f).AppendCallback(() => {
            BaseHealthWhiteBar.gameObject.SetActive(false);
        });
    }
    private IEnumerator GetReady()
    {
        //DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            ReadyUI.gameObject.SetActive(true);
            ReadyUI.DOFade(1f, 0.5f).From(0f);
        });
        do
        {
            Sequence sequence1 = DOTween.Sequence();
            sequence1.AppendCallback(() =>
            {
                WaypointArrows.gameObject.SetActive(true);
            });
            sequence1.AppendInterval(1f).AppendCallback(() =>
            {
                WaypointArrows.gameObject.SetActive(false);
            });
            yield return new WaitForSeconds(2f);
        }
        while (!isReady);
        yield break;
    }
    private void Victory()
    {
        // Get reward
        int gemreward = RewardCalculator.CalculateGem(currentWave, ModeManager.instance.Star, ModeManager.instance.currentGamemode, true);
        CurrencySaveManager.instance.AddGem(gemreward);
        int diamondreward = RewardCalculator.CalculateGem(currentWave, ModeManager.instance.Star, ModeManager.instance.currentGamemode, true);
        CurrencySaveManager.instance.AddDiamonds(diamondreward);
        if (SoundManager.Instance != null) { SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Victory_Sound); }
        // Show Score
        DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            TitleBar.gameObject.SetActive(true);
            VictoryDimed.gameObject.SetActive(true);
            VictoryDimed.transform.DOScaleY(VictoryDimed.transform.localScale.y, 2f).From(0f);
        });
        sequence.AppendInterval(2f).AppendCallback(() =>
        {
            VictoryInfo.gameObject.SetActive(true);
            Victory_TimePlayedText.text = "Time Played: " + (TimeManager.instance.Get_TimePlayed() / 60).ToString("D2") + " : " + (TimeManager.instance.Get_TimePlayed() % 60).ToString("D2");
            if (gemreward == 0)
            {
                Victory_GemRewardText.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Victory_GemRewardText.text = gemreward.ToString();
            }
            // Thêm text diamond cho hard mode nữa
            if (diamondreward == 0)
            {
                Victory_DiamondRewardText.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Victory_DiamondRewardText.text = diamondreward.ToString();
            }
            VictoryInfo.DOFade(1f, 1f).From(0f);
        });
        sequence.AppendInterval(1f).AppendCallback(() =>
        {
            VictoryOptions.gameObject.SetActive(true);
            VictoryOptions.DOFade(1f, 1f).From(0f);
        });
    }
    private void Defeat()
    {
        if (EnemyManager.instance != null) { EnemyManager.instance.DestroyAllEnemies(); }
        if (CharacterManager.instance != null) { CharacterManager.instance.DestroyAllCharacters(); }
        BaseBullets[] BulletsOnScreen = GameObject.FindObjectsOfType<BaseBullets>();
        for (int i = 0; i < BulletsOnScreen.Length; i++)
        {
            Destroy(BulletsOnScreen[i]);
        }
        // Get reward
        int gemreward = RewardCalculator.CalculateGem(currentWave, ModeManager.instance.Star, ModeManager.instance.currentGamemode, false);
        CurrencySaveManager.instance.AddGem(gemreward);
        int diamondreward = RewardCalculator.CalculateGem(currentWave, ModeManager.instance.Star, ModeManager.instance.currentGamemode, true);
        CurrencySaveManager.instance.AddDiamonds(diamondreward);
        // UI
        if (SoundManager.Instance != null) { SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Defeat_Sound); }
        DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            TitleBar1.gameObject.SetActive(true);
            DefeatDimed.gameObject.SetActive(true);
            DefeatDimed.transform.DOScaleY(DefeatDimed.transform.localScale.y, 2f).From(0f);
        });
        sequence.AppendInterval(2f).AppendCallback(() =>
        {
            DefeatInfo.gameObject.SetActive(true);
            Defeat_TimePlayedText.text = "Time Played: " + (TimeManager.instance.Get_TimePlayed() / 60).ToString("D2") + " : " + (TimeManager.instance.Get_TimePlayed() % 60).ToString("D2");
            if (gemreward == 0)
            {
                Victory_GemRewardText.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Victory_GemRewardText.text = gemreward.ToString();
            }
            // Thêm text diamond cho hard mode nữa
            if (diamondreward == 0)
            {
                Victory_DiamondRewardText.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Victory_DiamondRewardText.text = diamondreward.ToString();
            }
            DefeatInfo.DOFade(1f, 1f).From(0f);
        });
        sequence.AppendInterval(1f).AppendCallback(() =>
        {
            DefeatOptions.gameObject.SetActive(true);
            DefeatOptions.DOFade(1f, 1f).From(0f);
        });
    }
}
