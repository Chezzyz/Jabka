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

    protected bool _isReadyForComplete;

    public static event System.Action<BaseQuest> QuestCompleted;
    public static event System.Action<BaseQuest> QuestAlreadyCompleted;
    public static event System.Action<BaseQuest> IsReadyForComplete;

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
            QuestAlreadyCompleted?.Invoke(this);
            return;
        }
        SceneLoader.SceneLoaded += OnSceneLoaded;
        QuestCompleted += OnLevelCompleted;
    }

    protected virtual void OnSceneLoaded(string sceneName)
    {
        _isReadyForComplete = false;
    }

    protected int GetLevelNumber()
    {
        return int.Parse(_id.Split(':')[0]);
    }

    protected virtual void Complete()
    {
        if (!_isCompleted)
        {
            _isCompleted = true;
            QuestCompleted?.Invoke(this);
            Debug.Log($"Quest {_id} Completed");
        }
    }

    //����� ��������, �� ������� ��� �� ��������, ����� ���������� ������������ ������ �� ���������� ������
    protected virtual void ReadyForComplete()
    {
        _isReadyForComplete = true;
        IsReadyForComplete?.Invoke(this);
        Debug.Log($"Quest {_id} is ready for Complete");
    }

    protected void OnLevelCompleted(BaseQuest quest)
    {
        if(quest is CompleteQuest && quest.GetLevelNumber() == GetLevelNumber() && _isReadyForComplete)
        {
            Complete();
        }
    }

    protected virtual void OnDisable()
    {
        QuestCompleted -= OnLevelCompleted;
    }
}
