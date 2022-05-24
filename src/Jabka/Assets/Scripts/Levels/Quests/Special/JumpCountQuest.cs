using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpCountQuest", menuName = "ScriptableObjects/Quest/JumpCountQuest", order = 3)]
public class JumpCountQuest : BaseQuest
{
    [SerializeField]
    private int _goalCount;

    private int _currentCount;

    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        SimpleJump.SimpleJumpStarted -= OnSimpleJumpStarted;
        JumpController.SuperJumpStarted -= OnSuperJumpStarted;
        CompletePlace.LevelCompleted -= OnLevelCompleted;

        SimpleJump.SimpleJumpStarted += OnSimpleJumpStarted;
        JumpController.SuperJumpStarted += OnSuperJumpStarted;
        CompletePlace.LevelCompleted += OnLevelCompleted;
        _currentCount = 0;
    }

    private void OnSimpleJumpStarted(float arg1, float arg2)
    {
        if (SceneStatus.GetCurrentLevelNumber() == GetLevelNumber())
        {
            _currentCount++;
        }
    }

    private void OnSuperJumpStarted(ISuperJump superJump)
    {
        if (SceneStatus.GetCurrentLevelNumber() == GetLevelNumber())
        {
            _currentCount++;
        }
    }

    protected override void OnLevelCompleted(CompletePlace completePlace)
    {
        if(SceneStatus.GetCurrentLevelNumber() == GetLevelNumber())
        {
            if(_currentCount <= _goalCount)
            {
                Complete();
            }
        }
    }
}
