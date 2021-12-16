using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableCountHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _countText;

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

    private void OnCollected(Collectable collectable)
    {
        _currentCount++;
        _countText.text = $"{_currentCount}/{_countOfCollectables}";
    }
}
