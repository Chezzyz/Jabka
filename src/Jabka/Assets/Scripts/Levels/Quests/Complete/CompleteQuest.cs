using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompleteQuest", menuName = "ScriptableObjects/Quest/CompleteQuest", order = 1)]
public class CompleteQuest : BaseQuest
{
    protected override void OnEnable()
    {
        base.OnEnable();
        
    }

    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        CompletePlace.LevelCompleted -= OnLevelCompleted;
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(CompletePlace completePlace)
    {
        if (completePlace.GetLevelNumber() == GetLevelNumber())
        {
            Complete();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
