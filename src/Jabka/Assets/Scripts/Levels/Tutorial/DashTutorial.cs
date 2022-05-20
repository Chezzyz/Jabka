using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTutorial : BaseFingerTutorial
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
    [SerializeField]
    private float _timeScale;

    private bool _isShowed;

    private void OnEnable()
    {
        Construct(_canvasGroupSerializable, _animatorSerializable, _fadeDurationSerializable, _closeDurationSerializable);
        InputHandler.FingerDown += OnFingerDown;
        DashSuperJump.DashJumpStarted += OnDashJumpStarted;
    }

    private void OnDashJumpStarted()
    {
        if(!_isShowed)
        {
            StartCoroutine(SlowAndShowAfterDelay(_delayToShowSerializable));
        }
    }

    private IEnumerator SlowAndShowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = _timeScale;
        //Избавляемся от задержки чтобы timescale точно вернулся назад
        _isAbleToClose = true;
        ShowTutorial();
    }

    private void OnFingerDown(Vector2 pos)
    {
        if (_isAbleToClose && !_isShowed)
        {
            //так как мы войдем в режим подготовки к дэшу нужно выставить скейл как там
            Time.timeScale = 0.1f;
            CloseTutorial();
            _isShowed = true;
        }
    }

    private void OnDisable()
    {
        InputHandler.FingerDown -= OnFingerDown;
        DashSuperJump.DashJumpStarted -= OnDashJumpStarted;
    }
}
