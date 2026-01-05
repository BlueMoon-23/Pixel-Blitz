using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PulserLaser : BaseBullets
{
    private LineRenderer lineRenderer;
    public GameObject HeadGun;
    private float TickClock = 0.1f;
    private float Tick = 0.1f;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {

    }
    // update của base bullet xóa pulserlaser nè
    private void Update()
    {
        
    }
    // Update is called once per frame
    void LateUpdate() // LateUpdate để tính toán cho chính xác
    {
        Stretch();
    }
    protected void Stretch()
    {
        lineRenderer.SetPosition(0, HeadGun.transform.position);
        if (enemy != null)
        {
            lineRenderer.SetPosition(1, enemy.Center.transform.position + new Vector3(0, 0.25f, 0));
            float Angle = Mathf.Atan2(enemy.Center.transform.position.y - transform.position.y, enemy.Center.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetDirection = Quaternion.Euler(0, 0, Angle - 90f);
            transform.rotation = targetDirection;
            // Gây damage mỗi 0.1s
            TickClock += Time.deltaTime;
            if (TickClock >= Tick)
            {
                DealDamage();
                TickClock = 0f;
            }
        }
    }
    protected void DealDamage()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 50f);
        if (enemy != null)
        {
            if (character != null)
            {
                Pulser pulser = character as Pulser;
                if (pulser != null)
                {
                    pulser.StackPulse(character.GetDamage() < enemy.GetHP() ? character.GetDamage() : enemy.GetHP());
                }
                enemy.TakeDamage(character.GetDamage(), character.canStrikethroughOrNot());
            }
        }
    }
}
