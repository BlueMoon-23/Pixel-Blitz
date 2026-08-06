using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : BaseCharacter
{
    public GameObject GraveStackBar;
    public GameObject GravePrefab;
    private GameObject currentGrave;
    private float currentGraveStack;
    [SerializeField] private float[] GraveStackByLevels;
    [SerializeField] private int[] UndeadCountByLevels;
    [SerializeField] private int[] UndeadHPByLevels;
    private float GraveStack;
    private int UndeadCount;
    private float UndeadHP;
    private float Original_x_GraveStackScale;
    public GameObject SummonVFX;
    public GameObject[] Undeads_Level_0;
    public GameObject[] Undeads_Level_2;
    public GameObject[] Undeads_Level_4;
    protected override void OnEnable()
    {
        base.OnEnable();
        currentGraveStack = 0f;
        GraveStack = GraveStackByLevels[0];
        UndeadCount = UndeadCountByLevels[0];
        UndeadHP = UndeadHPByLevels[0];
        // Grave Bar
        Original_x_GraveStackScale = 4.5f;
        // Update
        GraveStackBar.transform.localScale = new Vector3(Original_x_GraveStackScale * currentGraveStack / GraveStack, GraveStackBar.transform.localScale.y, GraveStackBar.transform.localScale.z);
    }
    public override void Upgrade()
    {
        base.Upgrade();
        GraveStack = GraveStackByLevels[Level];
        UndeadCount = UndeadCountByLevels[Level];
        UndeadHP = UndeadHPByLevels[Level];
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            base.SetUpgradeInformation();
            if (Level < profile.characterLevelDatas.Count - 1)
            {
                SetStatInfo(6, "Grave Stack", GraveStack, GraveStackByLevels[Level + 1], true);
                SetStatInfo(7, "Undead Count", UndeadCount, UndeadCountByLevels[Level + 1], true);
                SetStatInfo(8, "Undead HP", UndeadHP, UndeadHPByLevels[Level + 1], true);
            }
        }
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
            //Instantiate(Undeads_Level[random_index], currentGrave.transform.position, Quaternion.identity);
            if (SummonerUndeadPooler.instance != null)
            {
                SummonerUndead undead = SummonerUndeadPooler.instance.GetUndead(Undeads_Level[random_index].GetComponent<SummonerUndead>().ID);
                if (undead != null)
                {
                    undead.transform.position = currentGrave.transform.position;
                    undead.transform.rotation = Quaternion.identity;
                    undead.SetCharacter(this);
                }
            }
            if (SoundManager.Instance != null) SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.UndeadSummonSound);
            GameObject newSummonVFX = Instantiate(SummonVFX, currentGrave.transform.position, Quaternion.identity);
            Destroy(newSummonVFX, 1.0f);
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }
    public override void SetAbilityIcon()
    {
        if (characterUI != null) characterUI.AbilityCurrentIcon.sprite = characterUI.AbilityIcons[1];
        if (DragAbility.instance != null) DragAbility.instance.SetDragType(DragAbility.AbilityDragType.WaypointPlacement);
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
            if (SoundManager.Instance != null) SoundManager.Instance.UISource.PlayOneShot(SoundManager.Instance.Place_Sound);
            currentGrave = Instantiate(GravePrefab, position, Quaternion.identity);
        }
    }
    private bool OutOfRange(Vector3 position) 
    {
        float distance = Mathf.Sqrt(Mathf.Pow((position.x - this.transform.position.x), 2) + Mathf.Pow((position.y - this.transform.position.y), 2));
        return (distance > Range / 2f);
    }
    private void OnDisable()
    {
        Destroy(currentGrave);
    }
}
