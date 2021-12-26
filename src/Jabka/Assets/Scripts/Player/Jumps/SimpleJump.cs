using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using Zenject;

public class SimpleJump : BaseJump
{
    [SerializeField]
    private AnimationCurve _jumpCurve;
    [SerializeField]
    private float _maxHeight;
    [SerializeField]
    private float _maxLength;
    [SerializeField]
    private float _minLength;
    [SerializeField]
    private float _maxDuration;
    [SerializeField]
    private float _minDuration;

    public bool IsInFall { get; private set; }

    public Coroutine DoSimpleJump(PlayerTransformController playerTransformController, float forcePercent)
    {
        float duration = CalculateDuration(forcePercent);
        float height = CalculateHeight(forcePercent);
        float length = CalculateLength(forcePercent);
        float maxProgress = _jumpCurve.keys.Last().time;
        AnimationCurve lengthCurve = AnimationCurve.Linear(0, 0, maxProgress, maxProgress);

        Coroutine jump = StartCoroutine(JumpCoroutine(playerTransformController, duration, height, length, maxProgress, _jumpCurve, lengthCurve));

        playerTransformController.SetIsGrounded(false);
        return jump;
    }

    public AnimationCurve GetAnimationCurve() => _jumpCurve;

    public float CalculateDuration(float forcePercent) => _minDuration + (_maxDuration - _minDuration) * forcePercent;
    public float CalculateHeight(float forcePercent) => _maxHeight * forcePercent;
    public float CalculateLength(float forcePercent) => _minLength + (_maxLength - _minLength) * forcePercent;
}
