using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mauler : BaseEnemy
{
    public GameObject MaulerAim; 
    protected override IEnumerator ResetStats()
    {
        yield return StartCoroutine(base.ResetStats());
        yield return null;
        isFinalBoss = false;
        StartCoroutine(SmashGround());
    }
    private IEnumerator SmashGround()
    {
        do
        {
            SPUM_Prefabs.PlayAnimation(PlayerState.ATTACK, 0);
            yield return new WaitForSeconds(0.5f);
            if (SoundManager.Instance != null) { SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.StompGround_Sound); }
            CreateBlastChain();
            yield return new WaitForSeconds(9.5f);
        }
        while (true);
    }
    private void CreateBlastChain()
    {
        if (CharacterManager.instance != null)
        {
            // Tìm character. Do getpopulation có cả cliff character nên phải áp thuật toán tìm lại cho đến khi index khác cliff character
            int character_index_position = Random.Range(0, CharacterManager.instance.GetPopulation());
            while (CharacterManager.instance.GetCharacterByIndex(character_index_position).profile.isCliff)
            {
                character_index_position = Random.Range(0, CharacterManager.instance.GetPopulation());
            }
            // Instantiate MaulerAim rồi setting mauleraim là xong
            GameObject newMaulerAim = Instantiate(MaulerAim, transform.position, Quaternion.identity);
            MaulerAim maulerAim = newMaulerAim.GetComponent<MaulerAim>();
            Destroy(maulerAim.gameObject, 2.5f);
            if (maulerAim != null)
            {
                maulerAim.SetAimedCharacter(CharacterManager.instance.GetCharacterByIndex(character_index_position));
            }
        }
    }
}
