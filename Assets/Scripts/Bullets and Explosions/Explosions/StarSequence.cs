using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSequence : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] StarExplosions; // lắp theo hệ đếm -2, -1, 0, 1 , 2
    void Start()
    {
        StartCoroutine(ExplodeInSequence());
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator ExplodeInSequence()
    {
        for (int i = 0; i <= 2; i++)
        {
            StarExplosions[i + 2].SetActive(true);
            StarExplosions[-i + 2].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            StarExplosions[i + 2].SetActive(false);
            StarExplosions[-i + 2].SetActive(false);
        }
        // vòng 1: bật 2, 2
        // vòng 2: bậc 3, 1
        // vòng 3: bậc 4, 0
    }
}
