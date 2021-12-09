using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DashSuperJump : BaseJump, ISuperJump
{
    [Header("SimplePart")]
    [SerializeField]
    private AnimationCurve _jumpCurve;
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _height;
    [SerializeField]
    private float _duration;

    [Header("DashPart")]
    [SerializeField]
    private AnimationCurve _dashCurve;
    [SerializeField]
    private float _dashLength;
    [SerializeField]
    private float _dashHeight;
    [SerializeField]
    private float _dashDuration;

    public static event System.Action DashJumpPrepareStarted; 

    private PlayerTransformController _playerTransformController;

    protected override void OnEnable()
    {
        base.OnEnable();
        InputHandler.FingerUp += DoDash;
        InputHandler.FingerDown += PrepareForDash;
    }

    public void SuperJump(PlayerTransformController playerTransformController)
    {
        _playerTransformController = playerTransformController;
        _currentJump = StartCoroutine(JumpCoroutine(_playerTransformController, _duration, _height, _length, _jumpCurve));
    }

    public string GetJumpName()
    {
        return "Dash";
    }

    private void DoDash(Vector2 screenPos, float swipeTime)
    {
        if (IsInJump())
        {
            StopCoroutine(_currentJump);
            Time.timeScale = 1;
            StartCoroutine(JumpCoroutine(_playerTransformController, _dashDuration, _dashHeight, _dashLength, _dashCurve));
        }
    }

    private void PrepareForDash(Vector2 screenPos)
    {
        if (IsInJump())
        {
            Time.timeScale = 0.2f;
            DashJumpPrepareStarted?.Invoke();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InputHandler.FingerUp -= DoDash;
        InputHandler.FingerDown -= PrepareForDash;
    }
}
