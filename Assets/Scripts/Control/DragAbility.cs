using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragAbility : DragThing
{
    public static DragAbility instance;
    private BaseCharacter currentCharacter; // truyền con minigunner có ability vào đây để khóa số lượng 1 clone
    public enum AbilityDragType { None, GroundPlacement, WaypointPlacement }
    public AbilityDragType currentDragType = AbilityDragType.None;
    private GameObject[] Range_Prefab;
    protected new void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        base.Awake();
    }
    public void SetCurrentCharacter(BaseCharacter character)
    {
        currentCharacter = character;
    }

    // Logic from DragCharacter
    protected override void OnPointerDown_Specific(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        m_RectTransform.parent as RectTransform,
        eventData.position,
        eventData.pressEventCamera,
        out offset
    );
        offset = m_RectTransform.anchoredPosition - offset;
        // Tat Range UI o day moi dung
        Range_Prefab = GameObject.FindGameObjectsWithTag("Range");
        for (int i = 0; i < Range_Prefab.Length; i++)
        {
            Range_Prefab[i].GetComponent<Renderer>().enabled = false;
        }
    }
    protected override void OnBeginDrag_Specific(PointerEventData eventData)
    {
        switch (currentDragType)
        {
            case AbilityDragType.GroundPlacement:
                GroundPlacementBeginDrag(eventData);
                break;
            case AbilityDragType.WaypointPlacement:
                WaypointPlacementBeginDrag(eventData);
                break;
            default:
                return;
        }
    }
    protected override void OnEndDrag_Specific(PointerEventData eventData)
    {
        switch (currentDragType)
        {
            case AbilityDragType.GroundPlacement:
                GroundPlacementEndDrag(eventData);
                break;
            case AbilityDragType.WaypointPlacement:
                WaypointPlacementEndDrag(eventData);
                break;
            default:
                return;
        }
    }
    // HÀM RIÊNG BIỆT CHO MỖI CƠ CHẾ
    private void GroundPlacementBeginDrag(PointerEventData eventData)
    {
        // cái này của minigunner
        canvasGroup.blocksRaycasts = false;
        // Placing
        PlacingGroundUI.SetActive(true);
        CancelPlacing.SetActive(true);
        // Range
        RangeUI.SetActive(true);
        // Set range = 15 sẵn luôn
        RangeUI.transform.localScale = new Vector3(RangeUI.transform.localScale.x * 15, RangeUI.transform.localScale.y * 15, RangeUI.transform.localScale.z * 15);
        range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
    }
    private void GroundPlacementEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        PlacingGroundUI.SetActive(false);
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
        //
        currentCharacter.Ability(GetDropPosition(eventData.position));
        //
        m_RectTransform.anchoredPosition = previous_RectTransform;
        range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
    }

    private void WaypointPlacementBeginDrag(PointerEventData eventData)
    {
        // cái này của summoner
    }
    private void WaypointPlacementEndDrag(PointerEventData eventData)
    {
        // cái này của summoner
    }
}
