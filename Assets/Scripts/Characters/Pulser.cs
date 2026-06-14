using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pulser : BaseCharacter
{
    public GameObject PulseBar;
    private float currentPulse = 0f;
    [SerializeField] private float[] MaxPulseByLevels;
    [SerializeField] private float[] ChargeTimeByLevels;
    private float MaxPulse;
    private bool reachedMaxPulse;
    private float ChargeTime = 4f;
    private float Original_x_PulseBarScale;
    private GameObject currentLaser;
    protected override void OnEnable()
    {
        base.OnEnable();
        currentPulse = 0f;
        ChargeTime = ChargeTimeByLevels[0];
        reachedMaxPulse = false;
        MaxPulse = MaxPulseByLevels[0];
        Original_x_PulseBarScale = 4.5f;
        // Update
        PulseBar.transform.localScale = new Vector3(Original_x_PulseBarScale * currentPulse / MaxPulse, PulseBar.transform.localScale.y, PulseBar.transform.localScale.z);
        // Instantiate cục laser và tắt nó đi. Tái chế nó
        currentLaser = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, Quaternion.identity);
        currentLaser.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (StatsReseted)
        {
            if (!isStunned && !reachedMaxPulse) 
            { 
                AttackWithoutAnimation(); 
            }
            else
            {
                currentLaser.SetActive(false);
            }
        }
    }
    public override void Upgrade()
    {
        base.Upgrade();
        MaxPulse = MaxPulseByLevels[Level];
        ChargeTime = ChargeTimeByLevels[Level];
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            base.SetUpgradeInformation();
            if (Level < profile.characterLevelDatas.Count - 1)
            {
                SetStatInfo(6, "Max Pulse", MaxPulse, MaxPulseByLevels[Level + 1]);
                SetStatInfo(7, "Charge Time", ChargeTime, ChargeTimeByLevels[Level + 1]);
            }
        }
    }
    public override void AttackWithoutAnimation()
    {
        // Chỉ instantiate cái laser khi đang đánh. khi stop attack thì destroy cái đó đi
        if (isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (range.enemies_in_range.Count != 0)
            {
                BaseEnemy first_enemy = FindFirstEnemy();
                if (first_enemy != null)
                {
                    SelfRotate(first_enemy);
                    // Bắn đạn: lưu ý là truyền góc là hướng bắn của mình luôn chứ không dùng transform.rotation hay quaternion.identity
                    float Angle_in_Radian = Mathf.Atan2(first_enemy.Center.transform.position.y - transform.position.y, first_enemy.Center.transform.position.x - transform.position.x);
                    Quaternion Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
                    // Bật laser
                    currentLaser.SetActive(true);
                    // Gán headgun, gắn enemy cho laser
                    PulserLaser pulserLaser = currentLaser.GetComponent<PulserLaser>();
                    pulserLaser.HeadGun = Bullet_StartPosition;
                    pulserLaser.SetCharacter(this);
                    pulserLaser.SetEnemy(first_enemy);
                    // Tạo hiệu ứng nổ đạn (muzzle)
                    MuzzleEffect(Angle_in_Quaternion);
                    Clock = 0f;
                }
            }
            else
            {
                Clock = Cooldown;
                if (currentLaser.activeInHierarchy) if (SoundManager.Instance != null) SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.PulserLaserEnd);
                currentLaser.SetActive(false);
            }
        }
    }
    public void StackPulse(float damage)
    {
        currentPulse += damage;
        if (currentPulse >= MaxPulse)
        {
            currentPulse = 0f;
            StartCoroutine(StopAttack());
        }
        PulseBar.transform.localScale = new Vector3(Original_x_PulseBarScale * currentPulse / MaxPulse, PulseBar.transform.localScale.y, PulseBar.transform.localScale.z);
    }
    public IEnumerator StopAttack()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.PulserLaserEnd);
        // Dừng tấn công trong 4s
        reachedMaxPulse = true;
        currentLaser.SetActive(false);
        yield return new WaitForSeconds(ChargeTime);
        reachedMaxPulse = false;
    }
    private void OnDisable()
    {
        if (currentLaser != null)
        {
            Destroy(currentLaser);
        }
    }
}
