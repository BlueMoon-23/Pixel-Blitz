using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    private float _currentBaseHealth;
    public float currentBaseHealth
    {
        get { return _currentBaseHealth; }
        set { _currentBaseHealth = value; }
    }
    // Base Health Text
    public TextMeshProUGUI BaseHealthText;
    public Image BaseHealthBar;
    public Image BaseHealthWhiteBar;
    public static BaseHealth instance;
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
        currentBaseHealth = 100f;
    }
    public void BaseGetHit(float Damage)
    {
        currentBaseHealth -= Damage;
        if (currentBaseHealth < 0) currentBaseHealth = 0;
        if (SoundManager.Instance != null) SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.BaseGetHit_Sound);
        BaseHealthText.text = currentBaseHealth.ToString() + " / 100";
        //DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            BaseHealthWhiteBar.gameObject.SetActive(true);
            BaseHealthBar.transform.DOScaleX(currentBaseHealth / 100, 0.5f);
        });
        sequence.AppendInterval(0.25f).AppendCallback(() =>
        {
            BaseHealthWhiteBar.transform.DOScaleX(currentBaseHealth / 100, 0.125f);
        });
        sequence.AppendInterval(0.125f).AppendCallback(() => {
            BaseHealthWhiteBar.gameObject.SetActive(false);
        });
    }
}
