using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SuperJumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private Sprite _defaultSprite;
    [SerializeField]
    private Sprite _selectedSprite;
    [SerializeField]
    private BaseJump _superJump;
    [SerializeField]
    private float _clickTreshhold;
    [SerializeField]
    private float _holdTreshhold;

    private Image _currentImage;
    private SuperJumpsWheel _parentWheel;

    private bool _isLocked = true;
    private bool _isSelected = false;
    private bool _isClick = false;

    private float _clickTimer;

    public static event System.Action<ISuperJump> SuperJumpSelected;
    public static event System.Action SuperJumpButtonClicked;
    public static event System.Action<ScriptableJumpData> SuperJumpButtonHolded;
    public static event System.Action SuperJumpButtonReleased;

    private void OnEnable()
    {
        SuperJumpUnlocker.SuperJumpUnlocked += OnButtonUnlocked;
        SuperJumpSelected += OnSuperJumpSelected;
    }

    private void Start()
    {
        _currentImage = GetComponent<Image>();
        _parentWheel = GetComponentInParent<SuperJumpsWheel>();
    }

    public void Select()
    {
        SuperJumpSelected?.Invoke(GetSuperJump());
        _isSelected = true;
    }

    public bool IsLocked()
    {
        return _isLocked;
    }

    public void SetFade(float value)
    {
        Color current = _currentImage.color;
        _currentImage.color = new Color(current.r, current.g, current.b, Mathf.Clamp01(value));
    }

    public void SetScale(float value)
    {
        transform.localScale = new Vector3(value, value, 1);
    }

    private void OnSuperJumpSelected(ISuperJump superJump)
    {
        if(_superJump != null && GetSuperJump().GetJumpName() != superJump.GetJumpName())
        {
            _isSelected = false;
        }
    }

    private void OnButtonUnlocked(ISuperJump unlockedSuperJump)
    {
        if (_superJump != null && GetSuperJump().GetJumpName() == unlockedSuperJump.GetJumpName())
        {
            _isLocked = false;
            GetComponent<Image>().sprite = _defaultSprite;
        }
    }

    private ISuperJump GetSuperJump()
    {
        return (ISuperJump)_superJump;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ((IPointerDownHandler)_parentWheel).OnPointerDown(eventData);
        
        if (_isSelected && !_isLocked)
        {
            _currentImage.sprite = _selectedSprite;
            StartCoroutine(StartClickTimer());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((IPointerUpHandler)_parentWheel).OnPointerUp(eventData);

        if (!_isLocked)
        {
            _currentImage.sprite = _defaultSprite;
        }
        
        if (_isClick && _isSelected && !_isLocked && !eventData.dragging)
        {
            SuperJumpButtonClicked?.Invoke();
        }
        SuperJumpButtonReleased?.Invoke();
        StopAllCoroutines();
    }

    public void OnDrag(PointerEventData eventData)
    {
        ((IDragHandler)_parentWheel).OnDrag(eventData);
    }

    private IEnumerator StartClickTimer()
    {
        _isClick = true;
        _clickTimer = 0;

        while (true)
        {
            _clickTimer += Time.deltaTime;
            
            if(_clickTimer > _clickTreshhold)
            {
                _isClick = false;
            }
            if(_clickTimer > _holdTreshhold)
            {
                SuperJumpButtonHolded?.Invoke(_superJump.GetJumpData());
            }

            yield return null;
        }
    } 

    private void OnDisable()
    {
        SuperJumpUnlocker.SuperJumpUnlocked -= OnButtonUnlocked;
        SuperJumpSelected -= OnSuperJumpSelected;
    }
}
