using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponBoxUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public WeaponRarity Rarity;
    public WeaponInformation Weapon;
    public Image WeaponImage { get; set; }
    protected RectTransform WeaponImageRectTransform;
    protected RectTransform WeaponBoxRectTransform;
    protected bool DidHoldOutside;
    protected void Awake()
    {
        WeaponImage = Weapon.GetComponent<Image>();
        WeaponImageRectTransform = Weapon.GetComponent<RectTransform>();
        WeaponBoxRectTransform = GetComponent<RectTransform>();
        UpdateWeapon();
        DidHoldOutside = false;
    }
    public void UpdateWeapon()
    {
        if (WeaponImageRectTransform != null && Weapon != null && Weapon.weaponData != null && Weapon.weaponData.weaponProfile != null)
        {
            // AnchoredPosition mới là position được hiển thị trên inspector
            WeaponImageRectTransform.anchoredPosition = Weapon.weaponData.weaponProfile.RectTransformPosition;
            WeaponImageRectTransform.rotation = Weapon.weaponData.weaponProfile.Rotation;
            WeaponImageRectTransform.localScale = Weapon.weaponData.weaponProfile.LocalScale;
        }
        if (WeaponImage != null)
        {
            if (Weapon != null && Weapon.weaponData != null && Weapon.weaponData.weaponProfile != null)
            {
                WeaponImage.sprite = Weapon.weaponData.weaponProfile.WeaponImage;
                WeaponImage.SetNativeSize();
            }
            else
            {
                WeaponImage.sprite = null;
                WeaponImage.transform.localScale = Vector3.zero;
            }
        }
    }
    /// <summary>
    /// Khi người chơi nhấn vào weapon box, ô WeaponUIControll sẽ xuất hiện
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Weapon.weaponData.weaponProfile == null) return;
        WeaponUIControll.instance.ShowWeapon(Weapon.weaponData);
        MoveByPointer(eventData);
        // Truyền data vào
    }
    /// <summary>
    /// Khi người chơi vuốt trong weapon box, ô WeaponUIControll sẽ di chuyển theo
    /// </summary>
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (Weapon.weaponData.weaponProfile == null) return;
        // Sử dụng RectangleContainsScreenPoint để kiểm tra eventData.position có nằm trong WeaponBoxRectTransform không
        bool isInsideBox = RectTransformUtility.RectangleContainsScreenPoint(
            WeaponBoxRectTransform,
            eventData.position,
            null
        );
        if (isInsideBox)
        {
            DidHoldOutside = false;
            WeaponUIControll.instance.ShowWeapon(Weapon.weaponData);
            MoveByPointer(eventData);
        }
        else
        {
            DidHoldOutside = true;
            WeaponUIControll.instance.WeaponUI.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Khi người chơi thả vuốt --> Trong weapon box --> Tắt ô WeaponUIControll đi, Hiện UI Equip
    /// --> Ngoài weapon box --> Tắt ô WeaponUIControll đi, không hiện UI Equip
    /// </summary>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (Weapon.weaponData.weaponProfile == null) return;
        WeaponUIControll.instance.WeaponUI.gameObject.SetActive(false);
        if (!DidHoldOutside)
        {
            WeaponEquip.instance.CurrentWeaponBox = this;
            WeaponEquip.instance.WeaponEquipUI.gameObject.SetActive(true);
            WeaponEquip.instance.WeaponUnequipUI.gameObject.SetActive(false);
            // Hiện thông tin giá bán
            if (Weapon != null)
            {
                WeaponEquip.instance.EquipSell.text = "Sell (+" + Weapon.weaponData.weaponProfile.WeaponRarity.SellValue + " Gems)";
            }
        }
    }
    protected void MoveByPointer(PointerEventData eventData)
    {
        // Lấy Vector2 từ eventData, chính là vị trí ngón tay của người chơi
        RectTransform CanvasRectTransform = WeaponUIControll.instance.rectTransform.parent as RectTransform;
        Vector2 LocalPosition;
        // Sau đó, sửa anchoredPosition của WeaponUIControll theo Vector2 vừa lấy
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRectTransform, eventData.position, null, out Vector2 localPosition))
        {
            // Sửa vị trí hiển thị sao cho ngược lại với phía ngón tay
            float offsetX = (localPosition.x < 0) ? 250f : -250f;
            float offsetY = (localPosition.y < 0) ? 150f : -225f;
            Vector2 targetPosition = localPosition + new Vector2(offsetX, offsetY);
            // Không để WeaponUIControll ra khỏi canvas
            float canvasHalfWidth = CanvasRectTransform.rect.width * 0.5f;
            float canvasHalfHeight = CanvasRectTransform.rect.height * 0.5f;
            float uiHalfWidth = WeaponUIControll.instance.rectTransform.rect.width * WeaponUIControll.instance.rectTransform.localScale.x * 0.5f;
            float uiHalfHeight = WeaponUIControll.instance.rectTransform.rect.height * WeaponUIControll.instance.rectTransform.localScale.y * 0.5f;
            float clampedX = Mathf.Clamp(targetPosition.x, -canvasHalfWidth + uiHalfWidth, canvasHalfWidth - uiHalfWidth);
            float clampedY = Mathf.Clamp(targetPosition.y, -canvasHalfHeight + uiHalfHeight, canvasHalfHeight - uiHalfHeight);
            WeaponUIControll.instance.rectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
        }
    }
}
