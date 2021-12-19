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

    public static event System.Action<JumpData, PlayerTransformController> ForceChanged;
    public static event System.Action<float, ISuperJump> JumpStarted;

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
        SuperJumpPicker.SuperJumpPicked += SetSuperJump;
    }

    public void SetSuperJump(ISuperJump superJump)
    {
        _currentSuperJump = superJump;
    }

    private void OnFingerUp(Vector2 fingerPosition, float swipeTime)
    {
        //���� ���� ������ 0 � �� �� �������, �� ����� �������
        if (_currentForcePercent > 0 && !_simpleJump.IsInJump() && (_currentSuperJump == null || !_currentSuperJump.IsInJump()))
        {
            //���� ������������ ��������� �� ���� � �������, �� ������ �����-������, ����� ������� 
            if (_currentSuperJump != null && swipeTime <= _superJumpTimeTreshold && _currentForcePercent >= _superJumpForcePercentTreshold)
            {
                _currentSuperJump.SuperJump(_playerTransformController);
                JumpStarted?.Invoke(_currentForcePercent, _currentSuperJump);
            }
            else
            {
                _simpleJump.DoSimpleJump(_playerTransformController, _currentForcePercent);
                JumpStarted?.Invoke(_currentForcePercent, null);
            }
            _currentForcePercent = 0;
        }
        else
        {
            JumpStarted?.Invoke(0, null);
        }
    }

    private void OnSwipeY(Vector2 delta)
    {
        //����� ������ ���� � ��������� �� ������, ����� ����� ������ ���� delta �������� �������������
        _currentForcePercent = CalculateForcePercent(-delta, _minScreenHeightThreshold, _maxScreenHeightThreshold);

        if (!(_simpleJump.IsInJump() || (_currentSuperJump != null && _currentSuperJump.IsInJump()))){
            ForceChanged?.Invoke(
                GetSimpleJumpData(_currentForcePercent),
                _playerTransformController);
        }
    }

    private JumpData GetSimpleJumpData(float forcePercent)
    {
        return new JumpData(_simpleJump.CalculateHeight(forcePercent),
            _simpleJump.CalculateLength(forcePercent),
            forcePercent,
            _simpleJump.GetAnimationCurve());
    }

    private float CalculateForcePercent(Vector3 delta, float minHeightTreshold, float maxHeightTreshold)
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

    

    private void OnDisable()
    {
        InputHandler.SwipeDeltaChanged -= OnSwipeY;
        InputHandler.FingerUp -= OnFingerUp;
        SuperJumpPicker.SuperJumpPicked -= SetSuperJump;
    }
}
