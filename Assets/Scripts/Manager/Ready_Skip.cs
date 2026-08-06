using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ready_Skip : MonoBehaviour
{
    // Skip UI
    private bool _WantToSkip = false;
    public bool WantToSkip
    {
        get { return _WantToSkip; }
        set { _WantToSkip = value; }
    }
    public CanvasGroup SkipUI;
    // GetReady UI
    private bool _isReady = false;
    public bool isReady
    {
        get { return _isReady; }
        set { _isReady = value; }
    }
    public CanvasGroup ReadyUI;
    public GameObject WaypointArrows;
    private Sequence skipSequence;
    public UnityEvent OnReadyActivated;
    public static Ready_Skip instance;
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
    private void Start()
    {
        WaypointArrows = ModeManager.instance.currentMap.WaypointArrows;
    }
    public IEnumerator GetReady()
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
    public IEnumerator Skip()
    {
        yield return new WaitForSeconds(14f);
        if (GameSetting.instance != null)
        {
            if (GameSetting.instance._autoSkip)
            {
                DoSkip();
            }
            else
            {
                skipSequence?.Kill();
                skipSequence = DOTween.Sequence();
                skipSequence.AppendCallback(() =>
                {
                    SkipUI.gameObject.SetActive(true);
                    SkipUI.DOFade(1f, 0.5f).From(0f);
                });
                skipSequence.AppendInterval(43f);
                skipSequence.AppendCallback(() =>
                {
                    SkipUI.DOFade(0f, 0.5f);
                });
                skipSequence.AppendInterval(0.5f);
                skipSequence.AppendCallback(() =>
                {
                    SkipUI.gameObject.SetActive(false);
                });
            }
        }
    }
    public void CancelSkipUI()
    {
        skipSequence?.Kill();
        skipSequence = null;

        if (SkipUI != null)
        {
            SkipUI.DOKill();
            SkipUI.gameObject.SetActive(false);
        }
    }
    public void DoSkip()
    {
        WantToSkip = true;
        SkipUI.gameObject.SetActive(false);
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Skip_Sound);
    }
    public void DontSkip()
    {
        WantToSkip = false;
        SkipUI.gameObject.SetActive(false);
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Skip_Sound);
    }
    public void Ready()
    {
        isReady = true;
        ReadyUI.gameObject.SetActive(false);
        WaypointArrows.gameObject.SetActive(false);
        StopCoroutine(GetReady());
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Skip_Sound);
        // Thay vì gọi đích danh TutorialManager, ta chỉ "phát tín hiệu"
        OnReadyActivated?.Invoke();
    }
}
