using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMetaData", menuName = "ScriptableObjects/LevelMetaData", order = 1)]
public class LevelMetaData : ScriptableObject
{
    [SerializeField]
    private int _stageNumber;
    [SerializeField]
    private int _levelNumber;
    [SerializeField]
    private string _levelName;

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
}
