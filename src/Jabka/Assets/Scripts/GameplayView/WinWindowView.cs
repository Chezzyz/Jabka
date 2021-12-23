using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Canvas))]
public class WinWindowView : MonoBehaviour
{
    [SerializeField]
    private Image _background;
    [SerializeField]
    private float _backgroundFadeDuration;
    [SerializeField]
    private TextMeshProUGUI _levelNumber;
    [SerializeField]
    private TextMeshProUGUI _completedText;
    [SerializeField]
    private RectTransform _questsGroup;
    [SerializeField]
    private RectTransform _buttonsGroup;
    [SerializeField]
    private float _groupsAppearDuration;
    [SerializeField]
    private float _groupsAppearDelay;


    private Color _backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.75f);

    private void OnEnable()
    {
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(CompletePlace place)
    {
        GetComponent<Canvas>().enabled = true;
        _background.DOBlendableColor(_backgroundColor, _backgroundFadeDuration);
        _levelNumber.rectTransform.DOMoveY(1780, _groupsAppearDuration).SetDelay(_groupsAppearDelay);
        _completedText.rectTransform.DOMoveY(1680, _groupsAppearDuration).SetDelay(_groupsAppearDelay);
        _buttonsGroup.DOMoveY(-90, _groupsAppearDuration).SetDelay(_groupsAppearDelay);
        _questsGroup.DOLocalMoveX(0, _groupsAppearDuration).SetDelay(_groupsAppearDelay * 2);
    }

    private void OnDisable()
    {
        CompletePlace.LevelCompleted -= OnLevelCompleted;
    }
}
