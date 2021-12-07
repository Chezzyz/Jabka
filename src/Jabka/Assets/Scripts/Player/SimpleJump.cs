using System;
using System.Collections;
using UnityEngine;
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

    public SimpleJumpData JumpData { get; private set; }
    public bool IsInFall { get; private set; }

    private void OnEnable()
    {
        PlayerTransformController.Collided += StopCurrentJump;
        JumpData = new SimpleJumpData(_jumpCurve, _maxHeight, _minLength, _maxLength);
    }

    public Coroutine DoSimpleJump(PlayerTransformController playerTransformController, float forcePercent)
    {
        float duration = _minDuration + (_maxDuration - _minDuration) * forcePercent;
        float height = (_maxHeight * forcePercent);
        float length = (_minLength + (_maxLength - _minLength) * forcePercent);

        Coroutine jump = StartCoroutine(JumpCoroutine(playerTransformController, duration, height, length, _jumpCurve));

        playerTransformController.SetIsGrounded(false);

        return jump;
    }

    private void StopCurrentJump(Collision collision)
    {
        //избавляемся от ложных вызовов 
        if (IsInJump())
        {
            SetIsInJump(false);
        }
    }

    private void OnDisable()
    {
        PlayerTransformController.Collided -= StopCurrentJump;
    }

}

public struct SimpleJumpData
{
    public SimpleJumpData(AnimationCurve jumpCurve, float maxHeight, float minLength, float maxLength)
    {
        JumpCurve = jumpCurve;
        MaxHeight = maxHeight;
        MinLength = minLength;
        MaxLength = maxLength;
    }

    public AnimationCurve JumpCurve { get; private set; }
    public float MaxHeight { get; private set; }
    public float MaxLength { get; private set; }
    public float MinLength { get; private set; }
}

