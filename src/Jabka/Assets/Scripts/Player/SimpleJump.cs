using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class SimpleJump : AbstractJump
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

    private float _currentForcePercent;

    public bool IsInFall { get; private set; }

    private void OnEnable()
    {
        PlayerTransformController.Collided += StopCurrentJump;
    }

    public void SetCurrentForcePercent(float value)
    {
        _currentForcePercent = value;
    }

    public float GetCurrentForcePercent()
    {
        return _currentForcePercent;
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
