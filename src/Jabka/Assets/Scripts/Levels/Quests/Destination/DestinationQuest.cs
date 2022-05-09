using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestinationQuest", menuName = "ScriptableObjects/Quest/DestinationQuest", order = 3)]
public class DestinationQuest : BaseQuest
{
    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        Destination.Destinated -= OnDestinated;
        Destination.Destinated += OnDestinated;
    }

    private void OnDestinated(Destination destination)
    {
        if (destination.GetLevelNumber() == GetLevelNumber())
        {
            ReadyForComplete();
        }
    }
}
