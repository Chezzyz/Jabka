using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestView : MonoBehaviour
{
    [SerializeField]
    private Sprite _completedSprite;
    [SerializeField]
    private Sprite _completedStarSprite;
    [SerializeField]
    private TextMeshProUGUI _desctiption;
    [SerializeField]
    private LocalizedStringTable _localizedTable;
    [SerializeField]
    private Image _currentImage;
    [SerializeField]
    private Image _currentStarImage;
    [SerializeField]
    private bool _isInMainMenu = false;

    private Sprite _defaultSprite;

    private Sprite _defaultStarSprite;

    private BaseQuest _baseQuest;

    private readonly Color _grayColor = new Color(0.3019608f, 0.3019608f, 0.3019608f, 1);
    private readonly Color _defaultColor = Color.white;

    private void OnEnable()
    {
        BaseQuest.QuestCompleted += OnQuestCompleted;
        BaseQuest.IsReadyForComplete += OnReadyForComplete;
    }

    private void Awake()
    {
        _defaultSprite = _currentImage.sprite;
        _defaultStarSprite = _currentStarImage.sprite;
    }

    public void SetUpView(BaseQuest quest)
    {
        _baseQuest = quest;
        _desctiption.text = _localizedTable.GetTable()[quest.GetId()].LocalizedValue;
        
        if (quest && quest.IsCompleted())
        {
            SetCompletedView();
        }
        else if(quest && !quest.IsCompleted())
        {
            SetDefaultView();
        }
    }

    private void OnQuestCompleted(BaseQuest quest)
    {
        if (quest && quest.GetId() == _baseQuest.GetId())
        {
            SetCompletedView();
        }
    }

    private void OnReadyForComplete(BaseQuest quest)
    {
        if (quest && quest.GetId() == _baseQuest.GetId())
        {
            SetReadyForCompleteView();
        }
    }

    private void SetDefaultView()
    {
        SetSprite(_currentImage, _defaultSprite, _isInMainMenu ? _grayColor : _defaultColor);
        SetSprite(_currentStarImage, _defaultStarSprite, _isInMainMenu ? _grayColor : _defaultColor);
        SetTextColor(_isInMainMenu ? _grayColor : _defaultColor);
    }

    private void SetReadyForCompleteView()
    {
        SetSprite(_currentImage, _completedSprite, Color.white);
        SetTextColor(_grayColor);
    }

    private void SetCompletedView()
    {
        SetSprite(_currentImage, _completedSprite, Color.white);
        SetSprite(_currentStarImage, _completedStarSprite, Color.white);
        SetTextColor(_grayColor);
    }

    private void SetSprite(Image image, Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.color = color;
    }

    private void SetTextColor(Color color)
    {
        _desctiption.color = color;
    }

    private void OnDisable()
    {
        BaseQuest.QuestCompleted -= OnQuestCompleted;
        BaseQuest.IsReadyForComplete -= OnReadyForComplete;
    }
}
