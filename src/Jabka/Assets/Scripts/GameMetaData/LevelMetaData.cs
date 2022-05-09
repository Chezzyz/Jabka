using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelMetaData", menuName = "ScriptableObjects/MetaData/LevelMetaData", order = 1)]
public class LevelMetaData : ScriptableObject
{
    [SerializeField]
    private int _stageNumber;
    [SerializeField]
    private int _levelNumber;
    [SerializeField]
    private string _levelName;
    [SerializeField]
    private string _scene;
    [SerializeField]
    private List<BaseQuest> _levelQuests;

    public int GetStageNumber()
    {
        return _stageNumber;
    }

    public int GetLevelNumber()
    {
        return _levelNumber;
    }

    public string GetLevelName() 
    {
        return _levelName;
    }

    public string GetSceneName()
    {
        return _scene;
    }

    public BaseQuest GetQuest(int questNumber)
    {
        if(_levelQuests.Count < questNumber)
        {
            return null;
        }
        return _levelQuests[questNumber - 1];
    }
}
