using System;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpData", menuName = "ScriptableObjects/Jumps/JumpData", order = 1)]
public class ScriptableJumpData : ScriptableObject
{
    [SerializeField]
    protected JumpData _jumpData;
    
    public JumpData GetJumpData()
    {
        return _jumpData;
    }
}

[Serializable]
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
