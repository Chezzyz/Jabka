using System;
using UnityEngine;


public class SuperJumpPicker : MonoBehaviour
{
    [SerializeField]
    private SuperJumpButton _defaultSuperJump;

    public bool IsActive;

    private JumpController _jumpController;

    private void OnEnable()
    {
        SuperJumpButton.SuperJumpSelected += OnSuperJumpSelected;
    }

    [Zenject.Inject]
    public void Consruct(JumpController jumpController)
    {
        _jumpController = jumpController;
    }

    private void Start()
    {
        if (gameObject.activeInHierarchy)
        {
            _defaultSuperJump.Select();
        }
    }

    private void OnSuperJumpSelected(ISuperJump superJump)
    {
        _jumpController.SetSuperJump(superJump);
    }

    private void OnDisable()
    {
        SuperJumpButton.SuperJumpSelected -= OnSuperJumpSelected;
    }

}