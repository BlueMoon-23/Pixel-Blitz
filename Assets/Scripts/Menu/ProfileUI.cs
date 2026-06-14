using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    public TextMeshProUGUI Username;
    [Header("Bat/Tat")]
    private int currentLocation; // chỉ số sẽ nhảy 
    public GameObject[] Information; // gán Stats và History
    public GameObject[] LineFrames;
    [Header("Most uses")]
    public Image[] Character_with_MostUses;
    [Header("Player stats")]
    public TextMeshProUGUI ClearedTimes;
    public TextMeshProUGUI RoundTimes;
    public TextMeshProUGUI AttemptTimes;
    public TextMeshProUGUI Ratio;
    public TextMeshProUGUI ChracterOwned;
    [Header("Match history")]
    public GameObject MatchPrefab;
    public GameObject HistoryGameObject;
    void Start()
    {
        InitPlayerStats();
        InitHistory();
        PlayerStats();
    }
    private void InitPlayerStats()
    {
        var account = AccountSaveManager.CurrentAccount;
        Username.text = account.Username;
        List<CharacterData> list = account.userCharacterData.Top4_Character_with_MostUses();
        int i;
        for (i = 0; i < list.Count; i++)
        {
            Character_with_MostUses[i].sprite = list[i].characterProfile.CharacterImage;
        }
        for (; i < Character_with_MostUses.Length; i++)
        {
            Character_with_MostUses[i].transform.parent.parent.gameObject.SetActive(false);
        }
        ClearedTimes.text = account.ClearedTimes.ToString();
        RoundTimes.text = account.RoundTimes.ToString();
        AttemptTimes.text = account.AttemptTimes.ToString();
        if (account.RoundTimes > 0)
        {
            Ratio.text = $"{(float)account.ClearedTimes / account.RoundTimes:F2}";
        }
        else
        {
            Ratio.text = "0.00";
        }
        ChracterOwned.text = account.userCharacterData.OwnedCharacters.Count.ToString();
    }
    private void InitHistory()
    {
        if (MatchSaveManager.instance != null) {
            for (int i = MatchSaveManager.instance.userMatchData.list.Count - 1; i >= 0; i--) {
                GameObject MatchObject = Instantiate(MatchPrefab.gameObject, HistoryGameObject.transform.position, transform.rotation);
                // Ép làm con của HistoryGameObject
                MatchObject.transform.SetParent(HistoryGameObject.transform, false);
                MatchUI matchUI = MatchObject.GetComponent<MatchUI>();
                if (matchUI != null)
                {
                    matchUI.UpdateUI(MatchSaveManager.instance.userMatchData.list[i]);
                }
            }
        }
    }
    public void PlayerStats()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.MoveButton_Sound);
        currentLocation = 0;
        ShowInformation();
    }
    public void History()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.MoveButton_Sound);
        currentLocation = 1;
        ShowInformation();
    }
    private void ShowInformation()
    {
        Information[currentLocation].SetActive(true);
        LineFrames[currentLocation].SetActive(true);
        for (int index = 0; index < Information.Length; index++)
        {
            if (index == currentLocation) continue;
            Information[index].SetActive(false);
            LineFrames[index].SetActive(false);
        }
    }
}
