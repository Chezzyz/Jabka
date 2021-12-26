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

    private LevelMetaData _levelMetaData;

    private const int _levelCountInStage = 5;

    [Zenject.Inject] 
    public void Construct(LevelMetaData levelMetaData)
    {
        _levelMetaData = levelMetaData;
    }

    private void Start()
    {
        if (_levelMetaData != null)
        {
            SetupLevelView(_levelMetaData.GetLevelNumber());
        }    
    }

    public void SetupLevelViewInChooseMenu(int localLevelNumber)
    {
        //получаем «глобальный» номер уровня
        int levelNumber = _levelCountInStage * (_stageView.GetCurrentPageNumber() - 1) + localLevelNumber;
        LevelMetaData levelMetaData = _levelMetaDataMap.GetLevelMetaData(levelNumber);

        SetupLevelView(levelNumber);

        _startLevelButton.onClick.AddListener(() => _sceneLoader.LoadScene(levelMetaData.GetSceneName()));
    }
    
    private void SetupLevelView(int levelNumber)
    {
        _levelNumberText.text = $"Level {levelNumber}";
        LevelMetaData levelMetaData = _levelMetaDataMap.GetLevelMetaData(levelNumber);

        for (int questNumber = 1; questNumber <= _questViews.Count; questNumber++)
        {
            _questViews[questNumber - 1].SetUpView(levelMetaData.GetQuest(questNumber));
        }
    }
}
