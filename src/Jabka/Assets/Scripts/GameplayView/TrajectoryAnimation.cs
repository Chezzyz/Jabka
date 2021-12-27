using UnityEngine;
using DG.Tweening;

//[RequireComponent(typeof(Material))]
public class TrajectoryAnimation : MonoBehaviour
{
    [SerializeField]
    private float _animationSpeed;

    [SerializeField]
    private Material _trajectoryMat;

    private Tween _trajectoryTween;

    private void OnEnable()
    {
        _trajectoryTween = _trajectoryMat.DOOffset(new Vector2(-1, 0), _animationSpeed).SetEase(Ease.Linear).SetLoops(-1);
    }

    private void OnDisable()
    {
        _trajectoryMat.mainTextureOffset = Vector2.zero;
        _trajectoryTween.Kill();
    }
}
