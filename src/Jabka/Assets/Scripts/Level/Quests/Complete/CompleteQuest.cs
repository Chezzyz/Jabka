using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompleteQuest", menuName = "ScriptableObjects/Quest/CompleteQuest", order = 1)]
public class CompleteQuest : BaseQuest
{
    protected override void OnEnable()
    {
        base.OnEnable();
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(CompletePlace completePlace)
    {
        if (completePlace.GetLevelNumber() == GetLevelNumber())
        {
            Complete();
        }
    }
}
