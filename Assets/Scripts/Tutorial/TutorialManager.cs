using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] public TutorialChecklist[] checklists;
    [SerializeField] public Guidelines[] guidelines;
    private Archer targetArcher;
    private Musketeer targetMusketeer;
    public static TutorialManager instance;
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
    // Start is called before the first frame update
    void Start()
    {
        checklists[0].Appear();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator startChecklist(int wave)
    {
        yield return new WaitForSeconds(1f);
        checklists[wave].Appear();
        while (!checklists[wave].isComplete)
        {
            yield return new WaitForSeconds(1f);
        }
    }
    void OnEnable()
    {
        SpawnReporter.OnReport += CheckTask;
    }

    void OnDisable()
    {
        SpawnReporter.OnReport -= CheckTask;
    }

    void CheckTask(GameObject obj)
    {
        if (targetArcher == null && obj.GetComponent<Archer>() != null)
        {
            // Hoàn thành checklist kéo Archer
            checklists[1].finish_1stCheck();
            targetArcher = obj.GetComponent<Archer>();
            // "Găm" sự kiện ngay vào con Archer vừa bắt được
            targetArcher.OnLevelUp += (level) => {
                if (level == 1)
                {
                    checklists[1].finish_2ndCheck();
                    guidelines[0].Appear();
                }
                if (level == 2) checklists[3].finish_1stCheck();
                if (level == 3)
                {
                    checklists[3].finish_2ndCheck();
                    guidelines[1].Appear();
                }
            };
        }
        else if (targetMusketeer == null && obj.GetComponent<Musketeer>() != null)
        {
            checklists[2].finish_1stCheck();
            targetMusketeer = obj.GetComponent<Musketeer>();
            // "Găm" sự kiện ngay vào con Archer vừa bắt được
            targetMusketeer.OnLevelUp += (level) => {
                if (level == 1) checklists[4].finish_1stCheck();
                if (level == 2)
                {
                    checklists[4].finish_2ndCheck();
                    guidelines[2].Appear();
                    PlayedTutorial();
                }
            };
        }
    }
    public void PlayedTutorial()
    {
        if (AccountSaveManager.instance != null)
        {
            Debug.Log("Finished");
            AccountSaveManager.CurrentAccount.hasPlayedTutorial = true;
            // Ghi lên OwnedCharacterKey
            string json = JsonUtility.ToJson(AccountSaveManager.CurrentAccount.hasPlayedTutorial);
            PlayerPrefs.SetString(UserDataKey.PLAYEDTUTORIAL, json);
            PlayerPrefs.Save();
            AccountSaveManager.instance.SaveAccounts();
        }
    }
}
