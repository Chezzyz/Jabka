using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseQuest : ScriptableObject
{
    [SerializeField]
    protected string _id;
    [SerializeField]
    protected string _questDescription;

    public static event System.Action<BaseQuest> QuestCompleted;

    protected virtual void Complete()
    {
        QuestCompleted?.Invoke(this);
        Debug.Log($"Quest {_id} Completed");
    }
}
