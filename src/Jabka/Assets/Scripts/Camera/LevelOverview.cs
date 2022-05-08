using UnityEngine;
using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class LevelOverview : MonoBehaviour
{
    public static event Action OverviewEnded; 

    [SerializeField]
    private float _duration;

    [SerializeField]
    private CinemachineVirtualCamera _dollyCamera;

    [SerializeField]
    private AnimationCurve _cameraCurve;

    [SerializeField]
    private CinemachineDollyCart _dollyCart;

    [SerializeField]
    private AnimationCurve _cartCurve;

    private void OnEnable()
    {
        LevelPreparation.PlayButtonPushed += StartOverview;
        SceneStatus.SceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(int prevScene, int currentScene)
    {
        if (prevScene == currentScene)
        {
            _dollyCamera.Priority = 0;
        }
    }

    [Button]
    private void StartOverview()
    {
        StartCoroutine(OverviewCoroutine(_duration, _dollyCamera, _cameraCurve, _dollyCart, _cartCurve));
    }

    private IEnumerator OverviewCoroutine(float duration, CinemachineVirtualCamera dollyCamera,
        AnimationCurve cameraCurve, CinemachineDollyCart dollyCart, AnimationCurve cartCurve)
    {
        float expiredTime = 0.0f;
        float maxProgress = 1f;
        float progress = 0.0f;

        while (progress < maxProgress)
        {
            expiredTime += Time.deltaTime;

            progress = expiredTime / duration;

            dollyCart.m_Position = cartCurve.Evaluate(progress);

            dollyCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = cameraCurve.Evaluate(progress);

            yield return null;
        }

        dollyCamera.Priority = 0;

        yield return null;

        OverviewEnded?.Invoke();
    }

    private void OnDisable()
    {
        LevelPreparation.PlayButtonPushed -= StartOverview;
        SceneStatus.SceneChanged -= OnSceneChanged;
    }
}
