using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraController : MonoBehaviour
{
    private void OnEnable()
    {
        DeadZone.PlayerFall += OnPlayerFall;
    }

    private void OnPlayerFall(PlayerTransformController playerTransformController)
    {
        GetComponent<CinemachineVirtualCamera>().Follow = null;
    }

    private void OnDisable()
    {
        DeadZone.PlayerFall -= OnPlayerFall;
    }
}
