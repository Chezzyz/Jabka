using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpTutorial : BaseFingerTutorial
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

    private void OnEnable()
    {
        Construct(_canvasGroupSerializable, _animatorSerializable, _fadeDurationSerializable, _closeDurationSerializable);
        SuperJumpCollectable.SuperJumpCollected += OnSuperJumpCollected;
        LeanTouch.OnFingerDown += OnFingerDown;
    }

    private void OnSuperJumpCollected(SuperJumpCollectable jumpCollectable)
    {
        ShowTutorial();
        StartCoroutine(AccessToEndAfterDelay(_delayToCloseSerializable));
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
        SuperJumpCollectable.SuperJumpCollected -= OnSuperJumpCollected;
        LeanTouch.OnFingerDown -= OnFingerDown;
    }

}