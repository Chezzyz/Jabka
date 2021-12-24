using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;

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
    private AnimationCurve _dashHeightCurve;
    [SerializeField]
    private AnimationCurve _dashLengthCurve;
    [SerializeField]
    private float _dashLength;
    [SerializeField]
    private float _dashHeight;
    [SerializeField]
    private float _dashDuration;
    [SerializeField]
    private float _timeScale;
    [SerializeField]
    private float _slowMoDuration;

    public static event Action<JumpData, PlayerTransformController> DashJumpPreparing;

    private Action<Vector2, float> FingerUpDelegate;

    private PlayerTransformController _playerTransformController;

    private bool _isInPrepare;
    private bool _isInDash;

    public static event Action<float> DashJumpDashed;
    public static event Action<float> DashJumpStarted; //float: duration

    protected override void OnEnable()
    {
        base.OnEnable();
        FingerUpDelegate = (vec, num) => DoDash();
        InputHandler.FingerUp += FingerUpDelegate;
        InputHandler.FingerDown += PrepareForDash;
    }

    public override void SetIsInJump(bool value)
    {
        base.SetIsInJump(value);
        //если мы в дэше и ставим isInJump = false, то останавливаем и дэш
        if (!value)
        {
            SetTimeScale(1);
            _isInPrepare = false;
            _isInDash = false;
        }
    }

    public void SuperJump(PlayerTransformController playerTransformController)
    {
        _playerTransformController = playerTransformController;
        //shtefan: можно заменить на var
        float maxProgress = _jumpCurve.keys.Last().time;
        AnimationCurve lengthCurve = AnimationCurve.Linear(0, 0, maxProgress, maxProgress);

        _currentJump = StartCoroutine(JumpCoroutine(_playerTransformController, _duration, _height, _length, maxProgress, _jumpCurve, lengthCurve));

        DashJumpStarted?.Invoke(_duration);
    }

    public string GetJumpName()
    {
        return "Dash";
    }

    private void DoDash()
    {
        if (_isInPrepare)
        {
            StopCoroutine(_currentJump);
            SetTimeScale(1);
            //shtefan: можно заменить на var
            float maxProgress = _dashHeightCurve.keys.Last().time;

            _isInDash = true;
            _isInPrepare = false;

            StartCoroutine(JumpCoroutine(_playerTransformController, _dashDuration, _dashHeight, _dashLength, maxProgress, _dashHeightCurve, _dashLengthCurve));
            DashJumpDashed?.Invoke(_dashDuration);
        }
    }

    private void PrepareForDash(Vector2 screenPos)
    {
        if (IsInJump() && !_isInDash)
        {
            _isInPrepare = true;
            SetTimeScale(_timeScale);

            var jumpData = new JumpData(_height, _length, 1, _dashHeightCurve);
            StartCoroutine(PreparingForDash(jumpData, _playerTransformController));
            StartCoroutine(OffPrepareAfterDelay(_slowMoDuration));
        }
    }

    private IEnumerator PreparingForDash(JumpData jumpData, PlayerTransformController playerTransformController)
    {
        while (_isInPrepare)
        {
            DashJumpPreparing?.Invoke(jumpData, playerTransformController);
            yield return null;
        }
    }

    private IEnumerator OffPrepareAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetTimeScale(1);
        _isInPrepare = false;
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InputHandler.FingerUp -= FingerUpDelegate;
        InputHandler.FingerDown -= PrepareForDash;
    }
}
