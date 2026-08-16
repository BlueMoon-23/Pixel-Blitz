using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gamemodes;

public class TutorialMode : Gamemodes
{
    public override IEnumerator SpawnEnemyWave(int Wave)
    {
        if (TutorialManager.instance != null)
        {
            finished_spawningEnemies = false;
            yield return TutorialManager.instance.StartCoroutine(TutorialManager.instance.startChecklist(Wave));
        }
        switch (Wave)
        {
            case 1:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 3, 1));
                    break;
                }
            case 2:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Normal, 3, 1));
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Quick, 3, 1));
                    break;
                }
            case 3:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Hidden, 3, 1));
                    break;
                }
            case 4:
                {
                    yield return StartCoroutine(SpawnEnemyLayout(EnemyName.Armored, 2, 1));
                    break;
                }
            default:
                {
                    yield return null;
                    break;
                }
        }
    }
    public override void setGemReward(int wave, ref double BaseGem, ref int BonusGem, bool doVictory)
    {
        BaseGem = 2 * Mathf.Pow(wave, 1.25f);
        if (doVictory) { BonusGem = 50; }
    }
    public override void setCoinFormula(int wave, ref float Formula)
    {
        Formula = (int)Mathf.Pow(250 + 100 * wave, 1.2f);
    }
    public override int getMaxWave()
    {
        return 4;
    }
    public override int getDifficulty()
    {
        return 0;
    }
    public override Color getColor()
    {
        return new Color32(165, 255, 107, 255);
    }
}
