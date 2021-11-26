using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class SimpleJump : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _jumpYCurve;
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
    
    public bool IsInJump { get; private set; }

    public bool IsInFall { get; private set; }

    public void SetCurrentForcePercent(float value)
    {
        _currentForcePercent = value;
    }

    public float GetCurrentForcePercent()
    {
        return _currentForcePercent;
    }

    public void SetIsInJump(bool value)
    {
        IsInJump = value;
    }

    public Coroutine StartJump(PlayerTransformController playerTransformController, float forcePercent)
    {
        Coroutine jump = StartCoroutine(Jump(playerTransformController, forcePercent, _jumpYCurve));

        playerTransformController.SetIsGrounded(false);

        return jump;
    }

    private IEnumerator Jump(PlayerTransformController playerTransformController, float forcePercent, AnimationCurve curve)
    {
        SetIsInJump(true);
        
        float expiredTime = 0.0f;

        float duration = _minDuration + (_maxDuration - _minDuration) * forcePercent;
        float progress = expiredTime / duration;

        Vector3 originPosition = playerTransformController.GetPosition();
        Vector3 originDirection = playerTransformController.GetForwardDirection();
        
        while (progress < 1 && IsInJump) 
        {
            yield return new WaitForFixedUpdate();
            expiredTime += Time.deltaTime;
            progress = Mathf.Clamp01(expiredTime / duration);

            float currentHeight = (_maxHeight * forcePercent) * curve.Evaluate(progress);
            float currentLength = (_minLength + (_maxLength - _minLength) * forcePercent) * progress;

            playerTransformController.SetPosition(originPosition + new Vector3((originDirection * currentLength).x, currentHeight, (originDirection * currentLength).z));
        }

        SetIsInJump(false);
    }
}
