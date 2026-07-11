using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public Material OriginalMaterial;
    public Material HitMaterial;
    // HP Bar;
    public GameObject HP_RedBar;
    public GameObject HP_WhiteBar;
    private Tween whiteBarTween;
    // Die
    public GameObject DieExplosion;
    void Start()
    {
        //HPBar_Renderer = HP_RedBar.GetComponent<SpriteRenderer>();
    }
    public void GetHit(EnemyStats enemyStats, BaseEnemy enemy)
    {
        if (enemyStats != null)
        {
            HP_RedBar.transform.localScale = new Vector3(enemyStats.Original_x_HPScale * enemy.GetHP() / enemyStats.MaxHP, HP_RedBar.transform.localScale.y, HP_RedBar.transform.localScale.z);
            // Thanh trắng
            if (!HP_WhiteBar.activeSelf)
            {
                HP_WhiteBar.SetActive(true);
            }
            if (whiteBarTween != null && whiteBarTween.IsActive())
            {
                whiteBarTween.Kill();
            }
            HP_WhiteBar.transform.DOScaleX(enemyStats.Original_x_HPScale * enemy.GetHP() / enemyStats.MaxHP, 0.08f)
                .OnComplete(() =>
                {
                    HP_WhiteBar.SetActive(false);
                });
        }
    }
}
