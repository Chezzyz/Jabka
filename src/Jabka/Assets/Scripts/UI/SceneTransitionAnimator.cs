using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransitionAnimator : MonoBehaviour
{
    [SerializeField]
    private Image _mask;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private Ease _ease;
    
    private void OnEnable()
    {
        SceneLoader.SceneLoadStarted += OnSceneLoadStarted;
    }

    private void Start()
    {
        _mask.rectTransform.DOSizeDelta(new Vector2(3000, 3000), _duration).SetEase(_ease);
    }

    private void OnSceneLoadStarted(float delay)
    {
        _mask.rectTransform.DOSizeDelta(new Vector2(0, 0), delay).SetEase(_ease);
    }

    private void OnDisable()
    {
        SceneLoader.SceneLoadStarted -= OnSceneLoadStarted;
    }
}
