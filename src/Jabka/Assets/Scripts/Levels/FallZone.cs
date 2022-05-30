using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    [SerializeField]
    private float _deadZoneY;

    public static event System.Action<PlayerTransformController> PlayerFall;

    private PlayerTransformController _playerTransformController;

    private bool _eventSended = false;

    [Zenject.Inject]
    public void Construct(PlayerTransformController playerTransformController)
    {
        _playerTransformController = playerTransformController;
    }

    private void OnEnable()
    {
        _eventSended = false;
        Checkpoint.PlayerSpawned += OnPlayerSpawned;
    }

    private void OnPlayerSpawned(PlayerTransformController _)
    {
        _eventSended = false;
    }

    private void Update()
    {
        if(_playerTransformController.GetTransformPosition().y < _deadZoneY && !_eventSended)
        {
            PlayerFall?.Invoke(_playerTransformController);
            _eventSended = true;
        }
    }
}
