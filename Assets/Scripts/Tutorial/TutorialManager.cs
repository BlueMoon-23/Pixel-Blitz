using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] public TutorialChecklist[] checklists;
    [SerializeField] public Guidelines[] guidelines;
    private Archer targetArcher;
    private Musketeer targetMusketeer;
    public GameObject ArcherGroup;
    public GameObject MusketeerGroup;
    public GameObject TutorialDimed;
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
        ArcherGroup.SetActive(false);
        MusketeerGroup.SetActive(false);
        TutorialDimed.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator startChecklist(int wave)
    {
        yield return new WaitForSeconds(1f);
        checklists[wave].Appear();
        switch (wave)
        {
            case 1:
                {
                    ArcherGroup.SetActive(true);
                    MusketeerGroup.SetActive(false);
                    break;
                }
            case 2:
                {
                    ArcherGroup.SetActive(false);
                    MusketeerGroup.SetActive(true);
                    break;
                }
            case 3:
                {
                    targetMusketeer.gameObject.SetActive(false);
                    ArcherGroup.SetActive(false);
                    MusketeerGroup.SetActive(false);
                    break;
                }
            case 4:
                {
                    targetArcher.gameObject.SetActive(false);
                    ArcherGroup.SetActive(false);
                    MusketeerGroup.SetActive(false);
                    break;
                }
        }
        while (!checklists[wave].isComplete)
        {
            TutorialDimed.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        if (wave < 4) TutorialDimed.SetActive(true);
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
            ArcherGroup.SetActive(false);
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
                    targetMusketeer.gameObject.SetActive(true);
                }
            };
        }
        else if (targetMusketeer == null && obj.GetComponent<Musketeer>() != null)
        {
            checklists[2].finish_1stCheck();
            targetMusketeer = obj.GetComponent<Musketeer>();
            MusketeerGroup.SetActive(false);
            // "Găm" sự kiện ngay vào con Archer vừa bắt được
            targetMusketeer.OnLevelUp += (level) => {
                if (level == 1) checklists[4].finish_1stCheck();
                if (level == 2)
                {
                    checklists[4].finish_2ndCheck();
                    guidelines[2].Appear();
                    targetArcher.gameObject.SetActive(true);
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
