using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialChecklist : MonoBehaviour
{
    public List<GameObject> BoxTicks = new List<GameObject>();
    public GameObject Checklist;
    public bool isComplete = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void finish_1stCheck()
    {
        BoxTicks[0].SetActive(true);
        if (BoxTicks.Count == 1) StartCoroutine(Disappear());
    }
    public void finish_2ndCheck()
    {
        BoxTicks[1].SetActive(true);
        if (BoxTicks.Count == 2) StartCoroutine(Disappear());
    }
    IEnumerator Disappear()
    {
        isComplete = true;
        yield return new WaitForSeconds(2);
        // Move the checklist to the left;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveX(-400, 1f));
        sequence.OnComplete(() => { Checklist.gameObject.SetActive(false); });
    }
    public void Appear()
    {
        Checklist.gameObject.SetActive(true);
        // Move the checklist to the left;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveX(0, 1f));
    }
}
