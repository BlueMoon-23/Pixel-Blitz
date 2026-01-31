using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StarExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage(15f, true);
            baseEnemy.StartCoroutine(baseEnemy.GetStunned(1f));
        }
    }
}
