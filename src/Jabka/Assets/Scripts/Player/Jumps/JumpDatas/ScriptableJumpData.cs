using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "JumpData", menuName = "ScriptableObjects/Jumps/JumpData", order = 1)]
public class ScriptableJumpData : ScriptableObject
{
    [SerializeField]
    protected JumpData _jumpData;
    //??? ??????
    [SerializeField]
    private AnimationCurve curve;
    public JumpData GetJumpData()
    {
        return _jumpData;
    }
}

[System.Serializable]
public struct JumpData
{
    public float Height;
    public float Length;
    public float Duration;
    public AnimationCurve HeightCurve;
    public AnimationCurve LengthCurve;

    public JumpData(float height, float length, float duration, AnimationCurve heightCurve, AnimationCurve lengthCurve)
    {
        Height = height;
        Length = length;
        Duration = duration;
        HeightCurve = heightCurve;
        LengthCurve = lengthCurve;
    }
}
