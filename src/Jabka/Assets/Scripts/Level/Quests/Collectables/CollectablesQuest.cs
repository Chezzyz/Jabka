using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CollectableQuest", menuName = "ScriptableObjects/Quest/CollectableQuest", order = 2)]
public class CollectablesQuest : BaseQuest
{
    [SerializeField]
    private int _goalCount;

    private int _currentCount;

    protected override void OnEnable()
    {
        base.OnEnable();
        Collectable.Collected += OnCollected;
        _currentCount = 0;
    }

    private void OnCollected(Collectable collectable)
    {
        if (_id == collectable.GetQuestId())
        {
            _currentCount++;
            if (_currentCount == _goalCount)
            {
                Complete();
            }
        }
    }

    private void OnDisable()
    {
        Collectable.Collected -= OnCollected;
    }
}
