using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Trajectory : MonoBehaviour
{
    [SerializeField]
    private int _pointsCountPerLenght;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        JumpController.ForceChanged += OnForceChanged;
        JumpController.JumpStarted += OnJumpStarted;
    }

    private void OnJumpStarted(float forcePercent, ISuperJump superJump)
    {
        ClearTrajectory();
    }

    private void OnForceChanged(SimpleJumpData jumpData, PlayerTransformController playerTransformController, float forcePercent)
    {
        StartCoroutine(CalculateTrajectoryAndShow(jumpData, playerTransformController, forcePercent));
    }

    private IEnumerator CalculateTrajectoryAndShow(SimpleJumpData jumpData, PlayerTransformController playerTransformController, float forcePercent)
    {
        if (forcePercent == 0)
        {
            ClearTrajectory();
            yield break;
        }

        Vector3 originPosition = playerTransformController.GetTransformPosition();
        Vector3 direction = playerTransformController.GetForwardDirection();

        float height = (jumpData.MaxHeight * forcePercent);
        float length = (jumpData.MinLength + (jumpData.MaxLength - jumpData.MinLength) * forcePercent);

        float time = jumpData.JumpCurve.keys[jumpData.JumpCurve.length - 1].time;
        int pointsCount = (int)Mathf.Round(_pointsCountPerLenght * time);


        List<Vector3> points = new List<Vector3>();

        for (int i = 1; i < pointsCount; i++)
        {
            float progress = (float)i / _pointsCountPerLenght;
            float nextHeight = height * jumpData.JumpCurve.Evaluate(progress);
            float nextLength = length * (progress);
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
        yield return new WaitForFixedUpdate();
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
        JumpController.ForceChanged -= OnForceChanged;
        JumpController.JumpStarted -= OnJumpStarted;
    }
}
