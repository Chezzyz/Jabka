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

    private DG.Tweening.Core.TweenerCore<Quaternion, Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> _rotating;

    private void OnEnable()
    {
        Collectable.Collected += OnCollected;
        SuperJumpCollectable.SuperJumpCollected += OnCollected;
        Destination.Destinated += OnCollected; 
    }

    private void Start()
    {
        _rotating = transform.DOLocalRotate(new Vector3(0, 180, 0), _rotateLoopDuration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void OnCollected(BaseQuestItem questItem)
    {
        if (gameObject != null && GetComponent<BaseQuestItem>() == questItem)
        {
            SetOffCollider(questItem);
            StartFadeAnimation(questItem, _collectedAnimationDuration, _collectedColorAnimationEase);
            StartRiseAnimation(questItem, _collectedAnimationHeight, _collectedAnimationDuration, _collectedAnimationEase);
        }
    }

    private void SetOffCollider(BaseQuestItem questItem)
    {
        if (questItem.TryGetComponent<BaseQuestItem>(out var collider))
        {
            collider.enabled = false;
        }
    }

    private void StartFadeAnimation(BaseQuestItem questItem, float duration, Ease ease)
    {
        if (questItem.TryGetComponent<MeshRenderer>(out var mesh))
        {
            mesh.material.DOBlendableColor(new Color(1, 1, 1, 0), duration).SetEase(ease);
        }
    }

    private void StartRiseAnimation(BaseQuestItem questItem, float height, float duration, Ease ease)
    {
        questItem.transform.DOLocalMoveY(height, duration).SetEase(ease).OnKill(() => { _rotating.Kill(); questItem.SelfDestroy(); });
    }

    private void OnDisable()
    {
        Collectable.Collected -= OnCollected;
        SuperJumpCollectable.SuperJumpCollected -= OnCollected;
        Destination.Destinated -= OnCollected;
    }
}
