using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoadout : MonoBehaviour
{
    public static CharacterLoadout instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public List<CharacterInfomation> characterLoadout = new List<CharacterInfomation>();
    public GameObject[] baseCharacters;
    public void GetCharacterLoadout()
    {
        if (CharacterEquip.instance != null)
        {
            characterLoadout = CharacterEquip.instance.characterLoadout;
        }
    }
    public void Set_CharacterLoadout_Prefab()
    {
        GetCharacterLoadout();
        for (int i = 0; i < characterLoadout.Count; i++)
        {
            for (int j = 0; j < baseCharacters.Length; j++)
            {
                if (characterLoadout[i].name == baseCharacters[j].name)
                {
                    GameObject newCharacter = Instantiate(baseCharacters[j], transform.position, Quaternion.identity);
                    newCharacter.transform.SetParent(transform, false);
                    // THÀNH PHẦN RANGE CÓ LOCALSCALE BỊ BIẾN ĐỔI TỰ ĐỘNG, GIỜ CÀI LẠI
                    Transform range = newCharacter.transform.Find("Range");
                    if (range != null)
                    {
                        range.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    }
                    CharacterInfomation characterData = newCharacter.AddComponent<CharacterInfomation>();
                    // CharacterData là 1 con trỏ, không ghi characterData = CharacterLoadout[i] được
                    characterData.characterData = characterLoadout[i].characterData;
                    newCharacter.SetActive(false);
                }
            }
        }
    }
}
