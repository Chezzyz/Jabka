using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class LongSuperJump : BaseJump, ISuperJump
{
    [SerializeField]
    private ScriptableJumpData _jumpData;

    public static event Action<float> LongJumpStarted; //float: duration

    public override ScriptableJumpData GetJumpData()
    {
        return _jumpData;
    }

    public void SuperJump(PlayerTransformController playerTransformController)
    {
        _currentJump = StartCoroutine(JumpCoroutine(playerTransformController, _jumpData.GetJumpData()));

        LongJumpStarted?.Invoke(_jumpData.GetJumpData().Duration);
    }

    public string GetJumpName()
    {
        return "Long";
    }
}
