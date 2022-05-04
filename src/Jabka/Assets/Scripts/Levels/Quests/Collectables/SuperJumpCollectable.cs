using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpCollectable : BaseQuestItem
{
    private ISuperJump _superJump;

    public static event System.Action<SuperJumpCollectable> SuperJumpCollected;

    public ISuperJump GetSuperJump()
    {
        return _superJump;
    }

    protected override void SendEvent()
    {
        if (TryGetComponent(out _superJump) == false)
        {
            Debug.Log($"button {gameObject.name} doesn't have ISuperJump field");
        }
        SuperJumpCollected?.Invoke(this);
        Debug.Log($"SuperJump on level {_levelNumber} collected");
    }
}
