using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScript : MonoBehaviour
{
    [SerializeField] private BaseCharacter character;
    public List<BaseEnemy> enemies_in_range;
    void Start()
    {
        enemies_in_range = new List<BaseEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            if (enemy.isHiddenOrNot() && !character.hasHiddenDetectionOrNot())
            {

            }
            else
            {
                enemies_in_range.Add(enemy);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            if (enemy.isHiddenOrNot() && !character.hasHiddenDetectionOrNot())
            {

            }
            else
            {
                enemies_in_range.Remove(enemy);
            }
        }
    }
}
