using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserCodeData
{
    public string Code;
    public bool hasRedeemed;
    public UserCodeData(string code = "")
    {
        Code = code;
        hasRedeemed = true;
    }
}
