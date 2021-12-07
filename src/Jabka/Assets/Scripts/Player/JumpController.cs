using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class JumpController : MonoBehaviour
{
    [SerializeField]
    private float _minScreenHeightThreshold;
    [SerializeField]
    private float _maxScreenHeightThreshold;
    [SerializeField]
    private float _superJumpTimeTreshold;
    [SerializeField]
    private float _superJumpForcePercentTreshold;

    public static event System.Action<SimpleJumpData, Vector3, Vector3, float> ForceChanged;
    public static event System.Action<float, ISuperJump> JumpStarted;

    private PlayerTransformController _playerTransformController;

    private SimpleJump _simpleJump;

    private SuperJumpPicker _superJumpPicker;

    private ISuperJump _currentSuperJump;

    private float _currentForcePercent;

    [Inject]
    public void Construct(SimpleJump simpleJump, PlayerTransformController playerTransformController, SuperJumpPicker superJumpPicker)
    {
        _simpleJump = simpleJump;
        _playerTransformController = playerTransformController;
        _superJumpPicker = superJumpPicker;
    }

    private void OnEnable()
    {
        InputHandler.SwipeDeltaChanged += OnSwipeY;
        InputHandler.FingerUp += OnFingerUp;
        SuperJumpPicker.SuperJumpPicked += SetSuperJump;
    }

    private void Start()
    {
        _currentSuperJump = _superJumpPicker.GetDefaultSuperJump();
    }

    private void OnFingerUp(Vector2 fingerPosition, float swipeTime)
    {
        if (_currentForcePercent > 0 && _simpleJump.IsInJump() == false)
        {

            if (swipeTime <= _superJumpTimeTreshold && _currentForcePercent >= _superJumpForcePercentTreshold)
            {
                _currentSuperJump.SuperJump(_playerTransformController);
                JumpStarted?.Invoke(_currentForcePercent, _currentSuperJump);
            }
            else
            {
                _simpleJump.DoSimpleJump(_playerTransformController, _currentForcePercent);
                JumpStarted?.Invoke(_currentForcePercent, null);
                _currentForcePercent = 0;
            }
        }
    }

    private void OnSwipeY(Vector2 delta)
    {
        //длина свайпа вниз в процентах от экрана, когда свайп сделан вниз delta приходит отрицательная
        _currentForcePercent = CalculateSwipeLengthInPercent(-delta, _minScreenHeightThreshold, _maxScreenHeightThreshold);

        ForceChanged?.Invoke(_simpleJump.JumpData, 
            _playerTransformController.GetPosition(),
            _playerTransformController.GetForwardDirection(),
            _currentForcePercent);
    }

    private float CalculateSwipeLengthInPercent(Vector3 delta, float minHeightTreshold, float maxHeightTreshold)
    {
        float deltaYPercent = delta.y / Screen.height;

        if (deltaYPercent > minHeightTreshold)
        {
            deltaYPercent = Mathf.Clamp(deltaYPercent, minHeightTreshold, maxHeightTreshold);
        }
        else
        {
            return 0;
        }

        float forcePercent = (deltaYPercent - minHeightTreshold) / (maxHeightTreshold - minHeightTreshold);
        return forcePercent;
    }

    private void SetSuperJump(ISuperJump superJump)
    {
        _currentSuperJump = superJump;
    }

    private void OnDisable()
    {
        InputHandler.SwipeDeltaChanged -= OnSwipeY;
        InputHandler.FingerUp -= OnFingerUp;
        SuperJumpPicker.SuperJumpPicked -= SetSuperJump;
    }
}
