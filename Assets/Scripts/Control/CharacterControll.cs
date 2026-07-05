using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
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
        //if (characterUI.gameObject.activeInHierarchy) { return; };
        // Tạo data giả lập
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        foreach (var r in results)
        {
            if (r.gameObject.GetComponentInParent<Button>() != null)
            {
                return;
            }
        }
        GameObject[] Range_Prefab = GameObject.FindGameObjectsWithTag("Range");
        for (int i = 0; i < Range_Prefab.Length; i++)
        {
            Range_Prefab[i].GetComponent<Renderer>().enabled = false;
        }
        character.characterAttack.Range_Prefab.GetComponent<Renderer>().enabled = true;
        character.characterAttack.Range_Prefab.transform.DOScale(character.characterAttack.Range_Prefab.transform.localScale, 0.05f).From(0f);
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
        if (characterUI.CurrentCharacter.GetLevel() >= 4 || characterUI.CurrentCharacter.GetType() == typeof(MinigunnerClone))
        {
            characterUI.UpgradeButton.interactable = false;
        }
        else
        {
            characterUI.UpgradeButton.interactable = true;
        }
        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.ClickOnCharacter_Sound);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(characterUI.UpgradeContent);
    }
}
