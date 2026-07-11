using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    public void SetStunImmunity() { }
    public void RemoveStunImmunity() { }
    public bool IsStunImmunity();
    public void ApplyStun(float StunDuration);
    public IEnumerator GetStunned(float StunDuration);
}
