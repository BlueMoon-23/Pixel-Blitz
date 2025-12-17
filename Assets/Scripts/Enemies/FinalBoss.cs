using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : BaseEnemy
{
    // Easy: 25k HP, dậm sàn gây choáng toàn map
    // Medium: 50k HP, dậm sàn và ném kiếm vào character
    // Hard: 100k HP, dậm sàn, ném kiếm. khi máu còn 50k HP thì dậm sàn và chạy nhanh hơn (không ném kiếm nữa)
    public GameObject StompEffect; // ToonBodySlam
    void Start()
    {
        isFinalBoss = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Die();
    }
    protected IEnumerator StompGround()
    {
        do
        {
            SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, IndexPair[PlayerState.ATTACK]);
            SPUM_Prefabs._anim.speed = 0.5f;
            yield return new WaitForSeconds(1f);
            if (SoundManager.Instance != null) { SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.StompGround_Sound); }
            GameObject newEffect = Instantiate(StompEffect, transform.position, Quaternion.identity);
            HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
            hitCharacterExplosion.StunDuration = 2f;
            Destroy(newEffect, 1f);
            yield return new WaitForSeconds(9f);
        }
        while (true);
    }
    public override void TakeDamage(float Damage, bool canStrikethrough)
    {
        base.TakeDamage(Damage, canStrikethrough);
        if (GameManager.instance != null)
        {
            GameManager.instance.BossHPText.text = HP + " / " + MaxHP;
            GameManager.instance.BossHPBar.transform.localScale = new Vector3(HP / MaxHP, GameManager.instance.BossHPBar.transform.localScale.y, GameManager.instance.BossHPBar.transform.localScale.z);
        }
    }
    public override void Die()
    {
        if (HP <= 0)
        {
            if (EconomyManager.instance != null)
            {
                EconomyManager.instance.AddCoin(this.MaxHP);
                EconomyManager.instance.Change_CurrentCoin();
            }
            HP = 0;
            StartCoroutine(PlayDeathAnimation());
        }
    }
    protected IEnumerator PlayDeathAnimation()
    {
        Speed = 0f;
        SPUM_Prefabs.PlayAnimation(PlayerState.DEATH, IndexPair[PlayerState.DEATH]);
        SPUM_Prefabs._anim.speed = 0.25f;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
        yield break;
    }
}
