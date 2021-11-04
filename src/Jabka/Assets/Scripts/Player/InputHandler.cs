using UnityEngine;
using Lean.Touch;

public class InputHandler : MonoBehaviour
{
    public static event System.Action<Vector2> SwipeDeltaChanged;

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
    }

    private void OnFingerUpdate(LeanFinger finger)
    {
        //работает только с одним пальцем
        if (finger.Index != 0 || finger.Index != -1) return;

        SwipeDeltaChanged?.Invoke(finger.SwipeScreenDelta);
    }
}
