using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class StageView : MonoBehaviour
{
    [SerializeField]
    private ProgressData _progressData;

    [SerializeField]
    private List<StagePage> _stagePages;

    [SerializeField]
    private List<Button> _currentStageLevelButtons;

    [SerializeField]
    private Sprite _currentLevelSprite;

    [SerializeField]
    private Sprite _completedLevelSprite;

    [SerializeField]
    private Button _nextStageButton;

    [SerializeField]
    private Button _previousStageButton;

    [SerializeField]
    private StagePage _comingSoonPage;

    [SerializeField]
    private Canvas _mainMenuCanvas;

    private const int _countOfLevelsInStage = 5;
   
    private int _currentPageIndex;

    private StagePage _currentPage;

    private void OnEnable()
    {
        _nextStageButton.onClick.AddListener(() => { ChangeCurrentPage(1); });
        _previousStageButton.onClick.AddListener(() => { ChangeCurrentPage(-1); });
    }

    private void Start()
    {
        _currentPageIndex = _progressData.GetCurrentStage() - 1;
        ChangeCurrentPage(0);
    }

    private void ChangeCurrentPage(int value)
    {
        if(_currentPageIndex == 0 && value == -1)
        {
            //если нажимаем назад на первой странице то переходим на главную
            GetComponent<Canvas>().enabled = false;
            _mainMenuCanvas.enabled = true;
            return;
        }
        _currentPageIndex += value;

        _currentPage?.gameObject.SetActive(false);

        //если переключаемся на страницу этапа, номер которой больше максимального, показываем страницу coming soon
        if (_currentPageIndex == _stagePages.Count)
        {
            _nextStageButton.gameObject.SetActive(false);
            _comingSoonPage.gameObject.SetActive(true);
            return;
        }
        else
        {
            _nextStageButton.gameObject.SetActive(true);
            _comingSoonPage.gameObject.SetActive(false);
        }

        _currentPage = _stagePages[_currentPageIndex];

        _currentPage.gameObject.SetActive(true);
        _currentStageLevelButtons = _currentPage.GetStageButtons();

        SetupStage(_currentPageIndex, _currentStageLevelButtons);
    }

    public int GetCurrentPageNumber()
    {
        return _currentPageIndex;
    }

    private void SetupStage(int pageIndex, List<Button> stageButtons)
    {
        int currentProgressStage = _progressData.GetCurrentStage();
        int currentProgressLevel = _progressData.GetCurrentLevel();
        int stageNumber = pageIndex + 1;

        for (int i = 0; i < _countOfLevelsInStage; i++)
        {
            if (stageNumber > currentProgressStage)
            {
                SetButtonState(stageButtons[i], false, _currentLevelSprite);
            }
            if (stageNumber == currentProgressStage)
            {
                int levelNumber = i + pageIndex * _countOfLevelsInStage + 1;
                if (levelNumber <= currentProgressLevel)
                {
                    SetButtonState(stageButtons[i], true, levelNumber == currentProgressLevel ? _currentLevelSprite : _completedLevelSprite);
                }
                else
                {
                    SetButtonState(stageButtons[i], false, _currentLevelSprite);
                }
            }
            if (stageNumber < currentProgressStage)
            { 
                SetButtonState(stageButtons[i], true, _completedLevelSprite);
            }
        }
    }

    private void SetButtonState(Button button, bool state, Sprite sprite)
    {
        button.gameObject.SetActive(state);
        button.image.sprite = sprite;
    }
}
