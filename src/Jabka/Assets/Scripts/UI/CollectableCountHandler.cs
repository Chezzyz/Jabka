using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CollectableCountHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _countText;
    [SerializeField]
    private float _animationPunchScale;
    [SerializeField]
    private float _animationPunchDuration;

    private int _countOfCollectables;

    private int _currentCount;

    private void OnEnable()
    {
        Collectable.Collected += OnCollected;
    }

    private void Start()
    {
        _countOfCollectables = FindObjectsOfType<Collectable>().Length;
        _countText.text = $"{_currentCount}/{_countOfCollectables}";
    }

    public int GetCountOfCollectables()
    {
        return _countOfCollectables;
    }

    public int GetCurrentCountOfCollectables()
    {
        return _currentCount;
    }

    private void OnCollected(Collectable collectable)
    {
        _currentCount++;
        Tween unpunch = _countText.transform.DOScale(Vector3.one, _animationPunchDuration).SetEase(Ease.InQuad).Pause();
        _countText.transform.DOScale(Vector3.one * _animationPunchScale, _animationPunchDuration).SetEase(Ease.InCubic)
            .OnComplete(() => 
            {
                _countText.text = $"{_currentCount}/{_countOfCollectables}";
                unpunch.Play();
            });
        
    }

    private void OnDisable()
    {
        Collectable.Collected -= OnCollected;
    }
}
