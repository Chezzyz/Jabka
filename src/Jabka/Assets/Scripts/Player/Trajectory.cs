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

    private void OnForceChanged(SimpleJumpData jumpData, Vector3 originPosition, Vector3 direction, float forcePercent)
    {
        Vector3[] points = new Vector3[_pointsCountPerLenght];

        float height = (jumpData.MaxHeight * forcePercent);
        float length = (jumpData.MinLength + (jumpData.MaxLength - jumpData.MinLength) * forcePercent);

        float lastKeyX = jumpData.JumpCurve.keys[jumpData.JumpCurve.length - 1].time;
        float pointsCount = _pointsCountPerLenght * lastKeyX;

        for (int i = 0; i < pointsCount; i++)
        {
            float progress = (float)i / _pointsCountPerLenght;
            float nextHeight = height * jumpData.JumpCurve.Evaluate(progress);
            float nextLength = length * (progress);
            Vector3 nextPosition = originPosition + new Vector3((direction * nextLength).x, nextHeight, (direction * nextLength).z);

            points[i] = nextPosition;
        }

        ShowTrajectory(points);
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
