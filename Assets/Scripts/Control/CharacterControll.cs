using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CharacterControll : MonoBehaviour
{
    private BaseCharacter character;
    private CharacterUIControll characterUI;
    void Start()
    {
        character = GetComponent<BaseCharacter>();
        characterUI = FindObjectOfType<CharacterUIControll>(true);
        character.SetUpgradeInformation();
    }
    private void OnMouseDown()
    {
        if (characterUI.gameObject.activeInHierarchy) { return; };
        character.Range_Prefab.GetComponent<Renderer>().enabled = true;
        characterUI.gameObject.SetActive(true);
        characterUI.CurrentCharacter = character;
        characterUI.CurrentCharacter.SetUpgradeInformation();
    }
}
