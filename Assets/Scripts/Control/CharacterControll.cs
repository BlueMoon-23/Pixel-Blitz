using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CharacterControll : MonoBehaviour
{
    // Script này là cơ chế nhấn vào character và hiện lên character range và characterUI
    private BaseCharacter character;
    private CharacterUIControll characterUI;
    private DragAbility abilityIcon;
    void Start()
    {
        character = GetComponent<BaseCharacter>();
        characterUI = FindObjectOfType<CharacterUIControll>(true);
        abilityIcon = characterUI.AbilityCurrentIcon.GetComponent<DragAbility>();
        character.SetUpgradeInformation();
        if (character.hasAbility)
        {
            characterUI.AbilityButton.gameObject.SetActive(true);
            character.SetAbilityIcon();
            abilityIcon.SetCurrentCharacter(character);
        }
        else
        {
            characterUI.AbilityButton.gameObject.SetActive(false);
        }
    }
    private void OnMouseDown()
    {
        if (characterUI.gameObject.activeInHierarchy) { return; };
        character.Range_Prefab.GetComponent<Renderer>().enabled = true;
        characterUI.gameObject.SetActive(true);
        characterUI.CurrentCharacter = character;
        characterUI.CurrentCharacter.SetUpgradeInformation();
        if (characterUI.CurrentCharacter.hasAbility)
        {
            characterUI.AbilityButton.gameObject.SetActive(true);
            characterUI.CurrentCharacter.SetAbilityIcon();
            abilityIcon.SetCurrentCharacter(character);
        }
        else
        {
            characterUI.AbilityButton.gameObject.SetActive(false);
        }
    }
}
