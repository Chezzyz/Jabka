using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;

public class RotationTutorial : BaseFingerTutorial
{
    [SerializeField]
    private CanvasGroup _canvasGroupSerializable;
    [SerializeField]
    private Animator _animatorSerializable;
    [SerializeField]
    private float _fadeDurationSerializable;
    [SerializeField]
    private float _closeDurationSerializable;
    [SerializeField]
    private float _delayToShowSerializable;
    [SerializeField]
    private float _delayToCloseSerializable;

    private void OnEnable()
    {
        Construct(_canvasGroupSerializable, _animatorSerializable, _fadeDurationSerializable, _closeDurationSerializable);
        LeanTouch.OnFingerDown += OnFingerDown;
    }

    private void Start()
    {
        StartCoroutine(ShowTutorialAfterDelay(_delayToShowSerializable, _delayToCloseSerializable));
    }

    private void OnFingerDown(LeanFinger _)
    {
        if (_isAbleToClose)
        {
            CloseTutorial();
        }
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
    }
}
