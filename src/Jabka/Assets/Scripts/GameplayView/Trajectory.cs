using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Trajectory : MonoBehaviour
{
    [SerializeField]
    private int _pointsCountPerLenght;
    [SerializeField]
    private Material _simpleJumpMaterial;
    [SerializeField]
    private Material _dashJumpMaterial;

    private LineRenderer _lineRenderer;

    private Action<ScriptableJumpData, PlayerTransformController> OnForceChangedDelegate;
    private Action<ScriptableJumpData, PlayerTransformController> OnDashPreparingDelegate;
    private Action<float, ISuperJump> OnJumpStartedDelegate;
    private Action<float> OnDashJumpDashed;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        OnJumpStartedDelegate = (num, sj) => OnJumpStarted();
        OnDashJumpDashed = (duration) => OnJumpStarted();
        OnForceChangedDelegate = (data, ctrl) => OnTrajectoryChanged(data, ctrl, _simpleJumpMaterial);
        OnDashPreparingDelegate = (data, ctrl) => OnTrajectoryChanged(data, ctrl, _dashJumpMaterial);

        JumpController.ForceChanged += OnForceChangedDelegate;
        DashSuperJump.DashJumpPreparing += OnDashPreparingDelegate;
        JumpController.JumpStarted += OnJumpStartedDelegate;
        DashSuperJump.DashJumpDashed += OnDashJumpDashed;
    }

    private void OnJumpStarted()
    {
        ClearTrajectory();
    }

    private void OnTrajectoryChanged(ScriptableJumpData jumpData, PlayerTransformController playerTransformController, Material trajectoryMaterial)
    {
        ShowTrajectory(CalculateTrajectory(jumpData, playerTransformController).ToArray(), trajectoryMaterial);
    }

    private List<Vector3> CalculateTrajectory(ScriptableJumpData scriptableJumpData, PlayerTransformController playerTransformController)
    {
        if (scriptableJumpData is SimpleJumpData data && data.GetForcePercent() == 0)
        {
            ClearTrajectory();
            return new List<Vector3>();
        }
        
        Vector3 originPosition = playerTransformController.GetTransformPosition();
        Vector3 direction = playerTransformController.GetForwardDirection();

        JumpData jumpData = scriptableJumpData.GetJumpData();
        float time = jumpData.HeightCurve.keys.Last().time;
        int pointsCount = (int)Mathf.Round(_pointsCountPerLenght * time);


        List<Vector3> points = new List<Vector3>();

        bool isCollided = false;
        int pointsAfterCollided = 5;

        for (int i = 1; i < pointsCount; i++)
        {
            if (isCollided && pointsAfterCollided > 0)
            {
                pointsAfterCollided--;
                if (pointsAfterCollided == 0)
                {
                    break;
                }
            }
            float progress = (float)i / _pointsCountPerLenght;
            float nextHeight = jumpData.Height * jumpData.HeightCurve.Evaluate(progress);
            float nextLength = jumpData.Length * progress;
            Vector3 nextPosition = originPosition + new Vector3((direction * nextLength).x, nextHeight, (direction * nextLength).z);

            if (BaseJump.IsCollideWithSomething(nextPosition,
                playerTransformController.GetBoxColliderSize(),
                playerTransformController.GetQuaternion(),
                PlayerTransformController.playerLayerMask))
            {
                isCollided = true;
            }

            points.Add(nextPosition);
        }

        return points;
    }

    private void ShowTrajectory(Vector3[] points, Material material)
    {
        _lineRenderer.material = material;
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }

    private void ClearTrajectory()
    {
        _lineRenderer.positionCount = 0;
    }

    private void OnDestroy()
    {
        JumpController.ForceChanged -= OnForceChangedDelegate;
        DashSuperJump.DashJumpPreparing -= OnDashPreparingDelegate;
        JumpController.JumpStarted -= OnJumpStartedDelegate;
        DashSuperJump.DashJumpDashed -= OnDashJumpDashed;
    }
}
