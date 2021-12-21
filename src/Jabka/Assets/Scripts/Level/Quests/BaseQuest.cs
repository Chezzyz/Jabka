using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseQuest : ScriptableObject
{
    [SerializeField]
    protected string _id;
    [SerializeField]
    protected string _questDescription;
    [SerializeField]
    protected bool _isCompleted = false;

    public static event System.Action<BaseQuest> QuestCompleted;

    public string GetId()
    {
        return _id;
    }

    public string GetDescription()
    {
        return _questDescription;
    }

    public bool IsCompleted()
    {
        return _isCompleted;
    }

    protected virtual void OnEnable()
    {
        if (_isCompleted)
        {
            QuestCompleted?.Invoke(this);
        }
    }

    protected int GetLevelNumber()
    {
        return int.Parse(_id.Split(':')[0]);
    }

    protected virtual void Complete()
    {
        _isCompleted = true;
        QuestCompleted?.Invoke(this);
        Debug.Log($"Quest {_id} Completed");
    }
}
