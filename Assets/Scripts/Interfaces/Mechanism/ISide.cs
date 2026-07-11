using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISide
{
    public bool IsPlayerSide() { return GetSide() != SIDE.Enemy; }
    public SIDE GetSide();
}

public enum SIDE { GroundCharacter, CliffCharacter, Dummy, Enemy }