using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BaseFingerTutorial : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private Animator _animator;

    private float _fadeDuration;

    private float _closeDuration;

    protected bool _isAbleToClose;

    protected void Construct(CanvasGroup canvasGroup, Animator animator, float fadeDuration, float closeDuration)
    {
        _canvasGroup = canvasGroup;
        _animator = animator;
        _fadeDuration = fadeDuration;
        _closeDuration = closeDuration;
    }

    protected void ShowTutorial()
    {
        _canvasGroup.DOFade(1, _fadeDuration);
        _animator.SetTrigger("DoAnimation");
    }

    protected void CloseTutorial()
    {
        _canvasGroup.DOFade(0, _closeDuration);
    }

    protected IEnumerator AccessToEndAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isAbleToClose = true;
    }

    protected IEnumerator ShowTutorialAfterDelay(float delay, float accessDelay)
    {
        yield return new WaitForSeconds(delay);
        ShowTutorial();
        StartCoroutine(AccessToEndAfterDelay(accessDelay));
    }
}
