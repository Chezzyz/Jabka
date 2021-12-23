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
    }

    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        _currentCount = 0;
        Destination.Destinated -= OnDestinated;
    }

    private void OnDestinated(Destination destination)
    {
        if (destination.GetLevelNumber() == GetLevelNumber())
        {
            _currentCount++;
            if (_currentCount == _goalCount)
            {
                ReadyForComplete();
                destination.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
