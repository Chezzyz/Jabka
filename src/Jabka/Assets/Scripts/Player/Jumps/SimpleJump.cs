using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using Zenject;

public class SimpleJump : BaseJump
{
    [SerializeField]
    private SimpleJumpData _jumpData;

    public static event Action<float, float> SimpleJumpStarted; //float: forcePercent, duration

    public bool IsInFall { get; private set; }

    public override ScriptableJumpData GetJumpData()
    {
        return _jumpData;
    }

    public Coroutine DoSimpleJump(PlayerTransformController playerTransformController, float forcePercent)
    {
        JumpData currentJumpData = CalculateCurrentJumpData(forcePercent);
        Coroutine jump = StartCoroutine(JumpCoroutine(playerTransformController, currentJumpData));

        SimpleJumpStarted?.Invoke(forcePercent, currentJumpData.Duration);

        playerTransformController.SetIsGrounded(false);
        return jump;
    }

    public JumpData CalculateCurrentJumpData(float forcePercent)
    {
        return new JumpData(CalculateHeight(forcePercent),
            CalculateLength(forcePercent),
            CalculateDuration(forcePercent),
            GetHeightCurve(),
            GetLengthCurve());
    }

    public AnimationCurve GetHeightCurve() => _jumpData.GetJumpData().HeightCurve;
    public AnimationCurve GetLengthCurve() => _jumpData.GetJumpData().LengthCurve;
    public float CalculateDuration(float forcePercent) => _jumpData.MinDuration + (_jumpData.MaxDuration - _jumpData.MinDuration) * forcePercent;
    public float CalculateHeight(float forcePercent) => _jumpData.MaxHeight * forcePercent;
    public float CalculateLength(float forcePercent) => _jumpData.MinLength + (_jumpData.MaxLength - _jumpData.MinLength) * forcePercent;
}
