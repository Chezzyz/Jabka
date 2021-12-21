using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private const int _levelCountInStage = 5;
    public void SetupLevelView(int localLevelNumber)
    {
        //получаем «глобальный» номер уровня
        int levelNumber = _levelCountInStage * (_stageView.GetCurrentPageNumber() - 1) + localLevelNumber;

        _levelNumberText.text = $"Level {levelNumber}";

        for(int questNumber = 1; questNumber <= _questViews.Count; questNumber++)
        {
            _questViews[questNumber - 1].SetUpView(_levelMetaDataMap.GetLevelMetaData(levelNumber).GetQuest(questNumber));
        }
    }
}
