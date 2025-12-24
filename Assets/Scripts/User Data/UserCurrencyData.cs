using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserCurrencyData
{
    // Class lưu data tiền tệ của người chơi
    public int UserGems;
    public int UserDiamonds;
    public UserCurrencyData(int gem = 0, int diamond = 0)
    {
        UserGems = gem;
        UserDiamonds = diamond;
    }
}
