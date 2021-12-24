using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
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

    public static event Action<float> LongJumpStarted; //float: duration

    public void SuperJump(PlayerTransformController playerTransformController)
    {
        float maxProgress = _jumpCurve.keys.Last().time;
        AnimationCurve lengthCurve = AnimationCurve.Linear(0, 0, maxProgress, maxProgress);

        _currentJump = StartCoroutine(JumpCoroutine(playerTransformController, _duration, _height, _length, maxProgress, _jumpCurve, lengthCurve));

        LongJumpStarted?.Invoke(_duration);
    }

    public string GetJumpName()
    {
        return "Long";
    }
}
