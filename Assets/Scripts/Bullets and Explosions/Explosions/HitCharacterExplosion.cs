using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCharacterExplosion : BaseExplosion
{
    public bool CanHitCliffCharacter;
    public bool CanHitSummonerUndead;
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
        BaseCharacter character = collision.GetComponent<BaseCharacter>();
        if (character != null)
        {
            if (CanHitCliffCharacter || !character.profile.isCliff)
            {
                character.characterEffect.StartCoroutine(character.characterEffect.GetStunned(StunDuration));
            }
        }
        if (CanHitSummonerUndead)
        {
            SummonerUndead summonerUndead = collision.GetComponent<SummonerUndead>();
            if (summonerUndead != null && SummonerUndeadPooler.instance != null)
            {
                SummonerUndeadPooler.instance.ReturnUndead(summonerUndead);
            }
        }
    }
}
