using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class QuestView : MonoBehaviour
{
    [SerializeField]
    private BaseQuest _baseQuest;
    [SerializeField]
    private Sprite _completedSprite;
    [SerializeField]
    private TextMeshProUGUI _desctiption;

    private Image _currentImage;

    private void OnEnable()
    {
        BaseQuest.QuestCompleted += OnQuestCompleted;
    }

    private void Start()
    {
        SetUpView(_baseQuest);
    }

    private void OnQuestCompleted(BaseQuest quest)
    {
        if(quest.GetId() == _baseQuest.GetId())
        {
            SetSprite(_completedSprite);
        }
    }

    private void SetUpView(BaseQuest quest)
    {
        _currentImage = GetComponent<Image>();
        _desctiption.text = quest.GetDescription();
        if (quest.IsCompleted())
        {
            SetSprite(_completedSprite);
        }
    }

    private void SetSprite(Sprite sprite)
    {
        _currentImage.sprite = sprite;
    }

    private void OnDisable()
    {
        BaseQuest.QuestCompleted -= OnQuestCompleted;
    }
}
