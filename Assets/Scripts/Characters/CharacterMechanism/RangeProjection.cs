using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeProjection : MonoBehaviour
{
    // Script gắn lên PlacingCiff để có thể hạ tọa độ Y của Range của Character để hiệu chỉnh range theo thực tế
    [field: SerializeField] public float Adjusted_Y_Position { get; private set; }
}
