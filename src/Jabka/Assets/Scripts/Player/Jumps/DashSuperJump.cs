using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;

public class DashSuperJump : BaseJump, ISuperJump
{
    [SerializeField]
    private DashJumpData _jumpData;

    public static event Action<ScriptableJumpData> DashJumpPreparing;

    private Action<Vector2, float> FingerUpDelegate;

    private PlayerTransformController _playerTransformController;

    private bool _isInPrepare;
    private bool _isInDash;

    public static event Action DashJumpStarted;
    public static event Action<float> DashPreparingStarted;
    public static event Action DashPreparingEnded;
    public static event Action<float> DashJumpDashed;

    private void OnEnable()
    {
        FingerUpDelegate = (vec, num) => DoDash();
        InputHandler.FingerUp += FingerUpDelegate;
        InputHandler.FingerDown += PrepareForDash;
    }

    protected override void SetIsInJump(bool value)
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

    public override ScriptableJumpData GetJumpData()
    {
        return _jumpData;
    }

    public void SuperJump(PlayerTransformController playerTransformController)
    {
        _playerTransformController = playerTransformController;

        _currentJump = StartCoroutine(JumpCoroutine(_playerTransformController, _jumpData.GetJumpData()));

        DashJumpStarted?.Invoke();
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

            _isInDash = true;
            _isInPrepare = false;

            JumpData dashJumpData = new JumpData(_jumpData.DashHeight, _jumpData.DashLength, _jumpData.DashDuration, _jumpData.DashHeightCurve, _jumpData.DashLengthCurve);

            StartCoroutine(JumpCoroutine(_playerTransformController, dashJumpData));
            DashJumpDashed?.Invoke(_jumpData.DashDuration);
        }
    }

    private void PrepareForDash(Vector2 screenPos)
    {
        if (IsInJump() && !_isInDash)
        {
            _isInPrepare = true;
            SetTimeScale(_jumpData.TimeScale);

            StartCoroutine(PreparingForDash());
            StartCoroutine(OffPrepareAfterDelay(_jumpData.SlowMoDuration));
            DashPreparingStarted?.Invoke(_jumpData.SlowMoDuration);
        }
    }

    private IEnumerator PreparingForDash()
    {
        while (_isInPrepare)
        {
            DashJumpPreparing?.Invoke(_jumpData);
            yield return null;
        }
    }

    private IEnumerator OffPrepareAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetTimeScale(1);
        _isInPrepare = false;
        DashPreparingEnded?.Invoke();
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private void OnDisable()
    {
        InputHandler.FingerUp -= FingerUpDelegate;
        InputHandler.FingerDown -= PrepareForDash;
    }
}
