using UnityEngine;
using Zenject;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private float _sensetivity = 0.1f;

    private PlayerTransformController _playerTransformController;
    private float _originRotationY;

    [Inject]
    public void Construct(PlayerTransformController transformController)
    {
        _playerTransformController = transformController;
    }

    private void OnEnable()
    {
        InputHandler.SwipeDeltaChanged += OnSwipeX;
        InputHandler.FingerDown += OnFingerDown;
    }

    private void OnFingerDown(Vector2 screenPosition)
    {
        _originRotationY = _playerTransformController.GetRotation().y;
    }

    private void OnSwipeX(Vector2 delta)
    {
        //при движении пальца вправо, камера поворачивается влево и наоборот.
        _playerTransformController.SetRotationY(_originRotationY + (-1 * _sensetivity * delta.x));
    }
}
