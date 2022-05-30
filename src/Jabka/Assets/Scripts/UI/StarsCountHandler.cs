using TMPro;
using UnityEngine;

public class StarsCountHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private ProgressData _progressData;

    private void OnEnable()
    {
        SaveLoadSystem.SaveLoaded += OnSaveLoaded;
    }

    private void OnSaveLoaded()
    {
        SetStarCountView();
    }

    private void Start()
    {
        SetStarCountView();
    }

    private void SetStarCountView()
    {
        int completed = _progressData.GetCompletedQuestsCount();
        int all = _progressData.GetQuestsCount();
        _text.text = $"{completed}/{all}";
    }
}
