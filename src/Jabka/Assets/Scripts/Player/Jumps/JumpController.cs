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

    public static event System.Action<ScriptableJumpData, PlayerTransformController> ForceChanged;
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
        SuperJumpPicker.SuperJumpButtonClicked += StartSuperJump;
    }

    public void SetSuperJump(ISuperJump superJump)
    {
        _currentSuperJump = superJump;
    }

    private void OnFingerUp(Vector2 fingerPosition, float swipeTime)
    {
        //если сила больше 0 и мы не впрыжке, то можем прыгать
        //shtefan предложение: насколько понимание о состянии "в прыжке" это ответственость прыжка? Может есть вариант это затащить под ответственность класса игрока?
        //shtefan предлагаю вообще переписать это место, чтобы мы в условиях сначала находили аргументы для JumpStarted, а потом 1 раз в конце метода его запускали
        if (_currentForcePercent > 0 && !_simpleJump.IsInJump() && (_currentSuperJump == null || !_currentSuperJump.IsInJump()))
        {
            //если преодалеваем трешхолды по силе и времени, то делаем супер-прыжок, иначе обычный 
            if (_currentSuperJump != null && swipeTime <= _superJumpTimeTreshold && _currentForcePercent >= _superJumpForcePercentTreshold)
            {
                //StartSuperJump();
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
        //длина свайпа вниз в процентах от экрана, когда свайп сделан вниз delta приходит отрицательная
        _currentForcePercent = CalculateForcePercent(-delta, _minScreenHeightThreshold, _maxScreenHeightThreshold);

        if (!(_simpleJump.IsInJump() || (_currentSuperJump != null && _currentSuperJump.IsInJump()))){
            ForceChanged?.Invoke(
                GetSimpleJumpData(_currentForcePercent),
                _playerTransformController);
        }
    }

    private SimpleJumpData GetSimpleJumpData(float forcePercent)
    {
        JumpData jumpData = _simpleJump.CalculateCurrentJumpData(forcePercent);

        SimpleJumpData scriptableJumpData = _simpleJump.GetJumpData() as SimpleJumpData;
        scriptableJumpData.SetForcePercent(forcePercent);
        scriptableJumpData.SetCurrentJumpData(jumpData);

        return scriptableJumpData;
    }

    private float CalculateForcePercent(Vector3 delta, float minHeightTreshold, float maxHeightTreshold)
    {
        float deltaYPercent = delta.y / Screen.height;

        if (deltaYPercent < minHeightTreshold)
        {
            return 0;
        }
        
        deltaYPercent = Mathf.Clamp(deltaYPercent, minHeightTreshold, maxHeightTreshold);
        float forcePercent = (deltaYPercent - minHeightTreshold) / (maxHeightTreshold - minHeightTreshold);
        return forcePercent;
    }

    private void StartSuperJump()
    {
        if (!_currentSuperJump.IsInJump())
        {
            _currentSuperJump.SuperJump(_playerTransformController);
            JumpStarted?.Invoke(_currentForcePercent, _currentSuperJump);
        }
    }

    private void OnDisable()
    {
        InputHandler.SwipeDeltaChanged -= OnSwipeY;
        InputHandler.FingerUp -= OnFingerUp;
        SuperJumpPicker.SuperJumpPicked -= SetSuperJump;
        SuperJumpPicker.SuperJumpButtonClicked -= StartSuperJump;
    }
}
