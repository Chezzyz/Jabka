using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class JumpController : MonoBehaviour
{
    public static event System.Action<ScriptableJumpData> ForceChanged;
    public static event System.Action<ISuperJump> SuperJumpStarted;
    public static event System.Action<float, float> SimpleJumpCancelled;

    private PlayerTransformController _playerTransformController;

    private SimpleJump _simpleJump;

    private ISuperJump _currentSuperJump;

    private float _currentForcePercent;

    [Inject]
    public void Construct(SimpleJump simpleJump, PlayerTransformController playerTransformController)
    {
        _simpleJump = simpleJump;
        _playerTransformController = playerTransformController;
    }

    private void OnEnable()
    {
        InputHandler.SwipeDeltaChanged += OnSwipeY;
        InputHandler.FingerUp += OnFingerUp;
        SuperJumpButton.SuperJumpButtonClicked += StartSuperJump;
    }

    public void SetSuperJump(ISuperJump superJump)
    {
        _currentSuperJump = superJump;
    }

    private void OnFingerUp(Vector2 fingerPosition, float swipeTime)
    {
        //???? ???? ?????? 0 ? ?? ?? ???????, ?? ????? ???????
        if (_currentForcePercent > 0 && !_simpleJump.IsInJump() && (_currentSuperJump == null || !_currentSuperJump.IsInJump()))
        {
            _simpleJump.DoSimpleJump(_playerTransformController, _currentForcePercent);
            _currentForcePercent = 0;
        } 
        else
        {
            SimpleJumpCancelled?.Invoke(_currentForcePercent, _simpleJump.CalculateCurrentJumpData(_currentForcePercent).Duration);
        }
    }

    private void OnSwipeY(Vector2 delta)
    {
        //????? ?????? ???? ? ????????? ?? ??????, ????? ????? ?????? ???? delta ???????? ?????????????
        _currentForcePercent = CalculateForcePercent(-delta);

        ForceChanged?.Invoke(GetSimpleJumpData(_currentForcePercent));
    }

    private SimpleJumpData GetSimpleJumpData(float forcePercent)
    {
        JumpData jumpData = _simpleJump.CalculateCurrentJumpData(forcePercent);

        SimpleJumpData scriptableJumpData = _simpleJump.GetJumpData() as SimpleJumpData;
        scriptableJumpData.SetForcePercent(forcePercent);
        scriptableJumpData.SetCurrentJumpData(jumpData);

        return scriptableJumpData;
    }

    private float CalculateForcePercent(Vector3 delta)
    {
        (float minPercent, float maxPercent) = SettingsHandler.GetVerticalSensetivityBounds(); 

        float deltaYPercent = delta.y / Screen.height;

        if (deltaYPercent < minPercent)
        {
            return 0;
        }
        
        deltaYPercent = Mathf.Clamp(deltaYPercent, minPercent, maxPercent);
        float forcePercent = (deltaYPercent - minPercent) / (maxPercent - minPercent);
        return forcePercent;
    }

    private void StartSuperJump()
    {
        if (!_simpleJump.IsInJump() && !_currentSuperJump.IsInJump())
        {
            _currentSuperJump.SuperJump(_playerTransformController);
            SuperJumpStarted?.Invoke(_currentSuperJump);
        }
    }

    private void OnDisable()
    {
        InputHandler.SwipeDeltaChanged -= OnSwipeY;
        InputHandler.FingerUp -= OnFingerUp;
        SuperJumpButton.SuperJumpButtonClicked -= StartSuperJump;
    }
}
