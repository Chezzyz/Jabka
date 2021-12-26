using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SuperJumpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Sprite _selectedSprite;
    [SerializeField]
    private Sprite _defaultSprite;

    private ISuperJump _superJump;

    private Image _currentImage;

    private bool _isSelectable = false;

    private bool _isLocked = true;

    public static event System.Action<ISuperJump, Sprite> SuperJumpSelected;
    public static event System.Action SuperJumpUnselected;

    private void OnEnable()
    {
        SuperJumpSelected += OnSelect;
        SuperJumpUnlocker.SuperJumpUnlocked += UnlockButton;
        _currentImage = GetComponent<Image>();
        if (TryGetComponent(out _superJump) == false)
        {
            Debug.Log($"button {gameObject.name} doesn't have ISuperJump field");
        }
    }

    private void UnlockButton(ISuperJump unlockedSuperJump)
    {
        if(_superJump.GetJumpName() == unlockedSuperJump.GetJumpName())
        {
            _isLocked = false;
            _currentImage.sprite = _defaultSprite;
        }
    }

    public void SetIsSelectable(bool value)
    {
        if (_isLocked == false)
        {
            _isSelectable = value;
        }
        StopAllCoroutines();
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
            SuperJumpSelected?.Invoke(_superJump, _defaultSprite);
        }
    }

    private void OnSelect(ISuperJump selected, Sprite sprite)
    {
        if(_superJump.GetJumpName() != selected.GetJumpName())
        {
            SetSelectImageActive(false);
        }
    }

    private void SetSelectImageActive(bool isSelected)
    {
        if (_isSelectable)
        {
            GetComponent<Image>().sprite = isSelected ? _selectedSprite : _defaultSprite;
        }
    }
}
