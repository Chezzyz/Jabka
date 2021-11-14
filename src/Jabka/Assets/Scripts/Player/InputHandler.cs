using UnityEngine;
using Lean.Touch;
using System;

public class InputHandler : MonoBehaviour
{
    public static event System.Action<Vector2> SwipeDeltaChanged;
    public static event System.Action<Vector2> FingerDown;

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        LeanTouch.OnFingerDown += OnFingerDown;
    }


    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        LeanTouch.OnFingerDown -= OnFingerDown;
    }

    private void OnFingerDown(LeanFinger finger)
    {
        FingerDown?.Invoke(finger.ScreenPosition);
    }
    private void OnFingerUpdate(LeanFinger finger)
    {
        //�������� ������ � ����� �������
        //if (finger.Index != 0 && finger.Index != -1) return;

        SwipeDeltaChanged?.Invoke(finger.SwipeScreenDelta);
    }
}
