using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    // Boss HP
    public GameObject BossHPGroup;
    public TextMeshProUGUI BossName;
    public TextMeshProUGUI BossHPText;
    public Image BossHPBar;
    public static BossManager instance;
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
}
