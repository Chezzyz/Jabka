using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerCamera : MonoBehaviour
{
    private Transform _playerTransform;

    private CinemachineVirtualCamera _virtualCamera;

    [Inject]
    public void Construct(PlayerTransformController playerTransform)
    {
        _playerTransform = playerTransform.transform;
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Awake()
    {
        SetPlayer(_playerTransform);    
    }

    private void SetPlayer(Transform player)
    {
        _virtualCamera.Follow = player;
        _virtualCamera.LookAt = player;
    }
}
