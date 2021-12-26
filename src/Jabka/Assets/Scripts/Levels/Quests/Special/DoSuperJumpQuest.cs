using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoSuperJumpQuest", menuName = "ScriptableObjects/Quest/DoSuperJumpQuest", order = 4)]
public class DoSuperJumpQuest : BaseQuest
{
    [SerializeField]
    private string _superJumpName;

    protected override void OnSceneLoaded(string sceneName)
    {
        base.OnSceneLoaded(sceneName);
        JumpController.JumpStarted -= OnJumpStarted;
        CompletePlace.LevelCompleted -= OnLevelCompleted;
        JumpController.JumpStarted += OnJumpStarted;
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    private void OnJumpStarted(float force, ISuperJump superJump)
    {
        if(superJump != null && superJump.GetJumpName() == _superJumpName)
        {
            ReadyForComplete();
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
