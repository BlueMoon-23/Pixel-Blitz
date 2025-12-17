using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCharacterExplosion : MonoBehaviour
{
    public float StunDuration = 2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseCharacter character = collision.GetComponent<GroundCharacter>();
        if (character != null)
        {
            character.StartCoroutine(character.GetStunned(StunDuration));
        }
    }
}
