using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField]
    private int _pointsCountPerLenght;
    [SerializeField]
    private int _pointAfterCollisionCount;
    [SerializeField]
    private GameObject _collisionPointPrefab;

    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private LineRenderer _dashLineRenderer;

    private void OnEnable()
    {
        JumpController.ForceChanged += OnTrajectoryChanged;
        DashSuperJump.DashJumpPreparing += OnDashTrajectoryChanged;
        DashSuperJump.DashPreparingEnded += ClearTrajectory;
        JumpController.JumpStarted += OnJumpStarted;
        DashSuperJump.DashJumpDashed += OnJumpStarted;
    }

    private void OnTrajectoryChanged(ScriptableJumpData jumpData, PlayerTransformController playerTransformController)
    {
        ShowTrajectory(_lineRenderer, CalculateTrajectory(jumpData, playerTransformController).ToArray());
    }

    private void OnDashTrajectoryChanged(ScriptableJumpData jumpData, PlayerTransformController playerTransformController)
    {
        ShowTrajectory(_dashLineRenderer, CalculateTrajectory(jumpData, playerTransformController).ToArray());
    }

    private List<Vector3> CalculateTrajectory(ScriptableJumpData scriptableJumpData, PlayerTransformController playerTransformController)
    {
        JumpData jumpData = scriptableJumpData.GetJumpData();

        if (scriptableJumpData is SimpleJumpData simpleData && simpleData.GetForcePercent() == 0)
        {
            ClearTrajectory();
            return new List<Vector3>();
        }

        if(scriptableJumpData is DashJumpData dashData)
        {
            JumpData dashJumpData = new JumpData(dashData.DashHeight, dashData.DashLength, dashData.DashDuration, dashData.DashHeightCurve, dashData.DashLengthCurve);
            jumpData = dashJumpData;
        }

        Vector3 originPosition = playerTransformController.GetTransformPosition();
        Vector3 direction = playerTransformController.GetForwardDirection();

        float time = jumpData.HeightCurve.keys.Last().time;
        int pointsCount = (int)Mathf.Round(_pointsCountPerLenght * time);

        List<Vector3> points = new List<Vector3>();

        bool isCollided = false;
        int pointsAfterCollided = _pointAfterCollisionCount;
        float progress;
        float nextHeight;
        float nextLength;
        Vector3 nextPosition = Vector3.zero;
        for (int i = 1; i < pointsCount; i++)
        {
            if (isCollided && pointsAfterCollided > 0)
            {
                pointsAfterCollided--;
                ShowCollisionPoint(nextPosition, _collisionPointPrefab);
                if (pointsAfterCollided == 0)
                {
                    break;
                }
            }

            progress = (float)i / _pointsCountPerLenght;
            nextHeight = jumpData.Height * jumpData.HeightCurve.Evaluate(progress);
            nextLength = jumpData.Length * progress;
            nextPosition = originPosition + new Vector3((direction * nextLength).x, nextHeight, (direction * nextLength).z);

            if (BaseJump.IsCollideWithSomething(nextPosition,
                playerTransformController.GetBoxColliderSize(),
                playerTransformController.GetQuaternion(),
                PlayerTransformController.playerLayerMask))
            {
                isCollided = true;
            }

            points.Add(nextPosition);
        }

        //Если отрисовались почти все точки то скрываем точку коллизии
        if(points.Count > pointsCount * 0.9f)
        {
            HideCollisionPoint(_collisionPointPrefab);
        }

        return points;
    }

    private void ShowTrajectory(LineRenderer renderer, Vector3[] points)
    {
        renderer.positionCount = points.Length;
        renderer.SetPositions(points);
    }

    private void ShowCollisionPoint(Vector3 position, GameObject pointPrefab)
    {
        pointPrefab.SetActive(true);
        pointPrefab.GetComponent<Transform>().position = position;
    }

    private void HideCollisionPoint(GameObject pointPrefab)
    {
        pointPrefab.SetActive(false);
    }

    private void OnJumpStarted(float num, ISuperJump sj)
    {
        ClearTrajectory();
    }

    private void OnJumpStarted(float num)
    {
        ClearTrajectory();
    }

    private void ClearTrajectory()
    {
        _lineRenderer.positionCount = 0;
        _dashLineRenderer.positionCount = 0;
        HideCollisionPoint(_collisionPointPrefab);
    }

    private void OnDisable()
    {
        JumpController.ForceChanged -= OnTrajectoryChanged;
        DashSuperJump.DashJumpPreparing -= OnDashTrajectoryChanged;
        DashSuperJump.DashPreparingEnded -= ClearTrajectory;
        JumpController.JumpStarted -= OnJumpStarted;
        DashSuperJump.DashJumpDashed -= OnJumpStarted;
    }
}
