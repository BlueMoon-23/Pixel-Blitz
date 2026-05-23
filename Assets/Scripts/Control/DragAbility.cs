using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class DragAbility : DragThing
{
    public static DragAbility instance;
    [SerializeField] private BaseCharacter currentCharacter; // truyền con minigunner có ability vào đây để khóa số lượng 1 clone
    public enum AbilityDragType { None, GroundPlacement, WaypointPlacement }
    public AbilityDragType currentDragType = AbilityDragType.None;
    private GameObject[] Range_Prefab;
    // Truyền range vào đây để các drag sau hiện range chỉ định đúng range
    public float DragRange; 
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
        RangeUI.transform.localScale = new Vector3(RangeUI.transform.localScale.x * DragRange, RangeUI.transform.localScale.y * DragRange, RangeUI.transform.localScale.z * DragRange);
        range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
    }
    private void GroundPlacementEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        PlacingGroundUI.SetActive(false);
        RangeUI.SetActive(false);
        RangeUI.transform.localScale = new Vector3(1f, 1f, 1f);
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
        // Tắt characterUIControll, mang lại cảm giác thông thoáng hơn
        if (CharacterUIControll.instance != null)
        {
            CharacterUIControll.instance.UI_Off();
        }
    }

    private void WaypointPlacementBeginDrag(PointerEventData eventData)
    {
        // cái này của summoner
        canvasGroup.blocksRaycasts = false;
        // Placing
        WaypointUI.SetActive(true);
        CancelPlacing.SetActive(true);
        // Range
        currentCharacter.Range_Prefab.GetComponent<Renderer>().enabled = true;

    }
    private void WaypointPlacementEndDrag(PointerEventData eventData)
    {
        // cái này của summoner
        canvasGroup.blocksRaycasts = true;
        WaypointUI.SetActive(false);
        CancelPlacing.SetActive(false);
        GameObject cancelPlacing = eventData.pointerCurrentRaycast.gameObject;
        if (cancelPlacing.CompareTag("CancelPlacing"))
        {
            m_RectTransform.anchoredPosition = previous_RectTransform;
            range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
            return;
        }
        //
        currentCharacter.Ability(GetWaypointDropPosition(eventData.position));
        //
        m_RectTransform.anchoredPosition = previous_RectTransform;
    }
}
