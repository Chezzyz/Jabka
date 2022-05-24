using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class SuperJumpsWheel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    List<SuperJumpButton> _buttons;
    [SerializeField]
    private float _segmentAngle;
    [SerializeField]
    private float _overLimitAngle;
    [SerializeField]
    private float _releaseDuration;

    [Range(0, 1)]
    [SerializeField]
    private float _minimalButtonsFade;
    [Range(0, 1)]
    [SerializeField]
    private float _minimalButtonsScale;

    public static System.Action<bool> DragStateChanged;

    private Vector2 _wheelCenter;
    private Vector2 _dragOriginLastLocalPos;
    private float _dragOriginLastAngleZ;

    private Dictionary<Vector2, Vector2> _quarterToClockwiseMap = new Dictionary<Vector2, Vector2>()
    {
        { new Vector2(1,1), new Vector2(1,-1) },
        { new Vector2(1,-1), new Vector2(-1,-1) },
        { new Vector2(-1,-1), new Vector2(-1,1) },
        { new Vector2(-1,1), new Vector2(1,1) }
    };

    private void Start()
    {
        _wheelCenter = new Vector2(transform.position.x, transform.position.y);
        ChangeButtonsView();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DragStateChanged?.Invoke(true);
        _dragOriginLastLocalPos = eventData.position - _wheelCenter;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        DragStateChanged?.Invoke(false);
        RotateToNearest();
        ChangeButtonsView();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaAngle = CalculateAngle(_dragOriginLastLocalPos, eventData.delta);
        float dragClamp = CalculateDragClamp(deltaAngle);
        
        _dragOriginLastAngleZ = GetCurrentAngleZ();
        RotateWheel(dragClamp);
        _dragOriginLastLocalPos = eventData.position - _wheelCenter;

        ChangeButtonsView();
    }

    private void RotateToNearest()
    {
        transform.DOLocalRotate(new Vector3(0, 0, CalculateNearestButtonRotation(out var button)), _releaseDuration).OnUpdate(() => ChangeButtonsView());
        button.Select();
    }

    private float CalculateNearestButtonRotation(out SuperJumpButton selectedButton)
    {
        float currentAngleZ = GetCurrentAngleZ();

        float minimumDelta = _buttons.Select((button, i) => Mathf.Abs(currentAngleZ - i * _segmentAngle)).Min();

        float angle = 0;
        selectedButton = _buttons[0];
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (minimumDelta.Equals(Mathf.Abs(currentAngleZ - i * _segmentAngle)))
            {
                angle = i * _segmentAngle;
                selectedButton = _buttons[i];
            }
        }
            
        return angle;
    }

    private float CalculateDragClamp(float angle)
    {
        float leftLimit = 0;
        float rightLimit = (_buttons.Where(button => !button.IsLocked()).Count() - 1) * _segmentAngle;

        float currentAngleZ = GetCurrentAngleZ();
        
        float coef = 1;
        //Если мы не у крайних кнопок, то двигаемся с полной скоростью
        if(currentAngleZ + angle < leftLimit)
        {
            coef = 1 - ((currentAngleZ + angle - leftLimit) / -_overLimitAngle);

        }
        if (currentAngleZ + angle > rightLimit)
        {
            coef = 1 - ((currentAngleZ + angle - rightLimit) / _overLimitAngle);
        }
        if(currentAngleZ + angle  > rightLimit + _overLimitAngle || currentAngleZ + angle < leftLimit - _overLimitAngle)
        {
            coef = 0;
        }
        return coef * angle;
    }

    private void ChangeButtonsView()
    {
        float currentAngleZ = GetCurrentAngleZ();

        for (int i = 0; i < _buttons.Count; i++)
        {
            SuperJumpButton current = _buttons[i];
            float distanceToMain = Mathf.Abs(currentAngleZ - i * _segmentAngle);

            float fade = Mathf.Clamp(1 - distanceToMain / _segmentAngle, _minimalButtonsFade, 1);
            current.SetFade(fade);

            float scale = Mathf.Clamp(1 - distanceToMain / _segmentAngle, _minimalButtonsScale, 1);
            current.SetScale(scale);
        }
    }

    private void RotateWheel(float angleZ)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _dragOriginLastAngleZ + angleZ));
    }

    private float CalculateAngle(Vector2 lastPosition, Vector2 delta)
    {
        Vector2 localPointerPosition = lastPosition + delta;
        return Vector2.Angle(lastPosition, localPointerPosition) * -GetAngleSign(GetQuarter(localPointerPosition), delta);
    }

    private Vector2 GetQuarter(Vector2 localPosition)
    {
        return new Vector2(Mathf.Sign(localPosition.x), Mathf.Sign(localPosition.y));
    }

    private int GetAngleSign(Vector2 quarter, Vector2 delta)
    {
        return (int)Mathf.Sign(Vector2.Dot(_quarterToClockwiseMap[quarter], delta));
    }
    
    private float GetCurrentAngleZ()
    {
        float currentAngleZ = transform.localEulerAngles.z;
        return (currentAngleZ > 180) ? currentAngleZ - 360 : currentAngleZ;
    }
}
