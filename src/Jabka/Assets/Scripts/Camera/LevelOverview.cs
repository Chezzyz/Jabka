using UnityEngine;
using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelOverview : MonoBehaviour
{
    public static event Action OverviewEnded; 

    [SerializeField]
    private float _cameraDuration;

    [SerializeField]
    private CinemachineVirtualCamera _dollyCamera;

    [SerializeField]
    private AnimationCurve _cameraCurve;

    [SerializeField]
    private float _cartDuration;

    [SerializeField]
    private CinemachineDollyCart _dollyCart;

    [SerializeField]
    private AnimationCurve _cartCurve;

    [SerializeField]
    private Button _skipButton;

    private void OnEnable()
    {
        LevelPreparation.PlayButtonPushed += StartOverview;
        SceneStatus.SceneChanged += OnSceneChanged;
        _skipButton.onClick.AddListener(() => { _dollyCamera.Priority = 0; OverviewEnded?.Invoke(); _skipButton.gameObject.SetActive(false); });
    }

    private void OnSceneChanged(int prevScene, int currentScene)
    {
        if (prevScene == currentScene)
        {
            _dollyCamera.Priority = 0;
            _skipButton.gameObject.SetActive(false);
        }
    }

    [Button]
    private void StartOverview()
    {
        _skipButton.gameObject.SetActive(true);
        StartCoroutine(OverviewCoroutine(_cameraDuration, _dollyCamera, _cameraCurve, _cartDuration, _dollyCart, _cartCurve));
    }

    private IEnumerator OverviewCoroutine(float cameraDuration, CinemachineVirtualCamera dollyCamera,
        AnimationCurve cameraCurve, float cartDuration, CinemachineDollyCart dollyCart, AnimationCurve cartCurve)
    {
        float expiredTime = 0.0f;
        float maxProgress = 1f;
        float cameraProgress = 0.0f;
        float cartProgress = 0.0f;

        while (cameraProgress < maxProgress || cartProgress < maxProgress)
        {
            expiredTime += Time.deltaTime;

            cameraProgress = expiredTime / cameraDuration;
            cartProgress = expiredTime / cartDuration;

            dollyCart.m_Position = cartCurve.Evaluate(cartProgress);
            dollyCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = cameraCurve.Evaluate(cameraProgress);

            yield return null;
        }

        dollyCamera.Priority = 0;

        yield return null;

        _skipButton.gameObject.SetActive(false);

        OverviewEnded?.Invoke();
    }

    private void OnDisable()
    {
        LevelPreparation.PlayButtonPushed -= StartOverview;
        SceneStatus.SceneChanged -= OnSceneChanged;
        _skipButton.onClick.RemoveAllListeners();
    }
}
