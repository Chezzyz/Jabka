 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomPlatformAnimation_Level5_1 : MonoBehaviour
{
    [SerializeField]
    private float _rotationDuration;
    [SerializeField]
    private float _movingDurationY;
    [SerializeField]
    private float _movingDurationX;
    [SerializeField]
    float _movingLength;
    [SerializeField]
    float _movingHeight;
    [SerializeField]
    float _rotationAngle;
    [SerializeField]
    float _delay;
   
    void Start()
    {
        Sequence seq = DOTween.Sequence();

        Tween rotation = transform.DOLocalRotate(new Vector3(0, _rotationAngle, 0), _rotationDuration).SetEase(Ease.Linear);
        Tween moveY = transform.DOLocalMoveY(_movingHeight, _movingDurationY).SetEase(Ease.Linear);
        Tween moveX = transform.DOLocalMoveX(_movingLength, _movingDurationX).SetEase(Ease.Linear);

        seq.Append(rotation);
        seq.Join(moveY);
        seq.Append(moveX);
        seq.PrependInterval(_delay / 2).AppendInterval(_delay / 2).SetLoops(-1, LoopType.Yoyo);

        seq.Play();
    }

}
