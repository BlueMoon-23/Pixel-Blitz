using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserList
{
    public List<UserData> userDatas;
    public UserList()
    {
        userDatas = new List<UserData>();
    }
}
