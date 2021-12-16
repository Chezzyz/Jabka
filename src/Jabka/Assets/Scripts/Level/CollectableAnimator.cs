using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CollectableAnimator : MonoBehaviour
{
    [SerializeField]
    private float _rotateLoopDuration;
    [SerializeField]
    private float _collectedAnimationHeight;
    [SerializeField]
    private float _collectedAnimationDuration;
    [SerializeField]
    private Ease _collectedAnimationEase;
    [SerializeField]
    private Ease _collectedColorAnimationEase;

    private void OnEnable()
    {
        Collectable.Collected += OnCollected;
    }

    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 180, 0), _rotateLoopDuration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void OnCollected(Collectable collectable)
    {
        SetOffCollider(collectable);
        StartRiseAnimation(collectable, _collectedAnimationHeight, _collectedAnimationHeight, _collectedAnimationEase);
        StartFadeAnimation(collectable, _collectedAnimationDuration, _collectedColorAnimationEase);
    }

    private void SetOffCollider(Collectable collectable)
    {
        if (collectable.TryGetComponent<Collider>(out var collider))
        {
            collider.enabled = false;
        }
    }

    private void StartFadeAnimation(Collectable collectable, float duration, Ease ease)
    {
        if (collectable.TryGetComponent<MeshRenderer>(out var mesh))
        {
            mesh.material.DOBlendableColor(new Color(1, 1, 1, 0), duration).SetEase(ease);
        }
    }

    private void StartRiseAnimation(Collectable collectable, float height, float duration, Ease ease)
    {
        collectable.transform.DOLocalMoveY(_collectedAnimationHeight, _collectedAnimationDuration).SetEase(_collectedAnimationEase).OnComplete(() => collectable.SelfDestroy());
    }
}
