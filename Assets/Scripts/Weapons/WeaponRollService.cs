using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponRollService : MonoBehaviour
{
    // Script chứa logic roll vũ khí. WeaponDropManager và WeaponChest có 2 thằng này
    [SerializeField] private List<WeaponProfile> WeaponDropList = new List<WeaponProfile>();
    [SerializeField] private List<float> DropRatePrefixSum = new List<float>();
    private PerkType[] PerkTypes = { PerkType.Range, PerkType.Damage, PerkType.Cooldown };
    public void InitializeWeaponDropListInGame()
    {
        // Duyệt qua danh sách character đang được sử dụng, xem vũ khí có thể drop được là vũ khí nào rồi add vào WeaponDropList
        List<CharacterInfomation> characterInformations = new List<CharacterInfomation>();
        foreach (Transform child in CharacterLoadout.instance.transform) // do khi chuyển scene, character loadout mất reference các character information, nên dựa vào các prefab ở dưới character loadout để tìm ngược lại
        {
            CharacterInfomation info = child.GetComponent<CharacterInfomation>();
            if (info != null)
            {
                characterInformations.Add(info);
            }
        }
        foreach (CharacterInfomation character in characterInformations)
        {
            foreach (WeaponData weapon in WeaponSaveManager.instance.WeaponDatabase)
            {
                if (character.characterData.characterProfile == weapon.weaponProfile.WeaponOwner)
                {
                    WeaponDropList.Add(weapon.weaponProfile);
                }
            }
        }

    }
    public void InitializeEnhancedPrefixSum()
    {

        // Sửa drop rate prefix sum sao cho
        // WeaponDropList.DropRate = { 40, 10, 50, 50, 40, 10, 10, 40, 50 }, biến đổi lại về
        // DropRate Từng vũ khí = { 40/3, 10/3, 50/3, 50/3, 40/3, 10/3, 10/3, 40/3, 50/3 }
        // DropRate Prefix sum = { 0, 40/3, 50/3, 100/3, 150/3, 190/3, 200/3, 210/3, 250/3, 300/3 } => thêm 0 ở đầu, duyệt từ 0 đến n - 1
        DropRatePrefixSum.Add(0);
        foreach (WeaponProfile weapon in WeaponDropList)
        {
            bool hasOwner = false;
            // Kiểm tra có hay chưa
            foreach (CharacterData characterData in AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters)
            {
                if (characterData.characterProfile.CharacterName == weapon.WeaponOwner.CharacterName)
                {
                    hasOwner = true;
                    break;
                }
            }
            if (hasOwner)
            {
                DropRatePrefixSum.Add(weapon.WeaponRarity.EnhancedDropChance);
            }
            else
            {
                DropRatePrefixSum.Add(0);
            }
        }
        for (int i = 0; i < DropRatePrefixSum.Count - 1; i++)
        {
            DropRatePrefixSum[i + 1] += DropRatePrefixSum[i];
        }
    }
    public void InitializeNormalPrefixSum()
    {
        DropRatePrefixSum.Add(0);
        foreach (WeaponProfile weapon in WeaponDropList)
        {
            bool hasOwner = false;
            // Kiểm tra có hay chưa
            foreach (CharacterData characterData in AccountSaveManager.CurrentAccount.userCharacterData.OwnedCharacters)
            {
                if (characterData.characterProfile.CharacterName == weapon.WeaponOwner.CharacterName)
                {
                    hasOwner = true;
                    break;
                }
            }
            if (hasOwner)
            {
                DropRatePrefixSum.Add(weapon.WeaponRarity.NormalDropChance);
            }
            else
            {
                DropRatePrefixSum.Add(0);
            }
        }
        for (int i = 0; i < DropRatePrefixSum.Count - 1; i++)
        {
            DropRatePrefixSum[i + 1] += DropRatePrefixSum[i];
        }
    }
    private int LowerBound(List<float> PrefixSum, float roll)
    {
        int left = 0;
        int right = PrefixSum.Count - 1;
        int result = 0;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (PrefixSum[mid] <= roll)
            {
                result = mid;
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }
        return Mathf.Clamp(result, 0, PrefixSum.Count - 2);
    }
    public (WeaponProfile, List<Perk>) RollWeapon()
    {
        // thưởng người chơi weapon nào đó trong weapondroplist
        // Khi roll 1 số float roll = Random.Range (0, 100); lấy từ [0, 100)
        // Duyệt xem nằm trong nửa khoảng [PrefixSum[i], PrefixSum[i+1]), nửa khoảng thứ i => vũ khí thứ i => Có thể áp Binary Search ở đây
        float roll = Random.Range(0f, DropRatePrefixSum[DropRatePrefixSum.Count - 1]);
        int index = LowerBound(DropRatePrefixSum, roll);
        WeaponProfile weaponProfile = WeaponDropList[index];
        List<Perk> PerkList = new List<Perk>();
        HashSet<PerkType> types = new HashSet<PerkType>(); // Cấu trúc set để đảm bảo các lần roll đều khác type
        for (int i = 0; i < weaponProfile.WeaponRarity.PerkCount; i++)
        {
            Perk perk = new Perk();
            PerkType type;
            do
            {
                type = PerkTypes[Random.Range(0, PerkTypes.Length)];
            }
            while (types.Contains(type));
            perk.BuffType = type;
            types.Add(type);
            perk.PerkBonus = weaponProfile.WeaponRarity.GetPerkBonus(perk.BuffType);
            PerkList.Add(perk);
        }
        PerkList.Sort((a, b) => a.BuffType.CompareTo(b.BuffType));
        return (weaponProfile, PerkList);
    }
    /// <summary>
    /// Trả về bản sao danh sách đã được Sort theo Rarity (Legendary -> Rare -> Common)
    /// mà KHÔNG làm thay đổi thứ tự mảng gốc.
    /// </summary>
    public List<WeaponProfile> GetSortedWeaponDropList()
    {
        if (WeaponDropList == null) return new List<WeaponProfile>();
        // Sort theo thứ tự legendary - rare - common
        // Tạo copy mảng chỉnh để khi sort không ảnh hưởng đến thuật toán làm việc
        return WeaponDropList
            .OrderByDescending(w => w.WeaponRarity.weaponRarity)
            .ToList(); // ToList() đảm bảo tạo ra 1 Instance List mới hoàn toàn
    }
}
