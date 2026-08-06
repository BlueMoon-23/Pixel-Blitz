using System;
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
    private int CurrentPriorityIndex;
    private Dictionary<int, IAttackPriority> AttackPriorityByIndex = new Dictionary<int, IAttackPriority>()
    {
        {0, new FirstPriority() },
        {1, new LastPriority() },
        {2, new FarthestPriority() },
        {3, new ClosestPriority() },
        {4, new StrongestPriority() },
        {5, new WeakestPriority() },
        {6, new RandomPriority() },
    };
    private void Awake()
    {
        CurrentPriorityIndex = 0;
    }
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
    public string MoveAttackPriority()
    {
        CurrentPriorityIndex = (CurrentPriorityIndex + 1) % AttackPriorityByIndex.Count;
        return AttackPriorityByIndex[CurrentPriorityIndex].PriorityName;
    }
    public string GetAttackPriority()
    {
        return AttackPriorityByIndex[CurrentPriorityIndex].PriorityName;
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
        if (range.enemies_in_range.Count <= 0) return null;
        if (AttackPriorityByIndex[CurrentPriorityIndex].IsRandom)
        {
            return range.enemies_in_range[UnityEngine.Random.Range(0, range.enemies_in_range.Count)];
        }
        BaseEnemy BestEnemy = range.enemies_in_range[0];
        for (int i = 1; i < range.enemies_in_range.Count; i++)
        {
            if (range.enemies_in_range[i].isDieOrNot()) continue;
            if (range.enemies_in_range[i].isHidden && !character.hasHiddenDetectionOrNot()) continue;
            if (AttackPriorityByIndex[CurrentPriorityIndex].Priority(BestEnemy, range.enemies_in_range[i], character))
            {
                BestEnemy = range.enemies_in_range[i];
            }
        }
        Wizard wizard = character as Wizard;
        if (wizard == null)
        {
            BestEnemy.TakeIncomingDamage(character.GetDamage(), character.canStrikethroughOrNot());
        }
        return BestEnemy;
    }
    // tối ưu hóa theo bài toán TopK => Priority queue
    public List<BaseEnemy> FindThreeFirstEnemies()
    {
        IAttackPriority strategy = AttackPriorityByIndex[CurrentPriorityIndex];
        List<BaseEnemy> candidates = new List<BaseEnemy>();
        foreach (BaseEnemy enemy in range.enemies_in_range)
        {
            if (enemy.isDieOrNot()) continue;
            if (enemy.isHidden && !character.hasHiddenDetectionOrNot()) continue;
            candidates.Add(enemy);
        }
        List<BaseEnemy> Enemies_Result;
        if (strategy.IsRandom)
        {
            // Random không có khái niệm "tệ hơn/tốt hơn" => xáo trộn rồi lấy 3
            for (int i = candidates.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
            }
            Enemies_Result = candidates.GetRange(0, Mathf.Min(3, candidates.Count));
        }
        else
        {
            PriorityQueue<BaseEnemy, BaseCharacter> queue = new PriorityQueue<BaseEnemy, BaseCharacter>(strategy.Priority);
            foreach (BaseEnemy enemy in candidates)
            {
                if (queue.Count < 3)
                {
                    queue.Enqueue(enemy, character);
                }
                else if (strategy.Priority(queue.Peek(), enemy, character)) // root tệ hơn enemy => enemy tốt hơn => thay
                {
                    queue.Dequeue(character);
                    queue.Enqueue(enemy, character);
                }
            }
            Enemies_Result = new List<BaseEnemy>();
            while (queue.Count > 0)
            {
                Enemies_Result.Add(queue.Dequeue(character));
            }
        }
        Wizard wizard = character as Wizard;
        if (wizard == null)
        {
            foreach (BaseEnemy enemy in Enemies_Result)
            {
                enemy.TakeIncomingDamage(character.GetDamage(), character.canStrikethroughOrNot());
            }
        }
        return Enemies_Result;
    }
}

internal class PriorityQueue<TElement, TCharacter>
{
    // Mảng lưu các phần tử, biểu diễn dưới dạng heap
    private readonly List<TElement> items = new List<TElement>();
    // Hàm so sánh: isWorseThan(a, b) == true nghĩa là "a tệ hơn b"
    // Ví dụ với tiêu chí Closest: a tệ hơn b khi a xa hơn b
    private readonly Func<TElement, TElement, TCharacter, bool> isWorseThan;
    public PriorityQueue(Func<TElement, TElement, TCharacter, bool> isWorseThanFunc)
    {
        isWorseThan = isWorseThanFunc;
    }
    public int Count => items.Count;
    public TElement Peek()
    {
        return items[0];
    }
    public void Enqueue(TElement newItem, TCharacter character)
    {
        items.Add(newItem);
        int currentIndex = items.Count - 1;
        while (currentIndex > 0)
        {
            int parentIndex = GetParentIndex(currentIndex);
            bool currentIsWorseThanParent = isWorseThan(items[currentIndex], items[parentIndex], character);
            if (!currentIsWorseThanParent)
            {
                break;
            }
            Swap(currentIndex, parentIndex);
            currentIndex = parentIndex;
        }
    }
    public TElement Dequeue(TCharacter character)
    {
        TElement worstItem = items[0];
        int lastIndex = items.Count - 1;
        items[0] = items[lastIndex];
        items.RemoveAt(lastIndex);
        ShiftDown(0, character);
        return worstItem;
    }
    private void ShiftDown(int index, TCharacter character)
    {
        while (true)
        {
            int leftChildIndex = GetLeftChildIndex(index);
            int rightChildIndex = GetRightChildIndex(index);
            int worstIndex = index;
            if (leftChildIndex < items.Count && isWorseThan(items[leftChildIndex], items[worstIndex], character))
            {
                worstIndex = leftChildIndex;
            }
            if (rightChildIndex < items.Count && isWorseThan(items[rightChildIndex], items[worstIndex], character))
            {
                worstIndex = rightChildIndex;
            }
            if (worstIndex == index)
            {
                break;
            }
            Swap(index, worstIndex);
            index = worstIndex;
        }
    }
    private void Swap(int indexA, int indexB)
    {
        TElement temp = items[indexA];
        items[indexA] = items[indexB];
        items[indexB] = temp;
    }
    private int GetParentIndex(int index) => (index - 1) / 2;
    private int GetLeftChildIndex(int index) => index * 2 + 1;
    private int GetRightChildIndex(int index) => index * 2 + 2;
}