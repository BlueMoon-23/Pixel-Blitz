using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EarnCoinVFX : BaseVFX
{
    public TextMeshProUGUI EarnCoinText;
    void OnEnable()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(25f, 1f)).SetRelative(true).AppendInterval(1f).AppendCallback(() =>
        {
            if (VFXPooler.instance != null)
            {
                VFXPooler.instance.ReturnVFX(this);
            }
        });
    }
    public void SetEarnCoinText(float Coin)
    {
        EarnCoinText.text = "+ " + Coin.ToString();
    }
}
