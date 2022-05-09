using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashJumpTutorial : BaseFingerTutorial
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
        SuperJumpCollectable.SuperJumpCollected += OnSuperJumpCollected;
        InputHandler.FingerDown += OnFingerDown;
    }

    private void OnSuperJumpCollected(SuperJumpCollectable superJump)
    {
        StartCoroutine(AccessToEndAfterDelay(_delayToCloseSerializable));
        ShowTutorial();
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
        SuperJumpCollectable.SuperJumpCollected -= OnSuperJumpCollected;
        InputHandler.FingerDown -= OnFingerDown;
    }
}
