using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpCountQuest", menuName = "ScriptableObjects/Quest/JumpCountQuest", order = 3)]
public class JumpCountQuest : BaseQuest
{
    [SerializeField]
    private int _goalCount;
    [SerializeField]
    private int _levelNumber;

    private int _currentCount;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        JumpController.JumpStarted -= OnJumpStarted;
        CompletePlace.LevelCompleted -= OnLevelCompleted;
        JumpController.JumpStarted += OnJumpStarted;
        CompletePlace.LevelCompleted += OnLevelCompleted;
        _currentCount = 0;
    }

    private void OnJumpStarted(float force, ISuperJump superJump)
    {
        if(_levelNumber == GetLevelNumber() && force > 0)
        {
            _currentCount++;
        }
    }

    private void OnLevelCompleted(CompletePlace completePlace)
    {
        if(completePlace.GetLevelNumber() == GetLevelNumber())
        {
            if(_currentCount <= _goalCount)
            {
                Complete();
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}