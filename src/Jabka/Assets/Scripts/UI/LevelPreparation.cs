using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPreparation : MonoBehaviour
{
    public static event Action PlayButtonPushed;

    public static event Action QuestAppearStarted;

    [SerializeField]
    private List<QuestView> _questViews;

    [SerializeField]
    private float _allAppearDuration;

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        SceneStatus.SceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(int prevScene, int currentScene)
    {
        if (prevScene == currentScene)
        {
            gameObject.SetActive(false);
        }
        else
        {
            ShowQuests(_questViews, _allAppearDuration);
            _playButton.onClick.AddListener(StartLevel);
        }
    }

    private void StartLevel()
    {
        PlayButtonPushed?.Invoke();
        _canvasGroup.DOFade(0, 1f).onComplete = () => _canvasGroup.gameObject.SetActive(false);
    }

    private void ShowQuests(List<QuestView> questViews, float appearDuration)
    {
        //gridLayoutGroup.enabled = false;

        Sequence sequence = DOTween.Sequence();

        foreach (var quest in questViews)
        {
            sequence.Append(quest.GetComponent<RectTransform>().DOAnchorPosX(0, appearDuration / questViews.Count).OnStart(() => QuestAppearStarted?.Invoke()));
        }

        sequence.onComplete = () =>
        {
            //gridLayoutGroup.enabled = true;
            _playButton.gameObject.SetActive(true);
        };
    }

    private void OnDisable()
    {
        SceneStatus.SceneChanged -= OnSceneChanged;
    }
}
