using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * 14 * Time.deltaTime;
    }
}
