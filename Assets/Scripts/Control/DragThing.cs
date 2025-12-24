using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public abstract class DragThing : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Canvas canvas;
    protected RectTransform m_RectTransform;
    protected Vector2 offset;
    protected CanvasGroup canvasGroup;
    protected Vector2 previous_RectTransform;
    public Tilemap PlacingGround;
    public Tilemap PlacingCliff;
    //
    public GameObject CharacterPrefab;
    public GameObject PlacingGroundUI;
    public GameObject PlacingCliffUI;
    public GameObject CancelPlacing;
    // Range
    public GameObject RangeUI;
    protected RectTransform range_RectTransform;
    protected BaseCharacter baseCharacter;
    protected void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        previous_RectTransform = m_RectTransform.anchoredPosition;
        // Range
        range_RectTransform = RangeUI.GetComponent<RectTransform>();
    }
    void Start()
    {
        baseCharacter = CharacterPrefab.GetComponent<BaseCharacter>();
    }
    protected abstract void OnPointerDown_Specific(PointerEventData eventData);
    protected abstract void OnBeginDrag_Specific(PointerEventData eventData);
    protected abstract void OnEndDrag_Specific(PointerEventData eventData);
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown_Specific(eventData);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDrag_Specific(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDrag_Specific(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        OnDrag_Specific(eventData);
    }
    protected Vector3 GetDropPosition(Vector3 MousePosition)
    {
        Camera camera = Camera.main;
        MousePosition.z = Mathf.Abs(camera.transform.position.z);
        Vector3 WorldPosition = camera.ScreenToWorldPoint(MousePosition);
        if (baseCharacter.GetType().IsSubclassOf(typeof(CliffCharacter)))
        {
            Vector3Int TilePosition = PlacingCliff.WorldToCell(WorldPosition);
            // Kiểm tra có nằm ngoài phạm vi PlacingGround không
            if (PlacingCliff.HasTile(TilePosition))
            {
                return PlacingCliff.GetCellCenterWorld(TilePosition) - (new Vector3(0f, 0.4f, 0f));
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            Vector3Int TilePosition = PlacingGround.WorldToCell(WorldPosition);
            if (PlacingGround.HasTile(TilePosition))
            {
                return PlacingGround.GetCellCenterWorld(TilePosition) - (new Vector3(0f, 0.4f, 0f));
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
    // Phương thức OnDrag_Specific là bình thường vì như nhau ở DragAbility và DragCharacter
    protected void OnDrag_Specific(PointerEventData eventData) 
    {
        // eventData / scaleFactor khong duoc
        // Input.mousePosition khong duoc
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_RectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );
        m_RectTransform.anchoredPosition = localPoint + offset;
        range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
    }
}
