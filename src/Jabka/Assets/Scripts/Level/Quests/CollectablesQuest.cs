using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CollectableQuest", menuName = "ScriptableObjects/Quest/CollectableQuest", order = 1)]
public class CollectablesQuest : BaseQuest
{
    [SerializeField]
    private int _goalCount;

    private CollectableCountHandler _collectableCountHandler;

    private void OnEnable()
    {
        Collectable.Collected += OnCollected;
        _collectableCountHandler = FindObjectOfType<CollectableCountHandler>();
        var gavno = FindObjectOfType<Canvas>();
    }

    private void OnCollected(Collectable collectable)
    {
        if(_collectableCountHandler.GetCurrentCountOfCollectables() == _goalCount)
        {
            Complete();
        }
    }

    private void OnDisable()
    {
        Collectable.Collected -= OnCollected;
    }
}
