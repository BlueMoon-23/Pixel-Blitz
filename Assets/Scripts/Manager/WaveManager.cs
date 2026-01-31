using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // Wave Text
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI currentTimeLeft;
    public TextMeshProUGUI WaveText;
    [SerializeField] private int currentWave;
    private Coroutine skipCoroutine;
    // Singleton
    public static WaveManager instance;
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
    public int GetCurrentWave() { return currentWave; }
    public IEnumerator SpawnEnemyWave(Gamemodes mode, int MaxWave)
    {
        Ready_Skip.instance.ReadyUI.gameObject.SetActive(false);
        Coroutine coroutine = TimeManager.instance.StartCoroutine(TimeManager.instance.TimeCount());
        TimeManager.instance.SetCoroutine(coroutine);
        for (currentWave = 1; currentWave <= MaxWave; currentWave++)
        {
            if (!Win_Lose.instance.Defeated) // khong co if nay thi vong while bi break, wavetext bi lap lai nhieu lan
            {
                // Chỉnh chữ
                WaveText.text = "Wave " + currentWave + " is coming!";
                yield return new WaitForSeconds(1f); // Time between waves, sau ưng cài thì cài chỗ này
                WaveText.text = "Base Health";
                currentWaveText.text = currentWave.ToString() + " / " + MaxWave.ToString();
                // Thưởng tiền
                if (EconomyManager.instance != null) EconomyManager.instance.EarnCoinEachWave(mode, currentWave);
                // Sinh quái
                mode.StartCoroutine(mode.SpawnEnemyWave(currentWave));
                // Reset lại vụ skip
                Ready_Skip.instance.WantToSkip = false; // khong co lenh nay la neu skip roi thi wanttoskip = true, break vong while
                if (skipCoroutine != null) { StopCoroutine(skipCoroutine); }
                if (currentWave < MaxWave) { skipCoroutine = StartCoroutine(Ready_Skip.instance.Skip()); }
                // Time handle
                int time = 60;
                if (currentWave == MaxWave) { time = 300; }
                currentTimeLeft.text = (time / 60).ToString("D2") + " : " + (time % 60).ToString("D2");
                do
                {
                    time--;
                    currentTimeLeft.text = (time / 60).ToString("D2") + " : " + (time % 60).ToString("D2");
                    yield return new WaitForSeconds(1f);
                    // Skip luon neu khong co enemy
                    if (EnemyManager.instance != null && EnemyManager.instance.isEmptyEnemies() && mode.isFinished_spawning())
                    {
                        if (skipCoroutine != null) { StopCoroutine(skipCoroutine); } // có tình huống khi do skip như này thì cục UI skip vẫn hiện lên, nên phải để ở đây vì phải dừng cái skip cũ để thực hiện skip mới
                        Ready_Skip.instance.DoSkip();
                    }
                }
                while (time > 0 && !Ready_Skip.instance.WantToSkip && !Win_Lose.instance.Defeated);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield break;
            }
        }
        yield return new WaitForSeconds(5f);
        if (!Win_Lose.instance.Defeated) Win_Lose.instance.Victory();
        yield break;
    }
}
// wave 1: skip coroutine = null
