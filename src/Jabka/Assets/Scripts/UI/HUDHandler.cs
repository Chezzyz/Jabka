using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour
{
    [SerializeField]
    private Canvas _hudCanvas;

    private void OnEnable()
    {
        LevelOverview.OverviewEnded += OnOverviewEnded;
        SceneStatus.SceneChanged += OnSceneChanged;
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    private void OnSceneChanged(int prevScene, int currentScene)
    {
        //Если переключились с другой сцены, то отключаем hud. Кроме первого уровня
        if (prevScene != currentScene  && currentScene != 1)
        {
            SetCanvasState(false);
        }
    }

    private void OnLevelCompleted(CompletePlace _)
    {
        SetCanvasState(false);
    }

    private void OnOverviewEnded()
    {
        SetCanvasState(true);
    }

    private void SetCanvasState(bool state)
    {
        _hudCanvas.enabled = state;
        _hudCanvas.GetComponent<GraphicRaycaster>().enabled = state;
        _hudCanvas.GetComponentsInChildren<GraphicRaycaster>(true).ToList().ForEach(raycaster => raycaster.enabled = state);
    }

    private void OnDisable()
    {
        LevelOverview.OverviewEnded -= OnOverviewEnded;
        SceneStatus.SceneChanged -= OnSceneChanged;
        CompletePlace.LevelCompleted -= OnLevelCompleted;
    }
}
