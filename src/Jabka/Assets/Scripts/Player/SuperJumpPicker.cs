using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SuperJumpPicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject _defaultSuperJump;
    [SerializeField]
    private float _menuRadius;
    [SerializeField]
    private float _unselectedAlpha;
    [SerializeField]
    private float _showAnimationDuration;
    [SerializeField]
    private float _closeAnimationDuration;
    [SerializeField]
    Ease _showEase;
    [SerializeField]
    Ease _closeEase;

    private Image _currentJumpSprite;
    
    private Sprite _selectedJumpSprite;

    public static event System.Action<ISuperJump> SuperJumpPicked;

    private SuperJumpButton[] _buttons;

    //������� ����� ���������� � UI
    private ISuperJump _selectedJump;

    private void OnEnable()
    {
        SuperJumpButton.SuperJumpSelected += SetSelectedSuperJump;
        SuperJumpButton.SuperJumpUnselected += UnsetSuperJump;

        _buttons = GetComponentsInChildren<SuperJumpButton>();
        _currentJumpSprite = GetComponent<Image>();
        _currentJumpSprite.sprite = _defaultSuperJump.GetComponent<Image>().sprite;
        //����� ��������������
        _currentJumpSprite.color = new Color(1, 1, 1, _unselectedAlpha);
    }

    public ISuperJump GetDefaultSuperJump()
    {
        if (_defaultSuperJump.TryGetComponent<ISuperJump>(out var defaultSuperJump) == false)
        {
            Debug.Log("SuperJumpPicker doesn't have a default super jump");
            return null;
        }

        return defaultSuperJump;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowPickerMenu(_buttons, _showAnimationDuration, _menuRadius, _showEase);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_selectedJump != null)
        {
            PickSuperJump(_selectedJump, _selectedJumpSprite);
        }

        ClosePickerMenu(_buttons, _closeAnimationDuration, _closeEase);
    }

    private void PickSuperJump(ISuperJump pickedJump, Sprite pickedSprite)
    {
        SuperJumpPicked?.Invoke(pickedJump);
        GetComponent<Image>().sprite = pickedSprite;
        _selectedJumpSprite = null;
        _selectedJump = null;
    }

    private void ShowPickerMenu(SuperJumpButton[] buttons, float duration, float radius, Ease ease)
    {
        int count = buttons.Length;
        for(int i = 0; i < count; i++)
        {
            SuperJumpButton button = buttons[i];

            float halfCircle = 180;
            //�������� ���� ������ �������� * ����� ��������
            float angle = (halfCircle / (1 + count)) * (1 + i);
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            button.transform.DOLocalMove(new Vector3(x * radius, y * radius, 0), duration).SetEase(ease)
                .OnStart(() => button.SetIsSelectable(true, duration/2));
            button.GetComponent<Image>().DOFade(1, duration);
            _currentJumpSprite.DOFade(1, duration);
        }
    }

    private void ClosePickerMenu(SuperJumpButton[] buttons, float duration, Ease ease)
    {
        int count = buttons.Length;
        for (int i = 0; i < count; i++)
        {
            SuperJumpButton button = buttons[i];

            button.transform.DOLocalMove(Vector3.zero, duration).SetEase(ease)
                .OnStart(() => button.SetIsSelectable(false)); ;
            button.GetComponent<Image>().DOFade(0, duration);
            _currentJumpSprite.DOFade(_unselectedAlpha, duration);
        }
    }

    private void SetSelectedSuperJump(ISuperJump superJump, Sprite sprite)
    {
        _selectedJump = superJump;
        _selectedJumpSprite = sprite;
    }

    private void UnsetSuperJump()
    {
        _selectedJump = null;
    }

    private void OnDisable()
    {
        SuperJumpButton.SuperJumpSelected -= SetSelectedSuperJump;
        SuperJumpButton.SuperJumpUnselected -= UnsetSuperJump;
    }
}