using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelNumberText;
    [SerializeField]
    private List<QuestView> _questViews;
    [SerializeField]
    private Button _startLevelButton;
    [SerializeField]
    private SceneLoader _sceneLoader;
    [SerializeField]
    private LocalizedStringTable _localizedStringTable;

    private UnityAction _lastLoadSceneListener;

    private void Start()
    {
        if (SceneStatus.GetCurrentLevelNumber() != 0)
        {
            SetupLevelView(SceneStatus.GetCurrentLevelNumber());
        }
    }

    public void SetupLevelViewInChooseMenu(int levelNumber)
    {
        //получаем «глобальный» номер уровня
        LevelMetaData levelMetaData = SceneStatus.GetLevelMetaData(levelNumber);

        SetupLevelView(levelNumber);

        if(_lastLoadSceneListener != null) _startLevelButton.onClick.RemoveListener(_lastLoadSceneListener);
        _lastLoadSceneListener = () => _sceneLoader.LoadScene(levelMetaData.GetSceneName());
        _startLevelButton.onClick.AddListener(_lastLoadSceneListener);
    }
    
    private void SetupLevelView(int levelNumber)
    {
        _levelNumberText.text = $"{_localizedStringTable.GetTable()["LEVEL"].LocalizedValue} {levelNumber}";
        LevelMetaData levelMetaData = SceneStatus.GetLevelMetaData(levelNumber);

        for (int questNumber = 1; questNumber <= _questViews.Count; questNumber++)
        {
            _questViews[questNumber - 1].SetUpView(levelMetaData.GetQuest(questNumber));
        }
    }
}
