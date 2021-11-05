using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private float _sensetivity = 1;

    private PlayerTransformController _transformController;

    [Inject]
    public void Construct(PlayerTransformController transformController)
    {
        _transformController = transformController;
    }

    private void OnEnable()
    {
        InputHandler.SwipeDeltaChanged += OnSwipeX;
    }

    private void OnSwipeX(Vector2 delta)
    {
        //при движении пальца вправо, камера поворачивается влево и наоборот.
        _transformController.SetRotationY(-1 * _sensetivity * delta.x);
    }
}
