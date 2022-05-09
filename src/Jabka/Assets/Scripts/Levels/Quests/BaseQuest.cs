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
    public static event System.Action<BaseQuest> IsReadyForComplete;

    public string GetId()
    {
        return _id;
    }
    public int GetLevelNumber()
    {
        return int.Parse(_id.Split(':')[0]);
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
            return;
        }
        SceneLoader.SceneLoaded += OnSceneLoaded;
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    protected virtual void OnSceneLoaded(string sceneName)
    {
        _isReadyForComplete = false;
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

    //квест выполнен, но уровень еще не закончен, квест выполнится окончательно только по завершению уровня
    protected virtual void ReadyForComplete()
    {
        _isReadyForComplete = true;
        IsReadyForComplete?.Invoke(this);
        Debug.Log($"Quest {_id} is ready for Complete");
    }

    protected virtual void OnLevelCompleted(CompletePlace place)
    {
        if(place.GetLevelNumber() == GetLevelNumber() && _isReadyForComplete)
        {
            Complete();
        }
    }

    protected virtual void OnDisable()
    {
        CompletePlace.LevelCompleted -= OnLevelCompleted;
        SceneLoader.SceneLoaded -= OnSceneLoaded;
    }
}
