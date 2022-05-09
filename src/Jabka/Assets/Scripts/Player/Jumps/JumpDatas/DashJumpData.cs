using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashJumpData", menuName = "ScriptableObjects/Jumps/DashJumpData", order = 3)]
public class DashJumpData : ScriptableJumpData
{
    [SerializeField]
    private AnimationCurve _dashHeightCurve;
    [SerializeField]
    private AnimationCurve _dashLengthCurve;
    [SerializeField]
    private float _dashLength;
    [SerializeField]
    private float _dashHeight;
    [SerializeField]
    private float _dashDuration;
    [SerializeField]
    private float _timeScale;
    [SerializeField]
    private float _slowMoDuration;

    public AnimationCurve DashHeightCurve => _dashHeightCurve;

    public AnimationCurve DashLengthCurve => _dashLengthCurve;

    public float DashLength => _dashLength;

    public float DashHeight => _dashHeight;

    public float DashDuration => _dashDuration;

    public float TimeScale => _timeScale;

    public float SlowMoDuration => _slowMoDuration;
}
