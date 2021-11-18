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
    private float _fallSpeed;
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

    private bool _isInFall = false;

    private Coroutine _currentJump;

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
        if (_isInJump || _isInFall)
        {
            if (_playerTransformController.IsOnHorizontalSurface(collision))
            {
                _playerTransformController.SetIsGrounded(true);
                _isInJump = false;
                _isInFall = false;
                _playerTransformController.SetVelocity(Vector3.zero);
            }
            else
            {
                _isInJump = false;
                StartCoroutine(Fall(_fallSpeed));
            }
        }
    }

    private void OnFingerUp(Vector2 fingerPosition)
    {
        if (_currentForcePercent > 0 && _isInJump == false)
        {
            _currentJump = StartJump(_playerTransformController.GetPosition(), _playerTransformController.transform.forward, _currentForcePercent);
        }
    }

    private void OnSwipeY(Vector2 delta)
    {
        //длина свайпа вниз в процентах от экрана, когда свайп сделан вниз delta приходит отрицательная
        _currentForcePercent = CalculateForceInPercent(-delta, _minHeightThreshold, _maxHeightThreshold);
    }

    private Coroutine StartJump(Vector3 originPosition, Vector3 originDirection, float forcePercent)
    {
        Coroutine jump = StartCoroutine(Jump(forcePercent, originPosition, originDirection));

        _playerTransformController.SetIsGrounded(false);
        _currentForcePercent = 0;

        return jump;
    }

    private float CalculateForceInPercent(Vector3 delta, float minTreshold, float maxTreshold)
    {
        float deltaYPercent = delta.y / Screen.height;

        if (deltaYPercent > minTreshold)
        {
            deltaYPercent = Mathf.Clamp(deltaYPercent, minTreshold, maxTreshold);
        }
        else
        {
            return 0;
        }
        
        return (deltaYPercent - minTreshold) / (maxTreshold - minTreshold);
    }

    private IEnumerator Jump(float forcePercent, Vector3 originPosition, Vector3 originDirection)
    {
        _isInJump = true;
        
        float expiredTime = 0.0f;

        float duration = _minDuration + (_maxDuration - _minDuration) * forcePercent;
        float progress = expiredTime / duration;
        
        while (progress < 1 && _isInJump) 
        {
            yield return new WaitForFixedUpdate();
            expiredTime += Time.deltaTime;
            progress = Mathf.Clamp01(expiredTime / duration);

            float currentHeight = (_maxHeight * forcePercent) * _jumpYCurve.Evaluate(progress);
            float currentLength = (_minLength + (_maxLength - _minLength) * forcePercent) * progress;

            _playerTransformController.SetPosition(originPosition + new Vector3((originDirection * currentLength).x, currentHeight, (originDirection * currentLength).z));
        }

        _isInJump = false;

        if (_playerTransformController.IsGrounded == false)
        {
            StartCoroutine(Fall(_fallSpeed));
        }
    }

    private IEnumerator Fall(float fallSpeed) {
        _isInFall = true;

        while (_isInFall)
        {
            yield return new WaitForFixedUpdate();
            Vector3 currentPos = _playerTransformController.GetPosition();
            Vector3 nextPos = new Vector3(currentPos.x, currentPos.y - fallSpeed, currentPos.z);
            _playerTransformController.SetPosition(nextPos);
        }
    }
}