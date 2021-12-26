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

    private LineRenderer _lineRenderer;

    private Action<float, ISuperJump> OnJumpStartedDelegate;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        OnJumpStartedDelegate = (num, sj) => OnJumpStarted();

        JumpController.ForceChanged += OnTrajectoryChanged;
        DashSuperJump.DashJumpPreparing += OnTrajectoryChanged;
        JumpController.JumpStarted += OnJumpStartedDelegate;
        DashSuperJump.DashJumpDashed += OnJumpStarted;
    }

    private void OnJumpStarted()
    {
        ClearTrajectory();
    }

    private void OnTrajectoryChanged(JumpData jumpData, PlayerTransformController playerTransformController)
    {
        StartCoroutine(CalculateTrajectoryAndShow(jumpData, playerTransformController));
    }

    private IEnumerator CalculateTrajectoryAndShow(JumpData jumpData, PlayerTransformController playerTransformController)
    {
        if (jumpData.ForcePercent == 0)
        {
            ClearTrajectory();
            yield break;
        }
        
        Vector3 originPosition = playerTransformController.GetTransformPosition();
        Vector3 direction = playerTransformController.GetForwardDirection();

        float time = jumpData.JumpCurve.keys.Last().time;
        int pointsCount = (int)Mathf.Round(_pointsCountPerLenght * time);


        List<Vector3> points = new List<Vector3>();

        for (int i = 1; i < pointsCount; i++)
        {
            float progress = (float)i / _pointsCountPerLenght;
            float nextHeight = jumpData.Height * jumpData.JumpCurve.Evaluate(progress);
            float nextLength = jumpData.Length * progress;
            Vector3 nextPosition = originPosition + new Vector3((direction * nextLength).x, nextHeight, (direction * nextLength).z);

            if (BaseJump.IsCollideWithSomething(nextPosition,
                playerTransformController.GetBoxColliderSize(),
                playerTransformController.GetQuaternion(),
                PlayerTransformController.playerLayerMask))
            {
                break;
            }

            points.Add(nextPosition);
        }

        ShowTrajectory(points.ToArray());
    }

    private void ShowTrajectory(Vector3[] points)
    {
        _lineRenderer.positionCount = points.Length;
        
        _lineRenderer.SetPositions(points);
    }

    private void ClearTrajectory()
    {
        _lineRenderer.positionCount = 0;
    }

    private void OnDestroy()
    {
        DashSuperJump.DashJumpPreparing -= OnTrajectoryChanged;
        JumpController.ForceChanged -= OnTrajectoryChanged;
        JumpController.JumpStarted -= OnJumpStartedDelegate;
        DashSuperJump.DashJumpDashed -= OnJumpStarted;
    }
}
