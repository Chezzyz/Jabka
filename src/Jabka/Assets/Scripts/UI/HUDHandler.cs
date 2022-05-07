using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDHandler : MonoBehaviour
{
    [SerializeField]
    private Canvas _hudCanvas;

    private void OnEnable()
    {
        LevelOverview.OverviewEnded += OnOverviewEnded;
        SceneStatus.SceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(int prevScene, int currentScene)
    {
        if (prevScene != currentScene)
        {
            _hudCanvas.enabled = false;
        }
    }

    private void OnOverviewEnded()
    {
        _hudCanvas.enabled = true;
    }

    private void OnDisable()
    {
        LevelOverview.OverviewEnded -= OnOverviewEnded;
        SceneStatus.SceneChanged -= OnSceneChanged;
    }
}
