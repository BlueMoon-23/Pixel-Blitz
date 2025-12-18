using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    [SerializeField] private float _PlayerCoin;
    public float PlayerCoin
    {
        get { return _PlayerCoin; }
    }
    public TextMeshProUGUI CurrentCoin;
    public TextMeshProUGUI Announcement;
    public TextMeshProUGUI EarnCoin;
    public Image EarnCoinIcon;
    private Vector3 original_position1;
    private Vector3 original_position2;
    public TextMeshProUGUI WaveReward;
    public CanvasGroup WaveRewardAnnounecement;
    public TextMeshProUGUI WaveClearBonus;
    public CanvasGroup WaveClearBonusAnnounecement;
    void Start()
    {
        _PlayerCoin = 600f;
        original_position1 = EarnCoin.transform.position;
        original_position2 = EarnCoinIcon.transform.position;
        Change_CurrentCoin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Change_CurrentCoin()
    {
        CurrentCoin.text = PlayerCoin.ToString();
    }
    public void Purchase(float Cost)
    {
        _PlayerCoin -= Cost;
    }
    public void AddCoin(float Bonus)
    {
        _PlayerCoin += Bonus;
        EarnCoin.text = "+ " + Bonus.ToString();
        ShowBonusCoin(EarnCoin);
        ShowBonusCoin(EarnCoinIcon);
        if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.EarnCoin_Sound);
    }
    public void Announce_CantPlace(float Cost)
    {
        Announcement.text = "You need $" + (Cost - _PlayerCoin) + " more to place!";
        ShowAnnounce();
    }
    public void Announce_CantUpgrade(float Cost)
    {
        Announcement.text = "You need $" + (Cost - _PlayerCoin) + " more to upgrade!";
        ShowAnnounce();
    }
    private void ShowAnnounce()
    {
        Announcement.gameObject.SetActive(true);
        Vector3 original_position = Announcement.transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            Announcement.DOFade(1f, 0.25f).From(0f);
        }).Join(Announcement.transform.DOMove(new Vector3(Announcement.transform.position.x, Announcement.transform.position.y - 25f, Announcement.transform.position.z), 0.25f));
        sequence.AppendInterval(1f).Append(Announcement.transform.DOMove(new Vector3(Announcement.transform.position.x, Announcement.transform.position.y + 25f, Announcement.transform.position.z), 0.25f)).AppendInterval(0.25f).JoinCallback(() =>
        {
            Announcement.DOFade(0f, 0.25f).From(1f);
        });
        sequence.OnComplete(() =>
        {
            Announcement.transform.position = original_position;
            Announcement.gameObject.SetActive(false);
        });
    }
    // +337.8 - 406.9
    private void ShowBonusCoin(TextMeshProUGUI EarnCoin)
    {
        EarnCoin.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            EarnCoin.DOFade(1f, 0.25f).From(0f);
        }).Join(EarnCoin.transform.DOMove(new Vector3(EarnCoin.transform.position.x, EarnCoin.transform.position.y - CurrentCoin.transform.position.y / 2, EarnCoin.transform.position.z), 0.25f));
        sequence.AppendCallback(() =>
        {
            EarnCoin.DOFade(0f, 0.25f).From(1f);
        });
        sequence.OnComplete(() =>
        {
            EarnCoin.transform.position = original_position1;
            EarnCoin.gameObject.SetActive(false);
        });
    }
    private void ShowBonusCoin(Image EarnCoin)
    {
        EarnCoin.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            EarnCoin.DOFade(1f, 0.25f).From(0f);
        }).Join(EarnCoin.transform.DOMove(new Vector3(EarnCoin.transform.position.x, EarnCoin.transform.position.y - CurrentCoin.transform.position.y / 2, EarnCoin.transform.position.z), 0.25f));
        sequence.AppendCallback(() =>
        {
            EarnCoin.DOFade(0f, 0.25f).From(1f);
        });
        sequence.OnComplete(() =>
        {
            EarnCoin.transform.position = original_position2;
            EarnCoin.gameObject.SetActive(false);
        });
    }
    public void EarnCoinEachWave(int wave)
    {
        float Formula = 15 * wave * wave + 50 * wave;
        AddCoin(Formula);
        Change_CurrentCoin();
        WaveReward.text = "Wave Reward: $" + (Formula);
        ShowAnnounce(WaveRewardAnnounecement);
        // Wave clear bonus
        if (EnemyManager.instance != null && EnemyManager.instance.GetEnemyListCount() == 0)
        {
            AddCoin((int)((Formula / 3)));
            Change_CurrentCoin();
            WaveClearBonus.text = "Wave Clear Bonus: $" + (int)(Formula / 3);
            WaveClearBonusAnnounecement.gameObject.SetActive(true);
            ShowAnnounce(WaveClearBonusAnnounecement);
        }
    }
    private void ShowAnnounce(CanvasGroup canvasGroup)
    {
        canvasGroup.DOComplete(withCallbacks: true);
        canvasGroup.gameObject.SetActive(true);
        Vector3 original_position = canvasGroup.transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            canvasGroup.DOFade(1f, 0.25f).From(0f);
        }).Join(canvasGroup.transform.DOMove(new Vector3(canvasGroup.transform.position.x, canvasGroup.transform.position.y - 25f, canvasGroup.transform.position.z), 0.25f));
        sequence.AppendInterval(1f).Append(canvasGroup.transform.DOMove(new Vector3(canvasGroup.transform.position.x, canvasGroup.transform.position.y + 25f, canvasGroup.transform.position.z), 0.25f)).AppendInterval(0.25f).JoinCallback(() =>
        {
            canvasGroup.DOFade(0f, 0.25f).From(1f);
        });
        sequence.OnComplete(() =>
        {
            canvasGroup.transform.position = original_position;
            canvasGroup.gameObject.SetActive(false);
        });
    }
}
