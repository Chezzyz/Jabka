using UnityEngine;
using Lean.Touch;
using System;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> SwipeDeltaChanged;
    public static event Action<Vector2> FingerDown;
    public static event Action<Vector2, float> FingerUp;

    private bool _canSendEvents = true;

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
        Pause.PauseStateChanged += OnPauseStateChanged;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
        Pause.PauseStateChanged -= OnPauseStateChanged;
    }

    private void OnPauseStateChanged(bool state)
    {
        if (state)
        {
            _canSendEvents = false;
        }
        else
        {
            StartCoroutine(SetCanSendAfterDelay(true, 0.01f));
        }
    }

    private IEnumerator SetCanSendAfterDelay(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        _canSendEvents = value;
    } 

    private void OnFingerUp(LeanFinger finger)
    {
        if (_canSendEvents)
        {
            FingerUp?.Invoke(finger.ScreenPosition, finger.Age);
        }
    }

    private void OnFingerDown(LeanFinger finger)
    {
        if (_canSendEvents)
        {
            FingerDown?.Invoke(finger.ScreenPosition);
        }
    }

    private void OnFingerUpdate(LeanFinger finger)
    {
        if (_canSendEvents)
        {
            SwipeDeltaChanged?.Invoke(finger.SwipeScreenDelta);
        }
    }
}
