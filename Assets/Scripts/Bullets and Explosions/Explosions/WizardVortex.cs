using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class WizardVortex : MonoBehaviour
{
    private float damageInterval = 0.1f;
    private float damageValue = 50f;
    private float Clock = 0f;
    private float VortexDuration = 3.0f;
    private float DurationClock = 0f;
    /* Ý tưởng: khi enemy enter vào vortex, vortex sẽ thêm nó vào danh sách gây damage
    * Update: cộng clock, khi clock >= 0.1f thì gây 50 damage rồi đưa về 0 (giống logic cũ)
    * Khi enemy rời khỏi vortex, vortex sẽ xóa nó vào danh sách gây damage
    */
    private List<BaseEnemy> EnemiesInVortex = new List<BaseEnemy>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Clock += Time.deltaTime;
        if (Clock >= damageInterval)
        {
            foreach (BaseEnemy enemy in EnemiesInVortex)
            {
                enemy.TakeDamage(damageValue, true);
            }
            Clock = 0f;
        }
        DurationClock += Time.deltaTime;
        if (DurationClock >= VortexDuration)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            EnemiesInVortex.Add(baseEnemy);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            EnemiesInVortex.Remove(baseEnemy);
        }
    }
    /* Đoạn này không tối ưu vì hàm được gọi mỗi frame. mình muốn chính xác 0.1s mới gây damage, do vậy dùng kỹ thuật sau
    private void OnTriggerStay2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage(50f, true); // gây mỗi 0.1s
        }
    }*/
    public void ExtendDuration(float seconds)
    {
        DurationClock -= seconds;
        if (DurationClock < 0f) DurationClock = 0f;
    }
}
