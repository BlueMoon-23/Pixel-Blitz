using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class RewardCalculator : MonoBehaviour
{
    public static int CalculateGem(int wave, float star, Gamemodes gamemodes, bool doVictory)
    {
        double BaseGem = 0;
        int BonusGem = 0;
        if (gamemodes.GetType() == typeof(Easy))
        {
            BaseGem = 2 * Mathf.Pow(wave, 1.25f);
            if (doVictory) { BonusGem = 138; }
        }
        else if (gamemodes.GetType() == typeof(Medium))
        {
            BaseGem = 2 * Mathf.Pow(wave, 1.5f);
            if (doVictory) { BonusGem = 171; }
        }
        else if (gamemodes.GetType() == typeof(Hard))
        {
            BaseGem = 0.28 * Mathf.Pow(wave, 2f) + 4.75 * wave;
            if (doVictory) { BonusGem = 497; }
        }
        double starMultiplier = 1 + (star - 1) * 0.1f;
        int totalGem = (int)((BaseGem + BonusGem) * starMultiplier);
        return totalGem;
    }
    public static int CalculateDiamond(int wave, float star, Gamemodes mode, bool doVictory)
    {
        if (mode.GetType() != typeof(Hard)) return 0;
        // Công thức Diamond: 25 * log6(x+1) + (Win ? 50 : 0)
        double baseDiamond = 25 * Mathf.Log(wave + 1, 6);
        int bonusWin = doVictory ? 50 : 0;
        double starMultiplier = 1 + (star - 1) * 0.05f;
        return (int)((baseDiamond + bonusWin) * starMultiplier);
    }
}
