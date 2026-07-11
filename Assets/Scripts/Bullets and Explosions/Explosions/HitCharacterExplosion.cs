using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCharacterExplosion : BaseExplosion
{
    // Mặc định thằng này luôn luôn đập character, nhưng có sự phân chia giữa cliff và ground
    private bool CanHitCliffCharacter;
    private bool CanHitDummy;
    [Header("Chỉ được gán cliff character và dummy. Ground character và enemy không cần gán")]
    public List<SIDE> SideCanHit;
    public float StunDuration = 2f;
    void Start()
    {
        CanHitCliffCharacter = false;
        CanHitDummy = false;
        foreach (SIDE side in SideCanHit)
        {
            if (side == SIDE.CliffCharacter) CanHitCliffCharacter = true;
            else if (side == SIDE.Dummy) CanHitDummy = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IStunnable stunnable = collision.GetComponent<IStunnable>();
        ISide side = collision.GetComponent<ISide>();
        if (stunnable != null && side != null)
        {
            if (side.IsPlayerSide())
            {
                if (!stunnable.IsStunImmunity())
                {
                    // Điều kiện gộp: Gặp Ground tự động qua | Gặp Cliff phải có CanHitCliff | Gặp Dummy phải có CanHitDummy
                    if (side.GetSide() == SIDE.GroundCharacter ||
                       (side.GetSide() == SIDE.CliffCharacter && CanHitCliffCharacter) ||
                       (side.GetSide() == SIDE.Dummy && CanHitDummy))
                    {
                        stunnable.ApplyStun(StunDuration);
                    }
                }
            }
        }
    }
}
