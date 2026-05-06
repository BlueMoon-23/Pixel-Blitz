using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnReporter : MonoBehaviour
{
    public static Action<GameObject> OnReport;
    // Start is called before the first frame update
    void Start()
    {
        // Ngay khi vật X (đã găm script này) xuất hiện, nó phát tín hiệu
        OnReport?.Invoke(this.gameObject);
        // Sau khi báo cáo xong có thể tự hủy script này để nhẹ máy
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
