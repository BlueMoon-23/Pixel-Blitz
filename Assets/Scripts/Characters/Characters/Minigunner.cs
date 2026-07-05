using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigunner : BaseCharacter
{
    [Header("Minigunner Clone")]
    public MinigunnerClone ClonePrefab;
    private GameObject currentClone;
    public override void SetAbilityIcon()
    {
        characterUI.AbilityCurrentIcon.sprite = characterUI.AbilityIcons[0];
        DragAbility.instance.SetDragType(DragAbility.AbilityDragType.GroundPlacement);
        DragAbility.instance.SetDragRange(ClonePrefab.profile.characterLevelDatas[4].RangeStat);
    }
    public override void Ability(Vector3 position)
    {
        if (currentClone == null)
        {
            BaseCharacter character = ClonePrefab.GetComponent<BaseCharacter>();
            if (position != Vector3.zero && !(CharacterManager.instance.hasCharacterinPosition(position)))
            {
                if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
                currentClone = Instantiate(ClonePrefab.gameObject, position, Quaternion.identity);
                CharacterManager.instance.AddPosition(position);
            }
        }
        else
        {
            CharacterManager.instance.RemovePosition(currentClone.GetComponent<BaseCharacter>().transform.position);
            Destroy(currentClone);
            if (position != Vector3.zero && !(CharacterManager.instance.hasCharacterinPosition(position)))
            {
                if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
                currentClone = Instantiate(ClonePrefab.gameObject, position, Quaternion.identity);
                CharacterManager.instance.AddPosition(position);
            }
        }
    }
    protected void OnDisable()
    {
        if (currentClone != null) 
        {
            CharacterManager.instance.RemovePosition(currentClone.GetComponent<BaseCharacter>().transform.position);
            Destroy(currentClone); 
        }
    }
}
