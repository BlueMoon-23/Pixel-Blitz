using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragCharacter : DragThing
{
    void Start()
    {
        baseCharacter = CharacterPrefab.GetComponent<BaseCharacter>();
    }
    protected override void OnPointerDown_Specific(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        m_RectTransform.parent as RectTransform,
        eventData.position,
        eventData.pressEventCamera,
        out offset
    );
        offset = m_RectTransform.anchoredPosition - offset;
    }
    protected override void OnBeginDrag_Specific(PointerEventData eventData) 
    {
        canvasGroup.blocksRaycasts = false;
        // Placing
        if (baseCharacter.GetType().IsSubclassOf(typeof(CliffCharacter)))
        {
            PlacingCliffUI.SetActive(true);
        }
        else
        {
            PlacingGroundUI.SetActive(true);
        }
        CancelPlacing.SetActive(true);
        // Range
        RangeUI.SetActive( true );
        RangeUI.transform.localScale = new Vector3(RangeUI.transform.localScale.x * baseCharacter.GetRange(), RangeUI.transform.localScale.y * baseCharacter.GetRange(), RangeUI.transform.localScale.z * baseCharacter.GetRange());
        range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
    }
    protected override void OnEndDrag_Specific(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        PlacingGroundUI.SetActive(false);
        PlacingCliffUI.SetActive(false);
        RangeUI.SetActive(false);
        RangeUI.transform.localScale = new Vector3(2 / 3f, 2 / 3f, 2 / 3f);
        CancelPlacing.SetActive(false);
        GameObject cancelPlacing = eventData.pointerCurrentRaycast.gameObject;
        if (cancelPlacing.CompareTag("CancelPlacing"))
        {
            m_RectTransform.anchoredPosition = previous_RectTransform;
            range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
            return;
        }
        // Kiem tra xem co du tien de dat character khong
        BaseCharacter character = CharacterPrefab.GetComponent<BaseCharacter>();
        if (EconomyManager.instance != null && character.GetCost() <= EconomyManager.instance.PlayerCoin)
        {
            if (CharacterManager.instance != null)
            {
                if (CharacterManager.instance.GetPopulation() < 20)
                {
                    if (GetDropPosition(eventData.position) != Vector3.zero && !(CharacterManager.instance.hasCharacterinPosition(GetDropPosition(eventData.position))))
                    {
                        if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Place_Sound);
                        GameObject newCharacter = Instantiate(CharacterPrefab, GetDropPosition(eventData.position), Quaternion.identity);
                        newCharacter.SetActive(true);
                        CharacterManager.instance.AddCharacterWithPosition(newCharacter.GetComponent<BaseCharacter>(), GetDropPosition(eventData.position));
                        EconomyManager.instance.Purchase(character.GetCost());
                        EconomyManager.instance.Change_CurrentCoin();
                    }
                }
                else
                {
                    CharacterManager.instance.LimitPlacement_Announce();
                }
            }
        }
        else
        {
            EconomyManager.instance.Announce_CantPlace(character.GetCost());
        }
        m_RectTransform.anchoredPosition = previous_RectTransform;
        range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
    }
}
