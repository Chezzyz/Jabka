using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestView : MonoBehaviour
{
    [SerializeField]
    private BaseQuest _baseQuest;
    [SerializeField]
    private Sprite _completedSprite;
    [SerializeField]
    private Sprite _completedStarSprite;
    [SerializeField]
    private TextMeshProUGUI _desctiption;
    [SerializeField]
    private Image _currentImage;
    [SerializeField]
    private Image _currentStarImage;
    [SerializeField]
    private bool _isInMainMenu = false;

    private Sprite _defaultSprite;

    private Sprite _defaultStarSprite;


    private readonly Color _grayColor = new Color(0.3019608f, 0.3019608f, 0.3019608f, 1);
    private readonly Color _defaultColor = Color.white;

    private void OnEnable()
    {
        BaseQuest.QuestCompleted += OnQuestCompleted;
    }

    private void Start()
    {
        _defaultSprite = _currentImage.sprite;
        _defaultStarSprite = _currentStarImage.sprite;
        SetUpView(_baseQuest);
    }

    private void OnQuestCompleted(BaseQuest quest)
    {
        if(quest.GetId() == _baseQuest.GetId())
        {
            SetCompletedView();
        }
    }

    public void SetUpView(BaseQuest quest)
    {
        _desctiption.text = quest?.GetDescription();
        if (quest && quest.IsCompleted())
        {
            SetCompletedView();
        }
        else if(quest && !quest.IsCompleted())
        {
            SetDefaultView();
        }
    }

    private void SetDefaultView()
    {
        SetSprite(_currentImage, _defaultSprite, _isInMainMenu ? _grayColor : _defaultColor);
        SetSprite(_currentStarImage, _defaultStarSprite, _isInMainMenu ? _grayColor : _defaultColor);
        SetTextColor(_isInMainMenu ? _grayColor : _defaultColor);
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
    }
}
