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
        JumpController.JumpStarted += OnJumpStarted;
    }

    private void OnJumpStarted(float force, ISuperJump superJump)
    {
        if(!_isShowed && superJump != null && superJump.GetJumpName() == "Dash")
        {
            StartCoroutine(SlowAndShowAfterDelay(_delayToShowSerializable));
        }
    }

    private IEnumerator SlowAndShowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = _timeScale;
        StartCoroutine(AccessToEndAfterDelay(_delayToCloseSerializable));
        ShowTutorial();
    }

    private void OnFingerDown(Vector2 pos)
    {
        if (_isAbleToClose && !_isShowed)
        {
            //��� ��� �� ������ � ����� ���������� � ���� ����� ��������� ����� ��� ���
            Time.timeScale = 0.1f;
            CloseTutorial();
            _isShowed = true;
        }
    }

    private void OnDisable()
    {
        InputHandler.FingerDown -= OnFingerDown;
        JumpController.JumpStarted -= OnJumpStarted;
    }
}