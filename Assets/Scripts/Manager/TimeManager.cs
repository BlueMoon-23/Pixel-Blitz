using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
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
    [SerializeField] private int TimePlayed;
    private Coroutine Coroutine;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator TimeCount()
    {
        while (true)
        {
            TimePlayed++;
            yield return new WaitForSeconds(1f);
        }
    }
    public int Get_TimePlayed()
    {
        // StopCoroutine(TimeCount()); viết như này thì sẽ không dừng được do không xác định được coroutine. cần truyền coroutine bằng tham chiếu
        StopCoroutine(Coroutine);
        return TimePlayed;
    }
    public void SetCoroutine(Coroutine coroutine)
    {
        Coroutine = coroutine;
    }
}
