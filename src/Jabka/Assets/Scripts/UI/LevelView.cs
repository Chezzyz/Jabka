using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField]
    private LevelMetaDataMap _levelMetaDataMap;
    [SerializeField]
    private StageView _stageView;
    [SerializeField]
    private TextMeshProUGUI _levelNumberText;
    [SerializeField]
    private List<QuestView> _questViews;
    [SerializeField]
    private Button _startLevelButton;
    [SerializeField]
    private SceneLoader _sceneLoader;

    private const int _levelCountInStage = 5;
    public void SetupLevelView(int localLevelNumber)
    {
        //получаем «глобальный» номер уровня
        int levelNumber = _levelCountInStage * (_stageView.GetCurrentPageNumber() - 1) + localLevelNumber;

        _levelNumberText.text = $"Level {levelNumber}";
        LevelMetaData levelMetaData = _levelMetaDataMap.GetLevelMetaData(levelNumber);

        for (int questNumber = 1; questNumber <= _questViews.Count; questNumber++)
        {
            _questViews[questNumber - 1].SetUpView(levelMetaData.GetQuest(questNumber));
        }

        _startLevelButton.onClick.AddListener(() => _sceneLoader.LoadScene(levelMetaData.GetSceneName()));
    }
}
