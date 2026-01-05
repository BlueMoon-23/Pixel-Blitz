using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : BaseEnemy
{
    // Easy: 25k HP, dậm sàn gây choáng toàn map
    // Medium: 100k HP, dậm sàn và ném kiếm vào character
    // Hard: 250k HP, dậm sàn, ném kiếm. khi máu còn 100k HP thì dậm sàn và chạy nhanh hơn (không ném kiếm nữa)
    public GameObject StompEffect; // ToonBodySlam
    public GameObject SpiralStunEffect; // Sword spin purple
    public int AbilityCount; // easy = 1, medium = 2, hard = 3
    public bool shouldCastLowHpSkill;
    void Start()
    {
        if (WaypointManager.instance != null)
        {
            Waypoints = WaypointManager.instance.GetWaypointsWithIndex(Waypoint_SelectedIndex);
        }
        isFinalBoss = true;
        // Chỉnh máu của boss theo số character mang theo của người chơi
        if (CharacterLoadout.instance != null) 
        { 
            HP *= (1 + (CharacterLoadout.instance.characterLoadout.Count - 1) / 3f); 
            MaxHP *= (1 + (CharacterLoadout.instance.characterLoadout.Count - 1) / 3f); 
        }
        if (GameManager.instance != null)
        {
            GameManager.instance.BossHPText.text = HP + " / " + MaxHP;
        }
        StartCoroutine(DoAbility(AbilityCount));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Die();
        if (shouldCastLowHpSkill && HP <= MaxHP / 2f)
        {
            EnragedAbility();
        }
    }
    protected IEnumerator DoAbility(int abilityCount)
    {
        int index = Random.Range(0, abilityCount);
        do
        {
            switch (index)
            {
                case 0:
                    {
                        StartCoroutine(StompGround());
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(SpiralStun());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            yield return new WaitForSeconds(14f);
        }
        while (true);
    }
    protected IEnumerator StompGround()
    {
        SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, 0);
        SPUM_Prefabs._anim.speed = 0.5f;
        yield return new WaitForSeconds(1f);
        if (SoundManager.Instance != null) { SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.StompGround_Sound); }
        GameObject newEffect = Instantiate(StompEffect, transform.position, Quaternion.identity);
        HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
        hitCharacterExplosion.StunDuration = 2f;
        Destroy(newEffect, 1f);
        yield break;
    }
    protected IEnumerator SpiralStun()
    {
        SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, 1);
        SPUM_Prefabs._anim.speed = 0.5f;
        yield return new WaitForSeconds(1f);
        for (int quantity = 0; quantity < 5; quantity++)
        {
            if (CharacterManager.instance != null)
            {
                // Instantiate hiệu ứng ở các vị trí ngẫu nhiên của character
                int character_index_position = Random.Range(0, CharacterManager.instance.GetPopulation());
                if (SoundManager.Instance != null) { SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.SpiralStun_Sound); }
                GameObject newEffect = Instantiate(SpiralStunEffect, CharacterManager.instance.GetCharacterByIndex(character_index_position).gameObject.transform.position, Quaternion.identity);
                HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
                hitCharacterExplosion.StunDuration = 1f;
                Destroy(newEffect, 1f);
                yield return new WaitForSeconds(1f);
            }
        }
    }
    protected void EnragedAbility()
    {
        StartCoroutine(StompGround());
        AbilityCount = 1;
        Speed *= 1.5f;
        shouldCastLowHpSkill = false;
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
