using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _jumpYCurve;
    [SerializeField]
    private float _sensetivity;
    [SerializeField]
    private float _maxHeight;
    [SerializeField]
    private float _maxLength;
    [SerializeField]
    private float _minLength;
    [SerializeField]
    private float _maxDuration;
    [SerializeField]
    private float _minDuration;
    [SerializeField]
    private float _minHeightThreshold;
    [SerializeField]
    private float _maxHeightThreshold;

    private PlayerTransformController _playerTransformController;
    private float _currentForcePercent;
    private Vector3 _originPosition;
    private bool _isInJump = false;

    [Inject]
    public void Construct(PlayerTransformController playerTransformController)
    {
        _playerTransformController = playerTransformController;
    }

    private void OnEnable()
    {
        InputHandler.SwipeDeltaChanged += OnSwipeY;
        InputHandler.FingerUp += OnFingerUp;
        InputHandler.FingerDown += OnFingerDown;
    }

    private void OnFingerDown(Vector2 fingerPosition)
    {
        if (_isInJump == false)
        {
            _originPosition = _playerTransformController.transform.position;
        }
    }

    private void OnFingerUp(Vector2 fingerPosition)
    {
        if (_currentForcePercent > 0 && _isInJump == false)
        {
            StartCoroutine(Jump(_currentForcePercent, _originPosition));
            _currentForcePercent = 0;
        }
    }

    private void OnSwipeY(Vector2 delta)
    {
        //Свайп вниз
        if (delta.y < -_minHeightThreshold)
        {
            _currentForcePercent = CalculateForceInPercent(delta.y);
        }
    }

    private float CalculateForceInPercent(float deltaY)
    {
        deltaY = Mathf.Clamp(deltaY, -_maxHeightThreshold, -_minHeightThreshold);
        return deltaY / -(_maxHeightThreshold - _minHeightThreshold);
    }

    private IEnumerator Jump(float forcePercent, Vector3 originPosition)
    {
        _isInJump = true;

        float expiredTime = 0.0f;

        float duration = _minDuration + (_maxDuration - _minDuration) * forcePercent;
        float progress = expiredTime / duration;

        while (progress < 1) 
        {
            expiredTime += Time.deltaTime;
            progress = Mathf.Clamp01(expiredTime / duration);

            float currentHeight = (_maxHeight * forcePercent) * _jumpYCurve.Evaluate(progress);
            float currentLength = (_minLength + (_maxLength - _minLength) * forcePercent) * progress;

            Vector3 playerForward = _playerTransformController.transform.forward;

            _playerTransformController.SetPosition(originPosition + new Vector3((playerForward * currentLength).x, currentHeight, (playerForward * currentLength).z));
            yield return null;
        } 

        _isInJump = false;
    }

    
}