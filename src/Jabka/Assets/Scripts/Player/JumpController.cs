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

    private PlayerTransformController _playerTransformController;

    private SimpleJump _simpleJump;

    private ISuperJump _currentSuperJump;

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
    }

    private void Start()
    {
        _currentSuperJump
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_simpleJump.IsInJump)
        {
            _simpleJump.SetIsInJump(false);
        }
    }

    private void OnFingerUp(Vector2 fingerPosition, float swipeTime)
    {
        float currentForcePercent = _simpleJump.GetCurrentForcePercent();

        if (currentForcePercent > 0 && _simpleJump.IsInJump == false)
        {
            if (swipeTime <= _superJumpTimeTreshold && currentForcePercent >= _superJumpForcePercentTreshold)
            {
                _currentSuperJump.SuperJump(_playerTransformController);
            }
            else
            {
                _simpleJump.StartJump(_playerTransformController, currentForcePercent);
                _simpleJump.SetCurrentForcePercent(0);
            }
        }
    }

    private void OnSwipeY(Vector2 delta)
    {
        //длина свайпа вниз в процентах от экрана, когда свайп сделан вниз delta приходит отрицательная
        _simpleJump.SetCurrentForcePercent(CalculateSwipeLengthInPercent(-delta, _minScreenHeightThreshold, _maxScreenHeightThreshold));
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

        return (deltaYPercent - minHeightTreshold) / (maxHeightTreshold - minHeightTreshold);
    }
}
