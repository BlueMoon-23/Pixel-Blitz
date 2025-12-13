using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragCharacter : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    private RectTransform m_RectTransform;
    private Vector2 offset;
    private CanvasGroup canvasGroup;
    private Vector2 previous_RectTransform;
    public Tilemap PlacingGround;
    public Tilemap PlacingCliff;
    //
    public GameObject CharacterPrefab;
    public GameObject PlacingGroundUI;
    public GameObject PlacingCliffUI;
    public GameObject CancelPlacing;
    // Range
    public GameObject RangeUI;
    private RectTransform range_RectTransform;
    private BaseCharacter baseCharacter;
    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        previous_RectTransform = m_RectTransform.anchoredPosition;
        // Range
        range_RectTransform = RangeUI.GetComponent<RectTransform>();
        baseCharacter = CharacterPrefab.GetComponent<BaseCharacter>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        m_RectTransform.parent as RectTransform,
        eventData.position,
        eventData.pressEventCamera,
        out offset
    );
        offset = m_RectTransform.anchoredPosition - offset;
    }
    public void OnBeginDrag(PointerEventData eventData) 
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
    public void OnEndDrag(PointerEventData eventData)
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
                        GameObject newCharacter = Instantiate(CharacterPrefab, GetDropPosition(eventData.position), Quaternion.identity);
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
    public void OnDrag(PointerEventData eventData)
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
    public Vector3 GetDropPosition(Vector3 MousePosition)
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
}
