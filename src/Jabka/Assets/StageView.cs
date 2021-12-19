using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageView : MonoBehaviour
{
    [SerializeField]
    private ProgressData _progressData;

    [SerializeField]
    private List<Button> _completedLevelButtons;

    [SerializeField]
    private TextMeshProUGUI _stageName;

    [SerializeField]
    private Sprite _currentLevelSprite;

    [SerializeField]
    private Sprite _completedLevelSprite;

    [SerializeField]
    private Button _nextStageButton;

    [SerializeField]
    private Button _previousStageButton;

    [SerializeField]
    private GameObject _comingSoonBook;


    private static Dictionary<int, string> _stageNameMapping = 
        new Dictionary<int, string> {{1, "Cock"}, {2, "Dick"}, {3, "Ass"}};

    private const int _countOfLevelsInStage = 5;

    [SerializeField]
    private int _currentPage;

    private void OnEnable()
    {
        _nextStageButton.onClick.AddListener(() => { ChangeCurrentPage(1); SetupStage(); });
        _previousStageButton.onClick.AddListener(() => { ChangeCurrentPage(-1); SetupStage(); });
    }

    private void Start()
    {
        _currentPage = _progressData.GetCurrentStage();
        SetupStage();
    }

    public void ChangeCurrentPage(int value)
    {
        _currentPage += value;
    }

    private void SetupStage()
    {
        int currentStage = _progressData.GetCurrentStage();
        int currentLevel = _progressData.GetCurrentLevel();

        if (_currentPage == 1)
        {
            _previousStageButton.gameObject.SetActive(false);
        }
        else 
        {
            _previousStageButton.gameObject.SetActive(true);
        }

        if (_currentPage == _stageNameMapping.Count + 1)
        {
            _nextStageButton.gameObject.SetActive(false);
            _comingSoonBook.SetActive(true);
            return;
        }
        else
        {
            _nextStageButton.gameObject.SetActive(true);
            _comingSoonBook.SetActive(false);
        }

        _stageName.text = _stageNameMapping[_currentPage];

        for (int i = 0; i < _countOfLevelsInStage; i++)
        {
            if (_currentPage > currentStage)
            {
                SetButtonState(_completedLevelButtons[i], false, _currentLevelSprite);
            }
            if (_currentPage == currentStage)
            {
                int levelNumber = i + 1;
                if (levelNumber <= currentLevel)
                {
                    SetButtonState(_completedLevelButtons[i], true,
                        levelNumber == currentLevel ? _currentLevelSprite : _completedLevelSprite);
                }
                else
                {
                    SetButtonState(_completedLevelButtons[i], false, _currentLevelSprite);
                }
            }
            if (_currentPage < currentStage)
            { 
                SetButtonState(_completedLevelButtons[i], true, _completedLevelSprite);
            }
        }
    }

    private void SetButtonState(Button button, bool state, Sprite sprite)
    {
        button.gameObject.SetActive(state);
        button.image.sprite = sprite;
    }
}
