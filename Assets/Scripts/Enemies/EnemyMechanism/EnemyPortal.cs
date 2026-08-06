using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    // Script quản lý việc teleport cho enemy
    public EnemyPortal targetPortal; // Cổng đích
    // Quy tắc để không cho enemy teleport lại cổng cũ khi chạm cổng mới: gán EndPortal sẵn để không bị teleport back lại
    // Sau đó khi thoát khỏi collider cổng, gán EndPortal = null để cho nó teleport cổng mới
    [Header("Truyền waypoint ở targetPortal vào")]
    public GameObject[] WaypointLocations; // Với mỗi waypoint trong này, duyệt trong danh sách waypoint của enemy để xác định waypoint index
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tìm bất cứ object nào có khả năng dịch chuyển
        ITeleportable teleportObj = collision.GetComponent<ITeleportable>();
        if (teleportObj != null)
        {
            teleportObj.DoTeleport(targetPortal);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ITeleportable teleportObj = collision.GetComponent<ITeleportable>();
        if (teleportObj != null)
        {
            teleportObj.StopTeleport(this); // Vẫn dùng "this" như cũ để mở khóa
        }
    }
}
