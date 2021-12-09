using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSuperJump : BaseJump, ISuperJump
{
    [SerializeField]
    private AnimationCurve _jumpCurve;
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _height;
    [SerializeField]
    private float _duration;

    public void SuperJump(PlayerTransformController playerTransformController)
    {
        _currentJump = StartCoroutine(JumpCoroutine(playerTransformController, _duration, _height, _length, _jumpCurve));
    }

    public string GetJumpName()
    {
        return "Long";
    }
}
