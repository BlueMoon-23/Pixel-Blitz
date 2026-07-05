using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapScroll : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Camera camera;
    private Vector3 dragOrigin;
    private Tilemap tilemap;
    private void Awake()
    {
        camera = Camera.main;
        tilemap = ModeManager.instance.currentMap.ViewBounds;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragOrigin = camera.ScreenToWorldPoint(eventData.position);
        Vector2 screenPos = eventData.position;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        // Raycast kiểm tra xem có trúng CharacterControl không
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction); // raycast như này mới trúng được collider của character mà check
        if (hit.collider != null)
        {
            // Nếu object trúng có component CharacterControl thì bỏ qua UI_Off()
            if (hit.collider.GetComponent<CharacterControll>() != null)
            {
                return; // không làm gì cả
            }
        }
        if (CharacterUIControll.instance != null)
        {
            CharacterUIControll.instance.UI_Off();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnEndDrag(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 MousePosition = camera.ScreenToWorldPoint(eventData.position);
        camera.transform.position += -MousePosition + dragOrigin;
    }
    private void LateUpdate()
    {
        if (tilemap != null)
        {
            float max_X = (tilemap.localBounds.extents.x - 2) / 2f;
            float max_Y = (tilemap.localBounds.extents.y - 2) / 2f;
            float min_X = (-tilemap.localBounds.extents.x + 2) / 2f;
            float min_Y = (-tilemap.localBounds.extents.y + 2) / 2f;
            Vector3 CorrectPosition = camera.transform.position;
            CorrectPosition.x = Mathf.Clamp(camera.transform.position.x, min_X, max_X);
            CorrectPosition.y = Mathf.Clamp(camera.transform.position.y, min_Y, max_Y);
            camera.transform.position = CorrectPosition;
        }
    }
}
