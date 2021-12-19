using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePlace : BaseQuestItem
{
    public static event System.Action<CompletePlace> LevelCompleted;

    protected override void SendEvent()
    {
        LevelCompleted?.Invoke(this);
        Debug.Log($"Level for quest {_questId} completed");
    }
}
