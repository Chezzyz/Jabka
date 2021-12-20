using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestinationQuest", menuName = "ScriptableObjects/Quest/DestinationQuest", order = 3)]
public class DestinationQuest : BaseQuest
{
    [SerializeField]
    private int _goalCount;

    private int _currentCount;

    protected override void OnEnable()
    {
        base.OnEnable();
        Destination.Destinated += OnDestinated;
        _currentCount = 0;
    }

    private void OnDestinated(Destination destination)
    {
        if (_id == destination.GetQuestId())
        {
            _currentCount++;
            if (_currentCount == _goalCount)
            {
                Complete();
                destination.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        Destination.Destinated += OnDestinated;
    }
}
