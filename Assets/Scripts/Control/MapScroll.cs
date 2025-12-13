using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MapScroll : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Camera camera;
    private Vector3 dragOrigin;
    public Tilemap tilemap;
    private void Awake()
    {
        camera = Camera.main;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragOrigin = camera.ScreenToWorldPoint(eventData.position);
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
            float max_X = (tilemap.localBounds.extents.x - 4) / 2f;
            float max_Y = (tilemap.localBounds.extents.y - 2) / 2f;
            float min_X = (-tilemap.localBounds.extents.x + 4) / 2f;
            float min_Y = (-tilemap.localBounds.extents.y + 2) / 2f;
            Vector3 CorrectPosition = camera.transform.position;
            CorrectPosition.x = Mathf.Clamp(camera.transform.position.x, min_X, max_X);
            CorrectPosition.y = Mathf.Clamp(camera.transform.position.y, min_Y, max_Y);
            camera.transform.position = CorrectPosition;
        }
    }
}
