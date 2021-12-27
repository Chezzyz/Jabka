using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private JumpController _jumpController;

    private Image _currentJumpImage;
    
    private Sprite _selectedJumpSprite;

    public static event System.Action<ISuperJump> SuperJumpPicked;

    public static event System.Action<bool> SuperJumpMenuStateChanged;

    private SuperJumpButton[] _buttons;

    //имеется ввиду выделенное в UI
    private ISuperJump _selectedJump;

    private void OnEnable()
    {
        SuperJumpButton.SuperJumpSelected += SetSelectedSuperJump;
        SuperJumpButton.SuperJumpUnselected += UnsetSuperJump;
        SuperJumpUnlocker.SuperJumpsUnlocked += OnSuperJumpsUnlocked;
    }

    [Zenject.Inject]
    public void Consruct(JumpController jumpController)
    {
        _jumpController = jumpController;
    }

    private void Start()
    {
        _buttons = GetComponentsInChildren<SuperJumpButton>();
        _currentJumpImage = GetComponent<Image>();
        
        //делаю полупрозрачной
        _currentJumpImage.color = new Color(1, 1, 1, _unselectedAlpha);

        if (gameObject.activeInHierarchy)
        {
            _jumpController.SetSuperJump(_defaultSuperJump.GetComponent<ISuperJump>());
        }
    }

    private void OnSuperJumpsUnlocked()
    {
        GetComponent<Image>().sprite = _defaultSuperJump.GetComponent<Image>().sprite;
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
        SuperJumpMenuStateChanged?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_selectedJump != null)
        {
            PickSuperJump(_selectedJump, _selectedJumpSprite);
        }

        ClosePickerMenu(_buttons, _closeAnimationDuration, _closeEase);
        SuperJumpMenuStateChanged?.Invoke(false);
    }

    private void PickSuperJump(ISuperJump pickedJump, Sprite pickedSprite)
    {
        SuperJumpPicked?.Invoke(pickedJump);
        _currentJumpImage.sprite = pickedSprite;
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
            //величина угла одного сегмента * номер сегмента
            float angle = (halfCircle / (1 + count)) * (1 + i);
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            Image buttonImage = button.GetComponent<Image>();

            StopTweensOfObjects(button.transform, buttonImage, _currentJumpImage);

            button.transform.DOLocalMove(new Vector3(x * radius, y * radius, 0), duration).SetEase(ease)
                .OnStart(() => button.SetIsSelectable(true, duration/2));
            buttonImage.DOFade(1, duration);
            _currentJumpImage.DOFade(1, duration);
        }
    }

    private void ClosePickerMenu(SuperJumpButton[] buttons, float duration, Ease ease)
    {
        int count = buttons.Length;
        for (int i = 0; i < count; i++)
        {
            SuperJumpButton button = buttons[i];

            Image buttonImage = button.GetComponent<Image>();

            StopTweensOfObjects(button.transform, buttonImage, _currentJumpImage);

            button.transform.DOLocalMove(Vector3.zero, duration).SetEase(ease)
                .OnStart(() => button.SetIsSelectable(false)); ;
            buttonImage.DOFade(0, duration);
            _currentJumpImage.DOFade(_unselectedAlpha, duration);
        }
    }

    private int StopTweensOfObjects(params Component[] components)
    {
        int killed = 0;
        components.ToList().ForEach(component => killed += component.DOKill());
        return killed;
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
        SuperJumpUnlocker.SuperJumpsUnlocked -= OnSuperJumpsUnlocked;
    }
}