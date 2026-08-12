using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoxUI : MonoBehaviour
{
    public Image EnemyImage;
    public EnemyProfile enemyProfile;
    public void SetInformation(EnemyProfile profile)
    {
        enemyProfile = profile;
        EnemyImage.sprite = profile.EnemyImage;
    }
    public void ShowInformation()
    {
        // Gọi EnemyIndex show info bla bla
        if (EnemyIndex.instance != null && enemyProfile != null)
        {
            EnemyIndex.instance.ShowInformation(enemyProfile);
        }
    }
}
