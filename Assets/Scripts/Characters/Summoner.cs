using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : GroundCharacter
{
    public GameObject GraveStackBar;
    public GameObject GravePrefab;
    private GameObject currentGrave;
    private float currentGraveStack = 0f;
    private float GraveStack;
    private int UndeadCount;
    private float Original_x_GraveStackScale;
    public GameObject SummonVFX;
    public GameObject[] Undeads_Level_0;
    public GameObject[] Undeads_Level_2;
    public GameObject[] Undeads_Level_4;
    void Start()
    {
        Range = 13f;
        Damage = 7f;
        Cooldown = 1.75f;
        GraveStack = 50f;
        UndeadCount = 2;
        Cost = 1875;
        Level = 0;
        hasHiddenDetection = false;
        canStrikethrough = true;
        UpgradeCost = new float[] { 2350, 5600, 17700, 23500 };
        SellCost = (int)(Cost / 3);
        _hasAbility = true;
        Staff_Attack_Duration = 0.417f;
        // Grave Bar
        Original_x_GraveStackScale = GraveStackBar.transform.localScale.x;
        // Update
        GraveStackBar.transform.localScale = new Vector3(Original_x_GraveStackScale * currentGraveStack / GraveStack, GraveStackBar.transform.localScale.y, GraveStackBar.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        float min_duration = Staff_Attack_Duration < Cooldown ? Staff_Attack_Duration : Cooldown;
        if (!isStunned) { AttackWithCooldown(min_duration); }
        // Không có if này thì đạn vẫn sinh ra do lệnh tấn công ở update còn lệnh stunned là 1 lần gọi
    }
    public override float GetRange()
    {
        if (Range <= 13f) { return 13f; } // <= la chua duoc khoi tao
        else return Range;
    }
    public override float GetCost()
    {
        if (Cost != 1875) { return 1875; }
        else return Cost;
    }
    public override void UpgradeToLevel1()
    {
        Damage = 13f;
        GraveStack = 100f;
        UndeadCount = 3;
        Level = 1;
    }
    public override void UpgradeToLevel2()
    {
        Cooldown = 1.5f;
        Level = 2;
    }
    public override void UpgradeToLevel3()
    {
        Damage = 56f;
        GraveStack = 728f;
        Level = 3;
    }
    public override void UpgradeToLevel4()
    {
        Damage = 140;
        GraveStack = 5200f;
        UndeadCount = 5;
        Level = 4;
    }
    public override void SetUpgradeInformation()
    {
        characterUI.characterName.text = "Summoner";
        characterUI.characterImage.sprite = characterUI.characterImages[5]; // Copy paste nhớ chỉnh ở đây dùm con
        switch (Level)
        {
            case 0:
                {
                    characterUI.upgradeName.text = "Sinister Soul Stacker";
                    characterUI.Info1.text = "Damage: 7 => 13";
                    characterUI.Info2.text = "Grave Stack: 50 => 100";
                    characterUI.Info3.text = "Undead count: 2 => 3";
                    break;
                }
            case 1:
                {
                    characterUI.upgradeName.text = "Exotic Undead Travellers";
                    characterUI.Info1.text = "Cooldown: 1.75s => 1.5s";
                    characterUI.Info2.text = "Undead HP: 25 => 165";
                    characterUI.Info3.text = "";
                    break;
                }
            case 2:
                {
                    characterUI.upgradeName.text = "Bouncy Bullet Burial";
                    characterUI.Info1.text = "Damage: 13 => 56";
                    characterUI.Info2.text = "Grave Stack: 100 => 728";
                    characterUI.Info3.text = "Bullet now bounce behind enemies.";
                    break;
                }
            case 3:
                {
                    characterUI.upgradeName.text = "Unwrap Archaic Magic";
                    characterUI.Info1.text = "Damage: 56 => 140";
                    characterUI.Info2.text = "Grave Stack: 728 => 5200";
                    characterUI.Info3.text = "Undead count: 3 => 5\nUndead HP: 165 => 1350";
                    break;
                }
            default:
                {
                    characterUI.upgradeName.text = "";
                    characterUI.Info1.text = "";
                    characterUI.Info2.text = "";
                    characterUI.Info3.text = "";
                    break;
                }
        }
        base.SetUpgradeInformation();
    }
    public void Stack_for_Grave(float Damage)
    {
        currentGraveStack += Damage;
        if (currentGraveStack >= GraveStack)
        {
            if (currentGrave != null) {
                currentGraveStack = 0;
                if (Level < 2)
                {
                    StartCoroutine(SpawnUndead(Undeads_Level_0));
                }
                else if (Level < 4)
                {
                    StartCoroutine(SpawnUndead(Undeads_Level_2));
                }
                else
                {
                    StartCoroutine(SpawnUndead(Undeads_Level_4));
                }
            }
            else
            {
                currentGraveStack = GraveStack;
            }
        }
        GraveStackBar.transform.localScale = new Vector3(Original_x_GraveStackScale * currentGraveStack / GraveStack, GraveStackBar.transform.localScale.y, GraveStackBar.transform.localScale.z);
        if (transform.localScale.x < 0)
        {
            GraveStackBar.transform.localScale = new Vector3(-1f * Original_x_GraveStackScale * currentGraveStack / GraveStack, GraveStackBar.transform.localScale.y, GraveStackBar.transform.localScale.z);
        }
    }
    private IEnumerator SpawnUndead(GameObject[] Undeads_Level)
    {
        for (int times = 0; times < UndeadCount; times++)
        {
            int random_index = Random.Range(0, Undeads_Level.Length);
            Instantiate(Undeads_Level[random_index], currentGrave.transform.position, Quaternion.identity);
            if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.UndeadSummonSound);
            GameObject newSummonVFX = Instantiate(SummonVFX, currentGrave.transform.position, Quaternion.identity);
            Destroy(newSummonVFX, 1.0f);
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }
    public override void SetAbilityIcon()
    {
        characterUI.AbilityCurrentIcon.sprite = characterUI.AbilityIcons[1];
        DragAbility.instance.currentDragType = DragAbility.AbilityDragType.WaypointPlacement;
    }
    public override void Ability(Vector3 position)
    {
        if (position != Vector3.zero && OutOfRange(position))
        {
            if (CharacterManager.instance != null)
            {
                CharacterManager.instance.AbilityOutOfRange_Announce();
            }
        }
        else if (position != Vector3.zero && !OutOfRange(position))
        {
            if (currentGrave != null)
            {
                Destroy(currentGrave);
            }
            // Sau này cài âm thanh mọc mộ
            if (SoundManager.Instance != null) SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.Place_Sound);
            currentGrave = Instantiate(GravePrefab, position, Quaternion.identity);
        }
    }
    private bool OutOfRange(Vector3 position) 
    {
        float distance = Mathf.Sqrt(Mathf.Pow((position.x - this.transform.position.x), 2) + Mathf.Pow((position.y - this.transform.position.y), 2));
        return (distance > Range / 2f);
    }
    private void OnDestroy()
    {
        Destroy(currentGrave);
    }
}
