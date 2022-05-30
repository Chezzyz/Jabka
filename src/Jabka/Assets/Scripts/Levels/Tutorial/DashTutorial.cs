using System.Collections;
using Lean.Touch;
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
        LeanTouch.OnFingerDown += OnFingerDown;
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
        //����������� �� �������� ����� timescale ����� �������� �����
        _isAbleToClose = true;
        ShowTutorial();
    }

    private void OnFingerDown(LeanFinger _)
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
        LeanTouch.OnFingerDown -= OnFingerDown;
        DashSuperJump.DashJumpStarted -= OnDashJumpStarted;
    }
}
