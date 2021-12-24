using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progress", menuName = "ScriptableObjects/MetaData/Progress", order = 2)]
public class ProgressData : ScriptableObject
{
    [SerializeField]
    private int _currentStage;
    [SerializeField]
    private int _currentLevel;

    public int GetCurrentStage()
    {
        return _currentStage;
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void SetCurrentStage(int stage) 
    { 
        _currentStage = stage; 
    }

    public void SetCurrentLevel(int level) 
    { 
        _currentLevel = level; 
    }
}
