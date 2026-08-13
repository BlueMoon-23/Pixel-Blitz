using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pulser : BaseCharacter
{
    public GameObject PulseBar;
    private float currentPulse = 0f;
    [SerializeField] private float[] MaxPulseByLevels;
    private float MaxPulse;
    private bool reachedMaxPulse;
    private float Original_x_PulseBarScale;
    private GameObject currentLaser;
    private PulserLaser currentPulserLaser;
    protected override void OnEnable()
    {
        base.OnEnable();
        currentPulse = 0f;
        reachedMaxPulse = false;
        MaxPulse = MaxPulseByLevels[0];
        Original_x_PulseBarScale = 4.5f;
        // Update
        PulseBar.transform.localScale = new Vector3(Original_x_PulseBarScale * currentPulse / MaxPulse, PulseBar.transform.localScale.y, PulseBar.transform.localScale.z);
        // Instantiate cục laser và tắt nó đi. Tái chế nó
        currentLaser = Instantiate(bullet_Prefab, Bullet_StartPosition.transform.position, Quaternion.identity);
        currentPulserLaser = currentLaser.GetComponent<PulserLaser>();
        currentPulserLaser.SetCharacter(this);
        currentLaser.SetActive(false);
    }

    // Update is called once per frame
    new void Update()
    {
        if (StatsReseted)
        {
            if (!characterEffect.isStunned) 
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
    }
    public override void SetUpgradeInformation()
    {
        if (characterUI != null)
        {
            base.SetUpgradeInformation();
            if (Level < profile.characterLevelDatas.Count - 1)
            {
                SetStatInfo(6, "Max Pulse", MaxPulse, MaxPulseByLevels[Level + 1], true);
            }
        }
    }
    public override void AttackWithoutAnimation()
    {
        // Chỉ instantiate cái laser khi đang đánh. khi stop attack thì destroy cái đó đi
        if (characterEffect.isStunned) { return; }
        Clock += Time.deltaTime;
        if (Clock >= Cooldown)
        {
            if (characterAttack.GetEnemyCountInRange() != 0)
            {
                BaseEnemy first_enemy = characterAttack.FindFirstEnemy();
                if (first_enemy != null)
                {
                    SelfRotate(first_enemy);
                    // Bắn đạn: lưu ý là truyền góc là hướng bắn của mình luôn chứ không dùng transform.rotation hay quaternion.identity
                    float Angle_in_Radian = Mathf.Atan2(first_enemy.Center.transform.position.y - transform.position.y, first_enemy.Center.transform.position.x - transform.position.x);
                    Quaternion Angle_in_Quaternion = Quaternion.Euler(0, 0, Angle_in_Radian * Mathf.Rad2Deg - 90f);
                    // Bật laser
                    currentLaser.SetActive(true);
                    // Gán headgun, gắn enemy cho laser
                    currentPulserLaser.HeadGun = Bullet_StartPosition;                    
                    currentPulserLaser.SetEnemy(first_enemy);
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
    /// <summary>
    /// Trả về false nếu đang bắn laser thường, true nếu đang bắn bomb
    /// </summary>
    public bool isReachingMaxPulse()
    {
        return reachedMaxPulse;
    }
    public void StackPulse(float damage)
    {
        currentPulse += damage;
        if (currentPulse >= MaxPulse)
        {
            currentPulse = MaxPulse;
            if (SoundManager.Instance != null) SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.PulserLaserEnd);
            reachedMaxPulse = true;
        }
        PulseBar.transform.localScale = new Vector3(Original_x_PulseBarScale * currentPulse / MaxPulse, PulseBar.transform.localScale.y, PulseBar.transform.localScale.z);
    }
    public void DrainPulse(float damage)
    {
        currentPulse -= damage;
        if (currentPulse <= 0)
        {
            currentPulse = 0;
            if (SoundManager.Instance != null) SoundManager.Instance.SoundEffectSource.PlayOneShot(SoundManager.Instance.PulserLaserEnd);
            reachedMaxPulse = false;
        }
        PulseBar.transform.localScale = new Vector3(Original_x_PulseBarScale * currentPulse / MaxPulse, PulseBar.transform.localScale.y, PulseBar.transform.localScale.z);
    }
    private void OnDisable()
    {
        if (currentLaser != null)
        {
            Destroy(currentLaser);
        }
    }
}
