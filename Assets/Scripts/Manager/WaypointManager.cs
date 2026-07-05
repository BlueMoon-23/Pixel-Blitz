using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    // Script này dùng để quản lý các waypoint. Enemy sẽ truy cập vào đây để tìm lấy waypoint thay vì tự tìm bằng lệnh find
    // Singleton
    public static WaypointManager instance;
    private int callIndex = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public WaypointInformation[] List_of_Waypoints;
    private void Start()
    {
        List_of_Waypoints = ModeManager.instance.currentMap.List_of_Waypoints;
    }
    /// <summary>
    /// C++ có khái niệm về hàm truyền tham số (func(int& n), gọi func(a) thì giá trị của a sẽ bị thay đổi sau khi hết hàm func)
    /// Để thực hiện điều này trong C#, dùng 1 trong 2 từ khóa là out hoặc ref
    /// out: chỉ đơn giản là tạo thẳng 1 số mới trong hàm, kiểu như 1 hàm sẽ return 2 cái vậy
    /// bên trong hàm dùng từ khóa out BẮT BUỘC phải có lệnh gán giá trị
    /// ref: lấy giá trị của tham số, có lệnh gán giá trị mới hay không thì tùy, dùng để thay đổi dữ liệu hiện có
    /// </summary>
    public GameObject[] GetWaypoints(out int index) // out thay cho & trong c++ (int& index)
    {
        int i;
        // Đa đường
        if (ModeManager.instance.currentMap.isMultiPath)
        {
            // Lấy đường theo thứ tự tăng dần của lệnh gọi Coroutine
            i = callIndex % List_of_Waypoints.Length;
            // Tăng biến đếm lên cho lệnh gọi ngay sau đó ở cùng Frame
            callIndex++;
        }
        else
        {
            // Random path / đơn đường
            i = Random.Range(0, List_of_Waypoints.Length);
        }

        index = i;
        return List_of_Waypoints[i].Waypoints;
    }
    public GameObject[] GetWaypointsWithIndex(int index)
    {
        if (index < List_of_Waypoints.Length)
        {
            return List_of_Waypoints[index].Waypoints;
        }
        else return null;
    }
    // Hàm này của Unity tự chạy ở cuối mỗi Frame để reset lại biến đếm về 0
    private void LateUpdate()
    {
        if (callIndex != 0)
        {
            callIndex = 0;
        }
    }
}
