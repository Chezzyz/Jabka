using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GenericPlatformMove : MonoBehaviour
{
    [SerializeField] 
    private float _duration = 0;
    [SerializeField] 
    float _movingLength = 0;
    [SerializeField] 
    float _rotationAngle = 0;

    [SerializeField]
    private Mode _mode;
    [SerializeField]
    private Direction _direction;
    [SerializeField]
    private LoopType _loopType;
    [SerializeField]
    private Ease _ease;

    public enum Mode
    {
        Moving, Rotation
    }

    public enum Direction
    {
        X, Y, Z
    }

    private void Start()
    {
        if (_mode == Mode.Moving) { 
            StartMoving(_direction, _movingLength, _duration, _loopType, _ease);
        }
        else if(_mode == Mode.Rotation)
        {
            StartRotating(_direction, _rotationAngle, _duration, _loopType, _ease);
        }
    }

    private void StartMoving(Direction direction, float length, float duration, LoopType loopType ,Ease ease)
    {
        var move = direction switch
        {
            Direction.X => transform.DOLocalMoveX(length, duration).SetLoops(-1, loopType),
            Direction.Y => transform.DOLocalMoveY(length, duration).SetLoops(-1, loopType),
            Direction.Z => transform.DOLocalMoveZ(length, duration).SetLoops(-1, loopType),
            _ => throw new System.NotImplementedException(),
        };
        move.SetEase(ease);
        move.Play();
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
}

