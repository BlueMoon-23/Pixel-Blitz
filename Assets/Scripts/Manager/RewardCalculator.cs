using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class RewardCalculator : MonoBehaviour
{
    public static int CalculateGem(int wave, float star, Gamemodes gamemodes, bool doVictory)
    {
        // Easy: 2x^1.25 + 138
        // Medium: 2*x^1.5 + 171
        // Hard: 0.28*x^2 + 4.75x + 497
        double BaseGem = 0;
        int BonusGem = 0;
        gamemodes.setGemReward(wave, ref BaseGem, ref BonusGem, doVictory);
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
