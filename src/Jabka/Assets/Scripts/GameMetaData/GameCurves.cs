using UnityEngine;

[CreateAssetMenu(fileName = "GameCurves", menuName = "ScriptableObjects/GameCurves", order = 1)]
public class GameCurves : ScriptableObject
{
    [SerializeField]
    private AnimationCurve _simpleJumpCurve;

    [SerializeField]
    private AnimationCurve _longJumpCurve;

    [SerializeField]
    private AnimationCurve _dashSimplePartJumpCurve;
    [SerializeField]
    private AnimationCurve _dashDashPartLengthJumpCurve;

    [SerializeField]
    private AnimationCurve _dashDashPartHeightJumpCurve;
}
