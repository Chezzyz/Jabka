using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class OnCompleteQuestEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject _taskGroup;
    [SerializeField]
    private Sprite _completedSprite;
    [SerializeField]
    private Sprite _defaultSprite;
    [SerializeField]
    private TextMeshProUGUI _desctiption;
    [SerializeField]
    private Image _currentImage;

    [SerializeField]
    private float _defaultPosY;
    [SerializeField]
    private float _targetPosY;
    [SerializeField]
    private float _animationInDuration;
    [SerializeField]
    private float _animationOutDuration;
    [SerializeField]
    private float _animationPunchDuration;
    [SerializeField]
    private float _animationPunchScale;

    [SerializeField]
    private LocalizedStringTable _localizedStringTable;

    public static event System.Action ReadyForCompleteViewSetted;

    private readonly Color _defaultColor = Color.white;
    private readonly Color _grayColor = new Color(0.3019608f, 0.3019608f, 0.3019608f, 1);

    private void OnEnable()
    {
        BaseQuest.IsReadyForComplete += OnReadyForComplete;
    }

    private void OnReadyForComplete(BaseQuest quest)
    {
        if (quest && SceneStatus.GetCurrentMetaData().GetLevelNumber() == quest.GetLevelNumber() && !(quest is CompleteQuest || quest is JumpCountQuest))
        {
            _taskGroup.SetActive(true);
            _desctiption.text = _localizedStringTable.GetTable()[quest.GetId()].LocalizedValue;
            DoReadyForCompleteAnimation();
        }
    }

    private void DoReadyForCompleteAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        Tween moveDown = _taskGroup.GetComponent<RectTransform>().DOAnchorPosY(_targetPosY, _animationInDuration).SetEase(Ease.OutCubic).Pause();
        Tween moveUp = _taskGroup.GetComponent<RectTransform>().DOAnchorPosY(_defaultPosY, _animationOutDuration).SetEase(Ease.InCubic).Pause();
        Tween punch = _taskGroup.transform.DOScale(Vector3.one * _animationPunchScale, _animationPunchDuration).SetEase(Ease.InCubic).Pause()
            .OnComplete(() => SetReadyForCompleteView());
        Tween unpunch = _taskGroup.transform.DOScale(Vector3.one, _animationPunchDuration).SetEase(Ease.InQuad).Pause();

        sequence.Append(moveDown);
        sequence.Append(punch);
        sequence.Append(unpunch);
        sequence.Append(moveUp);

        sequence.OnComplete(() => { SetDefaultView(); _taskGroup.SetActive(false); });

        sequence.Play();
    }

    private void SetReadyForCompleteView()
    {
        SetSprite(_currentImage, _completedSprite, Color.white);
        SetTextColor(_grayColor);
        ReadyForCompleteViewSetted?.Invoke();
    }

    private void SetDefaultView()
    {
        SetSprite(_currentImage, _defaultSprite, Color.white);
        SetTextColor(_defaultColor);
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
        BaseQuest.IsReadyForComplete -= OnReadyForComplete;
    }
}
