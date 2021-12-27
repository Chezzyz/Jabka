using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GenericPlatformMove : MonoBehaviour
{
    [SerializeField]
    private bool _playOnAwake = true;
    [SerializeField] 
    private float _duration;
    [SerializeField] 
    float _movingLength;
    [SerializeField] 
    float _rotationAngle;
    [SerializeField]
    float _delay;

    [SerializeField]
    private Mode _mode;
    [SerializeField]
    private Direction _direction;
    [SerializeField]
    private LoopType _loopType;
    [SerializeField]
    private int _loopCount = -1;
    [SerializeField]
    private Ease _ease;

    private bool _isMoving;

    public enum Mode
    {
        Moving, Rotation
    }

    public enum Direction
    {
        X, Y, Z
    }

    private void OnEnable()
    {
        MoveOnTouch.PlayerTouched += OnStartEvent;
        MoveOnButton.ButtonPressed += OnStartEvent;
    }

    private void Start()
    {
        if (!_playOnAwake)
        {
            return;
        }

        StartAnimation();
    }

    private void StartAnimation()
    {
        _isMoving = true;
        if (_mode == Mode.Moving)
        {
            StartMoving(_direction, _movingLength, _duration, _loopCount, _loopType, _delay, _ease);
        }
        else if (_mode == Mode.Rotation)
        {
            StartRotating(_direction, _rotationAngle, _duration, _loopType, _ease);
        }
    }

    private void StartMoving(Direction direction, float length, float duration, int loopCount, LoopType loopType, float delay, Ease ease)
    {
        var seq = DOTween.Sequence();
        var move = direction switch
        {
            Direction.X => transform.DOLocalMoveX(length, duration),
            Direction.Y => transform.DOLocalMoveY(length, duration),
            Direction.Z => transform.DOLocalMoveZ(length, duration),
            _ => throw new System.NotImplementedException(),
        };
        
        move.SetEase(ease);
        seq.Append(move);
        seq.SetLoops(loopCount, loopType).AppendInterval(delay/2).PrependInterval(delay/2);
        seq.Play();
    }

    private void StartRotating(Direction direction, float angle, float duration, LoopType loopType, Ease ease)
    {
        float x = direction == Direction.X ? angle : transform.localEulerAngles.x;
        float y = direction == Direction.Y ? angle : transform.localEulerAngles.y;
        float z = direction == Direction.Z ? angle : transform.localEulerAngles.z;
        var endValue = new Vector3(x, y, z);

        var rotationMove = transform.DOLocalRotate(endValue, duration).SetLoops(-1, loopType);
        rotationMove.SetEase(ease);
        rotationMove.Play();
    }

    private void OnStartEvent(GenericPlatformMove platform)
    {
        if(platform == this && !_isMoving)
        {
            StartAnimation();
        }
    }

    private void OnDisable()
    {
        MoveOnTouch.PlayerTouched -= OnStartEvent;
        MoveOnButton.ButtonPressed -= OnStartEvent;
    }
}

