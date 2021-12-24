using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct JumpData
{
    //shtefan private set'ы не используются
    public float Height { get; private set; }
    public float Length { get; private set; }
    public float ForcePercent { get; private set; }
    public AnimationCurve JumpCurve { get; private set; }

    public JumpData(float height, float length, float force, AnimationCurve curve)
    {
        Height = height;
        Length = length;
        ForcePercent = force;
        JumpCurve = curve;
    }
}
