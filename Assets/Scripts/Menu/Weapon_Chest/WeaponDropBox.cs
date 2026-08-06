using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponDropBox : WeaponBoxUI
{
    public override void OnPointerUp(PointerEventData eventData)
    {
        WeaponUIControll.instance.WeaponUI.gameObject.SetActive(false);
    }
}
