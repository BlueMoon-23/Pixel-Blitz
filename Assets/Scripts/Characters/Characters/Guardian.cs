using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * guardian: tạo vòng tròn miễn khống cho đồng đội, to theo range của nó. 
 * vòng tròn sẽ hấp thụ các đòn gây khống chế
 * (vd: level 1 là 3 đòn, level 5 là 10 đòn, ...) 
 * ở level 2 cứ chịu 1 đòn sẽ nổ, choáng toàn bộ địch trong phạm vi. 
 * ở level 1 vòng tròn làm chậm 25% (pulser 20%, freezer 30% rồi). 
 * cách hồi lại vòng: mỗi cooldown giây tăng 1 sức chịu đựng lên. 
 */

public class Guardian : BaseCharacter
{
    [Header("Guardian Attributes")]
    [SerializeField] private GuardianShield ProtectingField;
    [SerializeField] private StarExplosion ExplodingEffect;
    [SerializeField] private StarExplosion LowGraphicExplodingEffect;
    [SerializeField] private GameObject ShieldBar;
    private int ExplodeID;
    private int LowGraphic_ExplodeID;
    //
    private float Original_x_ShieldBarScale;
    //
    [SerializeField] private int[] DurabilityByLevels;
    private int Durability;
    private bool DidExplode;
    protected override void OnEnable()
    {
        Original_x_ShieldBarScale = 4.5f;
        base.OnEnable();
        DidExplode = false;
        Durability = DurabilityByLevels[0];
        ExplodeID = ExplodingEffect.ExplosionID;
        LowGraphic_ExplodeID = LowGraphicExplodingEffect.ExplosionID;
        ProtectingField.transform.localScale = new Vector3(0.08f * Range, 0.08f * Range, 0.08f * Range);
        ProtectingField.guardian = this;
        ShieldBar.transform.localScale = new Vector3(Original_x_ShieldBarScale * Durability / DurabilityByLevels[0], ShieldBar.transform.localScale.y, ShieldBar.transform.localScale.z);
        StartCoroutine(ShieldCharge());
    }
    new void Update()
    {
        // k lam gi het
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            base.SetUpgradeInformation();
            if (Level < profile.characterLevelDatas.Count - 1)
            {
                SetStatInfo(6, "Durability", DurabilityByLevels[Level], DurabilityByLevels[Level + 1], true);
            }
        }
    }
    public override void Upgrade()
    {
        base.Upgrade();
        Durability = DurabilityByLevels[Level];
        ProtectingField.gameObject.SetActive(true);
        ShieldBar.transform.localScale = new Vector3(Original_x_ShieldBarScale * Durability / DurabilityByLevels[Level], ShieldBar.transform.localScale.y, ShieldBar.transform.localScale.z);
        ProtectingField.transform.localScale = new Vector3(0.08f * Range, 0.08f * Range, 0.08f * Range);
    }
    private IEnumerator ShieldCharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);
            if (Durability + 1 <= DurabilityByLevels[Level])
            {
                PlayAttackAmination(profile.AttackDuration);
                SPUM_Prefabs._anim.speed = 0.5f;
                yield return new WaitForSeconds(profile.AttackDuration);
                ProtectingField.gameObject.SetActive(true);
                if (Durability + 1 <= DurabilityByLevels[Level]) Durability++;
                ShieldBar.transform.localScale = new Vector3(Original_x_ShieldBarScale * Durability / DurabilityByLevels[Level], ShieldBar.transform.localScale.y, ShieldBar.transform.localScale.z);
                yield return new WaitForSeconds(profile.AttackDuration);
                SPUM_Prefabs._anim.speed = 1;
            }
        }
    }
    public void ShieldGetHit()
    {
        Durability--;
        if (Durability - 1 <= 0)
        {
            Durability = 0;
            ProtectingField.gameObject.SetActive(false);
        }
        ShieldBar.transform.localScale = new Vector3(Original_x_ShieldBarScale * Durability / DurabilityByLevels[Level], ShieldBar.transform.localScale.y, ShieldBar.transform.localScale.z);
        if (Level >= 3) ShieldExplode();
    }
    public void ShieldExplode()
    {
        if (!DidExplode)
        {
            int chosenID = ExplodeID;
            if (GameSetting.instance != null && !GameSetting.instance._showExplosion)
            {
                chosenID = LowGraphic_ExplodeID;
            }
            if (ExplosionPooler.instance != null)
            {
                BaseExplosion newEffect = ExplosionPooler.instance.GetExplosion(chosenID);
                if (newEffect != null)
                {
                    newEffect.transform.position = this.transform.position;
                    newEffect.transform.rotation = Quaternion.identity;
                    // Sửa scale theo 2 hiệu ứng khác nhau (bị chệch tỉ lệ scale)
                    int ratio = (chosenID == ExplodeID ? 1 : 2);
                    newEffect.transform.localScale = new Vector3(0.75f * ratio * Range, 0.75f * ratio * Range, 0.75f * ratio * Range);
                    StarExplosion starExplosion = newEffect.GetComponent<StarExplosion>();
                    if (starExplosion != null) starExplosion.Initialize(this, Damage);
                    ExplosionPooler.instance.StartCoroutine(ExplosionPooler.instance.ReturnExplosionWithDelay(newEffect, 0.5f));
                }
            }
            DidExplode = true;
            StartCoroutine(ResetExplode());
        }
    }
    private IEnumerator ResetExplode()
    {
        yield return new WaitForSeconds(0.1f);
        DidExplode = false;
    }
}
