using System.Collections;
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
        InputHandler.FingerDown += OnFingerDown;
    }

    private void Start()
    {
        StartCoroutine(ShowTutorialAfterDelay(_delayToShowSerializable, _delayToCloseSerializable));
    }

    private void OnFingerDown(Vector2 pos)
    {
        if (_isAbleToClose)
        {
            CloseTutorial();
        }
    }

    private void OnDisable()
    {
        InputHandler.FingerDown -= OnFingerDown;
    }
}
