using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigunner : BaseCharacter
{
    public GameObject ClonePrefab;
    private GameObject currentClone;
    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            if (!isStunned) { AttackWithoutAnimation(); }
            // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
        }
    }
    public override void SetAbilityIcon()
    {
        characterUI.AbilityCurrentIcon.sprite = characterUI.AbilityIcons[0];
        DragAbility.instance.currentDragType = DragAbility.AbilityDragType.GroundPlacement;
    }
    public override void Ability(Vector3 position)
    {
        if (currentClone == null)
        {
            BaseCharacter character = ClonePrefab.GetComponent<BaseCharacter>();
            if (position != Vector3.zero && !(CharacterManager.instance.hasCharacterinPosition(position)))
            {
                if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
                currentClone = Instantiate(ClonePrefab, position, Quaternion.identity);
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
                currentClone = Instantiate(ClonePrefab, position, Quaternion.identity);
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
