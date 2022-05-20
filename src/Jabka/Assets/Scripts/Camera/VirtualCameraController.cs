using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraController : MonoBehaviour
{
    private void OnEnable()
    {
        FallZone.PlayerFall += OnPlayerFall;
        Checkpoint.PlayerSpawned += OnPlayerSpawned;
    }

    private void OnPlayerFall(PlayerTransformController playerTransformController)
    {
        GetComponent<CinemachineVirtualCamera>().Follow = null;
    }

    private void OnPlayerSpawned(PlayerTransformController playerTransformController)
    {
        GetComponent<CinemachineVirtualCamera>().Follow = playerTransformController.transform;
    }

    private void OnDisable()
    {
        FallZone.PlayerFall -= OnPlayerFall;
        Checkpoint.PlayerSpawned -= OnPlayerSpawned;
    }
}
