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
        JumpController.JumpStarted -= OnJumpStarted;
        CompletePlace.LevelCompleted -= OnLevelCompleted;
        JumpController.JumpStarted += OnJumpStarted;
        CompletePlace.LevelCompleted += OnLevelCompleted;
        _currentCount = 0;
    }

    private void OnJumpStarted(float force, ISuperJump superJump)
    {
        if(superJump != null && superJump.GetJumpName() == _superJumpName)
        {
            _currentCount++;
            if (_currentCount == _goalCount)
            {
                ReadyForComplete();
            }
        }
        //если обычный прыжок и прыгать должны подряд 
        else if (_inARow && force > 0)
        {
            _currentCount = 0;
        }
    }

    private void OnLevelCompleted(CompletePlace completePlace)
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
