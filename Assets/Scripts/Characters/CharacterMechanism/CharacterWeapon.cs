using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    [field: SerializeField] public WeaponData WeaponEquipped { get; private set; }
    [SerializeField] private SpriteRenderer WeaponSprite;
    [Header("Dành cho drone và các vũ khí có animator riêng")]
    [SerializeField] private Animator WeaponAnimator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AssignWeapon(CharacterProfile profile)
    {
        // Tìm trong accounts vũ khí nào đang được sử dụng bởi character nào sẽ gán vũ khí đó vào
        if (AccountSaveManager.instance != null)
        {
            foreach (CharacterData characterData in AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters)
            {
                // Xử lý trường hợp minigunner clone. con này xài chung vũ khí với minigunner
                if (characterData.characterProfile == profile || 
                    (profile.CharacterName == "Minigunner (Clone)" && characterData.characterProfile.CharacterName == "Minigunner"))
                {
                    WeaponEquipped = characterData.WeaponEquippedData;
                    break;
                }
            }
        }
        if (WeaponEquipped != null && WeaponEquipped.weaponProfile != null)
        {
            WeaponSprite.sprite = WeaponEquipped.weaponProfile.WeaponImage;
            if (WeaponAnimator != null && WeaponEquipped.weaponProfile.WeaponAnimatorController != null)
            {
                WeaponAnimator.runtimeAnimatorController = WeaponEquipped.weaponProfile.WeaponAnimatorController;
            }
        }
    }
}
