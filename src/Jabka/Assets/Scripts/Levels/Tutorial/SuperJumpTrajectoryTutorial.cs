using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpTrajectoryTutorial : BaseFingerTutorial
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
    private float _delayToCloseSerializable;

    private bool _isShowed;

    private void OnEnable()
    {
        Construct(_canvasGroupSerializable, _animatorSerializable, _fadeDurationSerializable, _closeDurationSerializable);
        BaseJump.JumpEnded += OnLongSuperJumpEnded;
        LeanTouch.OnFingerDown += OnFingerDown;
    }

    private void OnLongSuperJumpEnded(BaseJump jump)
    {
        if(jump is LongSuperJump && !_isShowed)
        {
            StartCoroutine(AccessToEndAfterDelay(_delayToCloseSerializable));
            ShowTutorial();
            _isShowed = true;
        }
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
        BaseJump.JumpEnded -= OnLongSuperJumpEnded;
        LeanTouch.OnFingerDown -= OnFingerDown;
    }
}
