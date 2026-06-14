using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : BaseEnemy
{
    protected bool hasDied = false;
    // Easy: 25k HP, dậm sàn gây choáng toàn map
    // Medium: 100k HP, dậm sàn và ném kiếm vào character
    // Hard: 250k HP, dậm sàn, ném kiếm. khi máu còn 100k HP thì dậm sàn và chạy nhanh hơn (không ném kiếm nữa)
    public GameObject StompEffect; // ToonBodySlam
    public GameObject LowGraphic_StompEffect;
    public GameObject SpiralStunEffect; // Sword spin purple
    public GameObject LowGraphic_SpiralStunEffect;
    public GameObject FallingSword;
    public int AbilityCount; // easy = 1, medium = 2, hard = 3
    public bool shouldCastLowHpSkill;
    protected override IEnumerator ResetStats()
    {
        yield return StartCoroutine(base.ResetStats());
        yield return null;
        isFinalBoss = true;
        hasDied = false;
        // Chỉnh máu của boss theo số character mang theo của người chơi
        if (CharacterLoadout.instance != null)
        {
            HP *= (1 + (CharacterLoadout.instance.characterLoadout.Count - 1) / 3f);
            enemyStats.MaxHP *= (1 + (CharacterLoadout.instance.characterLoadout.Count - 1) / 3f);
        }
        yield return new WaitForSeconds(0.01f);
        if (BossManager.instance != null)
        {
            int tag_index = Waypoint_SelectedIndex <= BossManager.instance.bossHP.Length ? Waypoint_SelectedIndex : BossManager.instance.bossHP.Length;
            BossManager.instance.bossHP[tag_index].BossHPText.text = HP + " / " + enemyStats.MaxHP;
        }
        StartCoroutine(DoAbility(AbilityCount));
    }
    // Update is called once per frame
    protected new void Update()
    {
        if (StatsReseted)
        {
            if (!hasDied)
            {
                Move();
                Die();
            }
            ResetIncomingDamage();
            if (shouldCastLowHpSkill && HP <= enemyStats.MaxHP / 2f)
            {
                EnragedAbility();
            }
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
        if (SoundManager.Instance != null) { SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.StompGround_Sound); }
        if (GameSetting.instance != null && GameSetting.instance._shakeEffect) 
        {
            if (ShakeFeedback.instance != null) {
                Debug.Log("Shake");
                ShakeFeedback.instance.ShakeCamera(); 
            }
        }
        /*GameObject newEffect = Instantiate(StompEffect, transform.position, Quaternion.identity);
        HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
        hitCharacterExplosion.StunDuration = 2f;
        Destroy(newEffect, 1f);*/
        GameObject chosenExplosion_SFX = StompEffect;
        if (GameSetting.instance != null && !GameSetting.instance._showExplosion)
        {
            chosenExplosion_SFX = LowGraphic_StompEffect;
        }
        if (ExplosionPooler.instance != null)
        {
            BaseExplosion newEffect = ExplosionPooler.instance.GetExplosion(chosenExplosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
            if (newEffect != null)
            {
                newEffect.transform.position = this.transform.position;
                newEffect.transform.rotation = Quaternion.identity;
                HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
                hitCharacterExplosion.StunDuration = 2f;
                ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(newEffect, 0.5f));
            }
        }
        yield break;
    }
    protected IEnumerator SpiralStun()
    {
        SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, 1);
        SPUM_Prefabs._anim.speed = 0.5f;
        for (int quantity = 0; quantity < 5; quantity++)
        {
            if (CharacterManager.instance != null)
            {
                int character_index_position = Random.Range(0, CharacterManager.instance.GetPopulation());
                GameObject newFallingSword = Instantiate(FallingSword, CharacterManager.instance.GetCharacterByIndex(character_index_position).gameObject.transform.position + new Vector3(0f, 15f, 0f), Quaternion.Euler(0, 0, 180));
                Destroy(newFallingSword, 1f);
                yield return new WaitForSeconds(1f);
                // Instantiate hiệu ứng ở các vị trí ngẫu nhiên của character
                if (SoundManager.Instance != null) { SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.SpiralStun_Sound); }
                if (GameSetting.instance != null && GameSetting.instance._shakeEffect)
                {
                    if (ShakeFeedback.instance != null)
                    {
                        ShakeFeedback.instance.ShakeCamera();
                    }
                }
                /*GameObject newEffect = Instantiate(SpiralStunEffect, CharacterManager.instance.GetCharacterByIndex(character_index_position).gameObject.transform.position, Quaternion.identity);
                HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
                hitCharacterExplosion.StunDuration = 1f;
                Destroy(newEffect, 1f);*/
                GameObject chosenExplosion_SFX = SpiralStunEffect;
                if (GameSetting.instance != null && !GameSetting.instance._showExplosion)
                {
                    chosenExplosion_SFX = LowGraphic_SpiralStunEffect;
                }
                if (ExplosionPooler.instance != null)
                {
                    BaseExplosion newEffect = ExplosionPooler.instance.GetExplosion(chosenExplosion_SFX.GetComponent<BaseExplosion>().ExplosionID);
                    if (newEffect != null)
                    {
                        newEffect.transform.position = CharacterManager.instance.GetCharacterByIndex(character_index_position).gameObject.transform.position;
                        newEffect.transform.rotation = Quaternion.identity;
                        HitCharacterExplosion hitCharacterExplosion = newEffect.GetComponent<HitCharacterExplosion>();
                        hitCharacterExplosion.StunDuration = 1f;
                        ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(newEffect, 1.0f));
                    }
                }
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
        if (BossManager.instance != null)
        {
            int tag_index = Waypoint_SelectedIndex <= BossManager.instance.bossHP.Length ? Waypoint_SelectedIndex : BossManager.instance.bossHP.Length;
            BossManager.instance.bossHP[tag_index].BossHPText.text = HP + " / " + enemyStats.MaxHP;
            BossManager.instance.bossHP[tag_index].BossHPBar.transform.localScale = new Vector3(HP / enemyStats.MaxHP, BossManager.instance.bossHP[tag_index].BossHPBar.transform.localScale.y, BossManager.instance.bossHP[tag_index].BossHPBar.transform.localScale.z);
        }
    }
    protected override void Die()
    {
        if (HP <= 0)
        {
            hasDied = true;
            if (EconomyManager.instance != null)
            {
                EconomyManager.instance.AddCoin(this.enemyStats.MaxHP);
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
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.ReturnEnemy(this);
        }
        yield break;
    }
}
