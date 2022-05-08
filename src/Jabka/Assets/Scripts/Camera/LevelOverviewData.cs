using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverviewData", menuName = "ScriptableObjects/Level Overview Data", order = 1)]
public class LevelOverviewData : ScriptableObject
{
    [SerializeField]
    private List<AnimationCurve> _cameraSpeedCurves;
}
