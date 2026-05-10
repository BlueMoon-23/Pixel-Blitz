using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Guidelines : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string fullText;
    public float timeBetweenChars = 0.05f; // Tốc độ hiện từng chữ
    public CanvasGroup DialogueBox;
    //private bool isTyping = true;
    IEnumerator ShowTextByChar()
    {
        textDisplay.text = ""; // Xóa sạch chữ ban đầu

        // Duyệt qua từng ký tự một
        foreach (char c in fullText)
        {
            textDisplay.text += c; // Cộng dồn từng chữ cái vào UI

            // Đợi một khoảng thời gian rất ngắn
            yield return new WaitForSeconds(timeBetweenChars);
        }
        //isTyping = false;
        Disappear();
    }
    public void Appear()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DialogueBox.DOFade(1f, 0.5f));
        StartCoroutine(ShowTextByChar());
    }
    public void Disappear()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(2f).Append(DialogueBox.DOFade(0f, 0.5f));
    }
}
