using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public class DragCharacter : DragThing
{
    private void Start()
    {
        baseCharacter = CharacterPrefab.GetComponent<BaseCharacter>();
        InitPlacing();
    }
    public void SetCharacterPrefab(GameObject character)
    {
        CharacterPrefab = character;
    }
    protected override void OnPointerDown_Specific(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        m_RectTransform.parent as RectTransform,
        eventData.position,
        eventData.pressEventCamera,
        out offset
    );
        offset = m_RectTransform.anchoredPosition - offset;
        if (CharacterUIControll.instance != null)
        {
            CharacterUIControll.instance.UI_Off();
        }
    }
    protected override void OnBeginDrag_Specific(PointerEventData eventData) 
    {
        canvasGroup.blocksRaycasts = false;
        // Placing
        if (baseCharacter.profile.isCliff)
        {
            PlacingCliffUI.SetActive(true);
        }
        else
        {
            PlacingGroundUI.SetActive(true);
        }
        CancelPlacing.SetActive(true);
        // Range
        RangeUI.SetActive(true);
        RangeUI.transform.DOScale(new Vector3(RangeUI.transform.localScale.x * baseCharacter.GetRange(), RangeUI.transform.localScale.y * baseCharacter.GetRange(), RangeUI.transform.localScale.z * baseCharacter.GetRange()), 0.05f).From(0f);
        //RangeUI.transform.localScale = new Vector3(RangeUI.transform.localScale.x * baseCharacter.GetRange(), RangeUI.transform.localScale.y * baseCharacter.GetRange(), RangeUI.transform.localScale.z * baseCharacter.GetRange());
        range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
    }
    protected override void OnEndDrag_Specific(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        PlacingGroundUI.SetActive(false);
        PlacingCliffUI.SetActive(false);
        RangeUI.SetActive(false);
        RangeUI.transform.localScale = new Vector3(2 / 3f, 2 / 3f, 2 / 3f);
        CancelPlacing.SetActive(false);
        GameObject cancelPlacing = eventData.pointerCurrentRaycast.gameObject;
        if (cancelPlacing.CompareTag("CancelPlacing"))
        {
            m_RectTransform.anchoredPosition = previous_RectTransform;
            range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
            return;
        }
        // Kiem tra xem co du tien de dat character khong
        BaseCharacter character = CharacterPrefab.GetComponent<BaseCharacter>();
        if (EconomyManager.instance != null && character.GetCost() <= EconomyManager.instance.PlayerCoin)
        {
            if (CharacterManager.instance != null)
            {
                if (CharacterManager.instance.GetPopulation() < 20)
                {
                    Vector3 dropPosition = GetDropPosition(eventData.position).Position;
                    Tilemap cliffTilemap = GetDropPosition(eventData.position).CliffTilemap;
                    if (dropPosition != Vector3.zero && !(CharacterManager.instance.hasCharacterinPosition(dropPosition)))
                    {
                        if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
                        if (CharacterManager.instance != null)
                        {
                            //GameObject newCharacter = Instantiate(CharacterPrefab, GetDropPosition(eventData.position), Quaternion.identity);
                            BaseCharacter newCharacter = CharacterManager.instance.GetCharacter(CharacterPrefab.GetComponent<BaseCharacter>());
                            if (newCharacter != null)
                            {
                                newCharacter.transform.position = dropPosition;
                                newCharacter.transform.rotation = Quaternion.identity;
                                newCharacter.gameObject.SetActive(true);
                                CharacterManager.instance.AddCharacterWithPosition(newCharacter, dropPosition);
                                // Sửa tọa độ y của range character theo ý muốn của PlacingCliff
                                // tránh trường hợp return pool xong range của character bị sửa vĩnh viễn
                                newCharacter.characterAttack.Range_Prefab.transform.localPosition = Vector3.zero;
                                if (cliffTilemap != null)
                                {
                                    RangeProjection rangeProjection = cliffTilemap.GetComponent<RangeProjection>();
                                    if (rangeProjection != null && newCharacter.profile.isCliff)
                                    {
                                        newCharacter.characterAttack.Range_Prefab.transform.localPosition += new Vector3(0, rangeProjection.Adjusted_Y_Position * 1.33f, 0);
                                    }
                                }
                            }
                        }
                        EconomyManager.instance.Purchase(character.GetCost());
                        EconomyManager.instance.Change_CurrentCoin();
                    }
                }
                else
                {
                    CharacterManager.instance.LimitPlacement_Announce();
                }
            }
        }
        else
        {
            EconomyManager.instance.Announce_CantPlace(character.GetCost());
        }
        m_RectTransform.anchoredPosition = previous_RectTransform;
        range_RectTransform.anchoredPosition = m_RectTransform.anchoredPosition - new Vector2(0f, 30f);
    }
}
