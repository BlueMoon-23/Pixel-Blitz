using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    // Script quản lý cách đánh và kiểu đánh của character. Có first enemy, last enemy, ...
    // Other references
    [Header("Range")]
    public GameObject Range_Prefab;
    protected Vector3 CircleScale;
    protected RangeScript range;
    private BaseCharacter character;
    private void Start()
    {
        character = GetComponent<BaseCharacter>();
    }
    public Vector3 GetCircleScale()
    {
        return CircleScale;
    }
    public int GetEnemyCountInRange()
    {
        return range.enemies_in_range.Count;
    }
    public void ResetCharacterAttack()
    {
        CircleScale = new Vector3(0.25f, 0.25f, 0.25f);
        Range_Prefab.transform.localScale = CircleScale * character.GetRange();
        Range_Prefab.GetComponent<Renderer>().enabled = false;
        range = Range_Prefab.GetComponent<RangeScript>();
    }
    public BaseEnemy FindFirstEnemy()
    {
        int max_position = -1;
        float max_distance = 0f;
        for (int i = 0; i < range.enemies_in_range.Count; i++)
        {
            if (range.enemies_in_range[i].isDieOrNot()) continue;
            if (range.enemies_in_range[i].isHidden && !character.hasHiddenDetectionOrNot()) continue;
            if (max_distance < range.enemies_in_range[i].Distance)
            {
                max_distance = range.enemies_in_range[i].Distance;
                max_position = i;
            }
        }
        if (max_position == -1) return null;
        else
        {
            Wizard wizard = character as Wizard;
            if (wizard == null)
            {
                range.enemies_in_range[max_position].TakeIncomingDamage(character.GetDamage(), character.canStrikethroughOrNot());
            }
            return range.enemies_in_range[max_position];
        }
    }
    // tối ưu hóa theo bài toán TopK => Priority queue
    public List<BaseEnemy> FindThreeFirstEnemies()
    {
        PriorityQueue<BaseEnemy, float> queue = new PriorityQueue<BaseEnemy, float>();
        foreach (BaseEnemy enemy in range.enemies_in_range)
        {
            if (enemy.isDieOrNot()) continue;
            if (enemy.isHidden && !character.hasHiddenDetectionOrNot()) continue;
            if (queue.Count < 3)
            {
                queue.Enqueue(enemy, enemy.Distance);
            }
            else
            {
                if (enemy.Distance > queue.PeekPriority())
                {
                    queue.Dequeue();
                    queue.Enqueue(enemy, enemy.Distance);
                }
            }
        }
        List<BaseEnemy> Enemies_Result = new List<BaseEnemy>();
        while (queue.Count > 0)
        {
            BaseEnemy enemy = queue.Dequeue();
            Enemies_Result.Add(enemy);
            Wizard wizard = character as Wizard;
            if (wizard == null)
            {
                enemy.TakeIncomingDamage(character.GetDamage(), character.canStrikethroughOrNot());
            }
        }
        return Enemies_Result;
    }
}


// internal giúp class này chỉ xuất hiện trong code
internal class PriorityQueue<TElement, TPriority> where TPriority : System.IComparable<TPriority>
{
    private List<(TElement Element, TPriority Priority)> _nodes = new List<(TElement, TPriority)>();
    public int Count => _nodes.Count;
    public void Enqueue(TElement element, TPriority priority)
    {
        _nodes.Add((element, priority));
        int i = _nodes.Count - 1;
        // Shift-up
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (_nodes[i].Priority.CompareTo(_nodes[parent].Priority) >= 0) break;
            var temp = _nodes[i]; _nodes[i] = _nodes[parent]; _nodes[parent] = temp;
            i = parent;
        }
    }
    public TElement Dequeue()
    {
        var result = _nodes[0].Element;
        _nodes[0] = _nodes[_nodes.Count - 1];
        _nodes.RemoveAt(_nodes.Count - 1);
        int i = 0;
        // Heapify
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left < _nodes.Count && _nodes[left].Priority.CompareTo(_nodes[smallest].Priority) < 0) smallest = left;
            if (right < _nodes.Count && _nodes[right].Priority.CompareTo(_nodes[smallest].Priority) < 0) smallest = right;

            if (smallest == i) break;
            var temp = _nodes[i]; _nodes[i] = _nodes[smallest]; _nodes[smallest] = temp;
            i = smallest;
        }
        return result;
    }
    public TPriority PeekPriority() => _nodes[0].Priority;
}