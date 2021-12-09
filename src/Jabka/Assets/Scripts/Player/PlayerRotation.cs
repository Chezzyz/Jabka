using UnityEngine;
using Zenject;
using System.Collections;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private float _sensetivity = 0.1f;

    private PlayerTransformController _playerTransformController;
    private float _originRotationY;
    private bool _canRotate = true;

    [Inject]
    public void Construct(PlayerTransformController transformController)
    {
        _playerTransformController = transformController;
    }

    private void OnEnable()
    {
        InputHandler.SwipeDeltaChanged += OnSwipeX;
        InputHandler.FingerDown += OnFingerDown;
        SuperJumpPicker.SuperJumpMenuStateChanged += OnPickerMenuStateChanged;
    }

    private void OnPickerMenuStateChanged(bool state)
    {
        if(state == true)
        {
            _canRotate = false;
        } 
        else
        {
            //���������� �������. ����� �� ���� �������� ������ ��� ����� ������,
            //��-�� ����� � ��������� ������ ������ �������.
            //����������� ��������� ����� �������� ���� ����� ����� ��������.
            StartCoroutine(SetRotateAfterDelay(true, 0.01f));
        }
    }

    private IEnumerator SetRotateAfterDelay(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        _canRotate = value;
    }

    private void OnFingerDown(Vector2 screenPosition)
    {
        if (_canRotate)
        {
            _originRotationY = _playerTransformController.GetRotation().y;
        }
    }

    private void OnSwipeX(Vector2 delta)
    {
        //��� �������� ������ ������, ������ �������������� ����� � ��������.
        if (_canRotate)
        {
            _playerTransformController.SetRotationY(_originRotationY + (-1 * _sensetivity * delta.x));
        }
    }
}
