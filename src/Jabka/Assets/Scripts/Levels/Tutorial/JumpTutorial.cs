using System.Collections;
using Lean.Touch;
using UnityEngine;

public class JumpTutorial : BaseFingerTutorial
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

    private bool _isShowed;

    private void OnEnable()
    {
        Construct(_canvasGroupSerializable, _animatorSerializable, _fadeDurationSerializable, _closeDurationSerializable);
        LeanTouch.OnFingerDown += OnFingerDown;
    }

    private void OnFingerDown(LeanFinger _)
    {
        if (!_isShowed)
        {
            StartCoroutine(ShowTutorialAfterDelay(_delayToShowSerializable, _delayToCloseSerializable));
            _isShowed = true;
        }
        else
        {
            if (_isAbleToClose)
            {
                CloseTutorial();
            }
        }
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
    }
}
