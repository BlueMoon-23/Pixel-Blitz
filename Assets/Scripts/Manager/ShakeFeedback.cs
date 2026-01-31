using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class ShakeFeedback : MonoBehaviour
{
    public MMF_Player Player;
    public static ShakeFeedback instance;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShakeCamera()
    {
        Player.PlayFeedbacks();
    }
}
