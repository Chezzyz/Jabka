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
        ReloadOnDead.LevelReloadStarted += OnLevelReloadStarted;
    }

    private void Start()
    {
        DoOpenTransition(_duration);
    }

    private void OnSceneLoadStarted(float delay)
    {
        DoCloseTransition(delay);
    }

    private void OnLevelReloadStarted(float delay)
    {
        DoCloseTransition(delay);
        StartCoroutine(DoOpenTransitionAfterDelay(delay, delay));
    }

    private void DoCloseTransition(float duration)
    {
        _mask.rectTransform.DOSizeDelta(new Vector2(0, 0), duration).SetEase(_ease);
    }

    private void DoOpenTransition(float duration)
    {
        _mask.rectTransform.DOSizeDelta(new Vector2(3000, 3000), duration).SetEase(_ease);
    }

    private IEnumerator DoOpenTransitionAfterDelay(float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        DoOpenTransition(duration);
    }

    private void OnDisable()
    {
        SceneLoader.SceneLoadStarted -= OnSceneLoadStarted;
        ReloadOnDead.LevelReloadStarted -= OnLevelReloadStarted;
    }
}
