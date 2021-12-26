using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class WinCamera : MonoBehaviour
{
    private void OnEnable()
    {
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(CompletePlace place)
    {
        GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
    }

    private void OnDisable()
    {
        CompletePlace.LevelCompleted -= OnLevelCompleted;
    }
}
