using UnityEngine;
using Lean.Touch;
using System;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> SwipeDeltaChanged;
    public static event Action<Vector2> FingerDown;
    public static event Action<Vector2, float> FingerUp;

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
    }
    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    private void OnFingerUp(LeanFinger finger)
    {
        FingerUp?.Invoke(finger.ScreenPosition, finger.Age);
    }

    private void OnFingerDown(LeanFinger finger)
    {
        FingerDown?.Invoke(finger.ScreenPosition);
    }

    private void OnFingerUpdate(LeanFinger finger)
    {
        //работает только с одним пальцем
        //if (finger.Index != 0 && finger.Index != -1) return;

        SwipeDeltaChanged?.Invoke(finger.SwipeScreenDelta);
    }
}
