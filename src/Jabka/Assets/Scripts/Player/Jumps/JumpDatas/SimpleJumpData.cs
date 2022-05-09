using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleJumpData", menuName = "ScriptableObjects/Jumps/SimpleJumpData", order = 2)]
public class SimpleJumpData : ScriptableJumpData
{
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

    private float _forcePercent;

    public void SetCurrentJumpData(JumpData jumpData)
    {
        _jumpData = jumpData;
    }

    public float MaxHeight => _maxHeight;

    public float MaxLength => _maxLength;

    public float MinLength => _minLength;

    public float MaxDuration => _maxDuration;

    public float MinDuration => _minDuration;

    public void SetForcePercent(float value)
    {
        _forcePercent = value;
    }

    public float GetForcePercent()
    {
        return _forcePercent;
    }
}
