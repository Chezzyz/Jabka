using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SuperJumpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Sprite _selectedImage;

    private Sprite _defaultImage;

    private ISuperJump _superJump;

    private bool _isSelectable = false;

    public static event System.Action<ISuperJump, Sprite> SuperJumpSelected;
    public static event System.Action SuperJumpUnselected;

    private void OnEnable()
    {
        SuperJumpSelected += OnSelect;
    }

    private void Start()
    {
        _defaultImage = GetComponent<Image>().sprite;

        if(TryGetComponent(out _superJump) == false)
        {
            Debug.Log($"button {gameObject.name} doesn't have ISuperJump field");
        }
    }

    public void SetIsSelectable(bool value)
    {
        _isSelectable = value;
    }

    public void SetIsSelectable(bool value, float delay)
    {
        StartCoroutine(SetIsSelectableDelay(value, delay));
    }

    private IEnumerator SetIsSelectableDelay(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetIsSelectable(value);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (_isSelectable)
        {
            SetSelectImageActive(false);
            SuperJumpUnselected?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelectable)
        {
            SetSelectImageActive(true);
            SuperJumpSelected?.Invoke(_superJump, _defaultImage);
        }
    }

    private void OnSelect(ISuperJump selected, Sprite sprite)
    {
        if(_superJump != selected)
        {
            SetSelectImageActive(false);
        }
    }

    private void SetSelectImageActive(bool isSelected)
    {
        GetComponent<Image>().sprite = isSelected ? _selectedImage : _defaultImage;
    }
}
