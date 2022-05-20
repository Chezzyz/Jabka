using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoSuperJumpQuest", menuName = "ScriptableObjects/Quest/DoSuperJumpQuest", order = 4)]
public class DoSuperJumpQuest : BaseQuest
{
    [SerializeField]
    private string _superJumpName;
    [SerializeField]
    private int _goalCount;
    [SerializeField]
    private bool _inARow;

    private int _currentCount = 0;

    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        JumpController.SuperJumpStarted -= OnJumpStarted;
        SimpleJump.SimpleJumpStarted -= OnSimpleJumpStarted;
        CompletePlace.LevelCompleted -= OnLevelCompleted;
        JumpController.SuperJumpStarted += OnJumpStarted;
        CompletePlace.LevelCompleted += OnLevelCompleted;
        SimpleJump.SimpleJumpStarted += OnSimpleJumpStarted;
        _currentCount = 0;
    }

    private void OnJumpStarted(ISuperJump superJump)
    {
        if(superJump != null && superJump.GetJumpName() == _superJumpName)
        {
            _currentCount++;
            if (_currentCount == _goalCount)
            {
                ReadyForComplete();
            }
        }
    }

    private void OnSimpleJumpStarted(float arg1, float arg2)
    {
        //если обычный прыжок и прыгать должны подряд 
        if (_inARow)
        {
            _currentCount = 0;
        }
    }

    protected override void OnLevelCompleted(CompletePlace completePlace)
    {
        if (completePlace.GetLevelNumber() == GetLevelNumber())
        {
            if (_isReadyForComplete)
            {
                Complete();
            }
        }
    }
}
