using System;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _jumpYCurve;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        StopCoroutine(nameof(Jump));
        //StopAllCoroutines();
    }

    private void OnFingerUp(Vector2 fingerPosition)
    {
        if (_currentForcePercent > 0 && _isInJump == false)
        {
            Vector3 originPosition = _playerTransformController.transform.position;
            Vector3 originDirection = _playerTransformController.transform.forward;

            StartCoroutine(Jump(_currentForcePercent, originPosition, originDirection));
            
            _currentForcePercent = 0;
        }
    }

    private void OnSwipeY(Vector2 delta)
    {
        //длина свайпа вниз в процентах от экрана
        float deltaYPercent = delta.y / Screen.height;

        //сравниваем так, так как delta приходит отрицательная, если свайп сделан вниз
        if (-deltaYPercent > _minHeightThreshold)
        {
            _currentForcePercent = CalculateForceInPercent(deltaYPercent);
        }
    }

    private float CalculateForceInPercent(float deltaY)
    {
        deltaY = Mathf.Clamp(deltaY, -_maxHeightThreshold, -_minHeightThreshold);
        return deltaY / -(_maxHeightThreshold - _minHeightThreshold);
    }

    private IEnumerator Jump(float forcePercent, Vector3 originPosition, Vector3 originDirection)
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

            _playerTransformController.SetPosition(originPosition + new Vector3((originDirection * currentLength).x, currentHeight, (originDirection * currentLength).z));
            yield return new WaitForFixedUpdate();
        } 
        
        _isInJump = false;
    }
}