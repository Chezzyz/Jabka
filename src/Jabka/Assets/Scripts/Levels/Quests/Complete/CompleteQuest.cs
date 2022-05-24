using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompleteQuest", menuName = "ScriptableObjects/Quest/CompleteQuest", order = 1)]
public class CompleteQuest : BaseQuest
{
    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        CompletePlace.LevelCompleted -= OnLevelCompleted;
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    protected override void OnLevelCompleted(CompletePlace completePlace)
    {
        base.OnLevelCompleted(completePlace);
        if (SceneStatus.GetCurrentLevelNumber() == GetLevelNumber())
        {
            Complete();
        }
    }
}
