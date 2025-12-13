using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int MaxWave = 25;
    public Easy easyMode;
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI currentTimeLeft;
    private bool WantToSkip = false;
    public CanvasGroup SkipUI;
    private float BaseHealth;
    public TextMeshProUGUI BaseHealthText;
    public Image BaseHealthBar;
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        BaseHealth = 100;
        StartCoroutine(SpawnEnemyWave());
        StartCoroutine(Skip());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemyWave()
    {
        for (int i = 1; i <= MaxWave; i++)
        {
            easyMode.StartCoroutine(easyMode.SpawnEnemyWave(i));
            currentWaveText.text = i.ToString() + " / " + MaxWave.ToString();
            int time = 60;
            currentTimeLeft.text = (time / 60).ToString("D2") + " : " + (time % 60).ToString("D2");
            do
            {
                time--;
                currentTimeLeft.text = (time / 60).ToString("D2") + " : " + (time % 60).ToString("D2");
                yield return new WaitForSeconds(1f);
            }
            while (time > 0 && !WantToSkip);
            yield return new WaitForSeconds(1f);
        }
        // logic win: check base health...
        yield break;
    }
    IEnumerator Skip()
    {
        yield return new WaitForSeconds(30f);
        // Show Skip annouonce
        DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            SkipUI.DOFade(1f, 0.5f).From(0f);
        });
        sequence.AppendInterval(0.5f).AppendCallback(() => {
            SkipUI.gameObject.SetActive(true);
        });
        sequence.AppendInterval(28f).AppendCallback(() =>
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
    }
    public void DontSkip()
    {
        WantToSkip = false;
    }
    public void BaseGetHit(float Damage)
    {
        BaseHealth -= Damage;
        BaseHealthText.text = BaseHealth.ToString() + " / 100";
        DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            BaseHealthBar.transform.DOScaleX(BaseHealth / 100, 0.5f);
        });
    }
}
