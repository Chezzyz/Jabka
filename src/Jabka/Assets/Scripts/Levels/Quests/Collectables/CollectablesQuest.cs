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

    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        _currentCount = 0;
        Collectable.Collected -= OnCollected;
        Collectable.Collected += OnCollected;
    }

    private void OnCollected(Collectable collectable)
    {
        if (collectable.GetLevelNumber() == GetLevelNumber())
        {
            _currentCount++;
            if (_currentCount == _goalCount)
            {
                ReadyForComplete();
            }
        }
    }
}
