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
        ShowQuests(_questViews, _allAppearDuration);
        _playButton.onClick.AddListener(StartLevel);
        SceneStatus.SceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(int prevScene, int currentScene)
    {
        if (prevScene == currentScene)
        {
            gameObject.SetActive(false);
        }
    }

    private void StartLevel()
    {
        PlayButtonPushed?.Invoke();
        _canvasGroup.DOFade(0, 1f).onComplete = () =>
        {
            gameObject.SetActive(false);
        };
    }

    private void ShowQuests(List<QuestView> questViews, float appearDuration)
    {
        //gridLayoutGroup.enabled = false;

        Sequence sequence = DOTween.Sequence();

        foreach (var quest in questViews)
        {
            quest.transform.localPosition += new Vector3(1000f, 0f, 0f);
            sequence.Append(quest.transform.DOLocalMoveX(0, appearDuration / questViews.Count));
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
