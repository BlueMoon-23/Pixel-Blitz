using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win_Lose : MonoBehaviour
{
    private bool _Defeated = false;
    public bool Defeated
    {
        get { return _Defeated; }
        set { _Defeated = value; }
    }
    // Victory UI
    public CanvasGroup TitleBar;
    public Image VictoryDimed;
    public CanvasGroup VictoryInfo;
    public CanvasGroup VictoryOptions;
    public TextMeshProUGUI Victory_TimePlayedText;
    public TextMeshProUGUI Victory_GemRewardText;
    public TextMeshProUGUI Victory_DiamondRewardText;
    public TextMeshProUGUI Victory_GamemodeText;
    // Defeat UI
    public CanvasGroup TitleBar1;
    public Image DefeatDimed;
    public CanvasGroup DefeatInfo;
    public CanvasGroup DefeatOptions;
    public TextMeshProUGUI Defeat_TimePlayedText;
    public TextMeshProUGUI Defeat_GemRewardText;
    public TextMeshProUGUI Defeat_DiamondRewardText;
    public TextMeshProUGUI Defeat_GamemodeText;
    public static Win_Lose instance;
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
    public void Restart()
    {
        // Làm lại ván mới
        if (MatchSaveManager.instance != null)
        {
            MatchSaveManager.instance.RestartMatch();
        }
        Scene currentScene = SceneManager.GetActiveScene();
        SceneKey.targetScene = currentScene.name;
        SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
    }
    public void Exit()
    {
        SceneKey.targetScene = SceneKey.MapChoose;
        SceneManager.LoadSceneAsync(SceneKey.LoadingScene);
    }
    private void ClearObjects()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayBGM(null);
        if (EnemyManager.instance != null) { EnemyManager.instance.ClearPool(); }
        if (CharacterManager.instance != null) { CharacterManager.instance.DestroyAllCharacters(); }
        if (BulletPooler.instance != null) { BulletPooler.instance.ClearPool(); }
        if (ExplosionPooler.instance != null) { ExplosionPooler.instance.ClearPool(); }
        if (SummonerUndeadPooler.instance != null) { SummonerUndeadPooler.instance.ClearPool(); }
        if (VFXPooler.instance != null) { VFXPooler.instance.ClearPool(); }
        // xuất hiện tình trạng khi chơi lại lần 2 thì prefab chưa bị clear, dẫn đến việc reference lộn xộn
        if (ModeManager.instance != null) { ModeManager.instance.ClearEnemyPrefab(); }
    }
    private void RecordMatch()
    {
        if (AccountSaveManager.instance == null || MatchSaveManager.instance == null) return;
        if (!Defeated) AccountSaveManager.CurrentAccount.ClearedTimes++;
        AccountSaveManager.CurrentAccount.AttemptTimes++;
        MatchSaveManager.instance.UpdateCurrentMatch(!Defeated, TimeManager.instance.Get_TimePlayed());
    }
    public void Victory()
    {
        ClearObjects();
        RecordMatch(); // ghi trước, rồi nhờ currencysavemanager lưu hộ mình
        if (WeaponDropManager.instance != null) WeaponDropManager.instance.DropWeapon(); // Thưởng vũ khí cho người chơi
        int gemReward = RewardCalculator.CalculateGem(WaveManager.instance.GetCurrentWave(), ModeManager.instance.Star, ModeManager.instance.currentGamemode, true);
        CurrencySaveManager.instance.AddGem(gemReward);
        int diamondReward = RewardCalculator.CalculateDiamond(WaveManager.instance.GetCurrentWave(), ModeManager.instance.Star, ModeManager.instance.currentGamemode, true);
        CurrencySaveManager.instance.AddDiamonds(diamondReward);
        if (SoundManager.Instance != null) { SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Victory_Sound); }
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
            Victory_GamemodeText.text = ModeManager.instance.currentMap.MapName + " | " + ModeManager.instance.currentGamemode.GetType().Name;
            Victory_TimePlayedText.text = "Time Played: " + (TimeManager.instance.Get_TimePlayed() / 60).ToString("D2") + " : " + (TimeManager.instance.Get_TimePlayed() % 60).ToString("D2");
            if (gemReward == 0)
            {
                Victory_GemRewardText.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Victory_GemRewardText.text = gemReward.ToString();
            }
            // Thêm text diamond cho hard mode nữa
            if (diamondReward == 0)
            {
                Victory_DiamondRewardText.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Victory_DiamondRewardText.text = diamondReward.ToString();
            }
            VictoryInfo.DOFade(1f, 1f).From(0f);
        });
        sequence.AppendInterval(Time.deltaTime).AppendCallback(() =>
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(Victory_GamemodeText.gameObject.transform.parent.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(Victory_TimePlayedText.gameObject.transform.parent.GetComponent<RectTransform>());
        });
        sequence.AppendInterval(1f).AppendCallback(() =>
        {
            VictoryOptions.gameObject.SetActive(true);
            VictoryOptions.DOFade(1f, 1f).From(0f);
        });
    }
    public void Defeat()
    {
        ClearObjects();
        RecordMatch();
        // Get reward
        int gemreward = RewardCalculator.CalculateGem(WaveManager.instance.GetCurrentWave(), ModeManager.instance.Star, ModeManager.instance.currentGamemode, false);
        CurrencySaveManager.instance.AddGem(gemreward);
        int diamondreward = RewardCalculator.CalculateDiamond(WaveManager.instance.GetCurrentWave(), ModeManager.instance.Star, ModeManager.instance.currentGamemode, false);
        CurrencySaveManager.instance.AddDiamonds(diamondreward);
        // UI
        if (SoundManager.Instance != null) { SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Defeat_Sound); }
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
            Defeat_GamemodeText.text = ModeManager.instance.currentMap.MapName + " | " + ModeManager.instance.currentGamemode.GetType().Name;
            Defeat_TimePlayedText.text = "Time Played: " + (TimeManager.instance.Get_TimePlayed() / 60).ToString("D2") + " : " + (TimeManager.instance.Get_TimePlayed() % 60).ToString("D2");
            if (gemreward == 0)
            {
                Defeat_GemRewardText.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Defeat_GemRewardText.text = gemreward.ToString();
            }
            // Thêm text diamond cho hard mode nữa
            if (diamondreward == 0)
            {
                Defeat_DiamondRewardText.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Defeat_DiamondRewardText.text = diamondreward.ToString();
            }
            DefeatInfo.DOFade(1f, 1f).From(0f);
        });
        sequence.AppendInterval(Time.deltaTime).AppendCallback(() =>
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(Defeat_GamemodeText.gameObject.transform.parent.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(Defeat_TimePlayedText.gameObject.transform.parent.GetComponent<RectTransform>());
        });
        sequence.AppendInterval(1f).AppendCallback(() =>
        {
            DefeatOptions.gameObject.SetActive(true);
            DefeatOptions.DOFade(1f, 1f).From(0f);
        });
    }
}
