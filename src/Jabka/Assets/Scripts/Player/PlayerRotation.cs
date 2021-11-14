using UnityEngine;
using Zenject;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private float _sensetivity = 0.1f;

    private PlayerTransformController _transformController;
    private float _originRotationY;

    [Inject]
    public void Construct(PlayerTransformController transformController)
    {
        _transformController = transformController;
    }

    private void OnEnable()
    {
        InputHandler.SwipeDeltaChanged += OnSwipeX;
        InputHandler.FingerDown += OnFingerDown;
    }

    private void OnFingerDown(Vector2 screenPosition)
    {
        _originRotationY = _transformController.transform.localEulerAngles.y;
    }

    private void OnSwipeX(Vector2 delta)
    {
        //при движении пальца вправо, камера поворачивается влево и наоборот.
        _transformController.SetRotationY(_originRotationY + (-1 * _sensetivity * delta.x));
    }
}
