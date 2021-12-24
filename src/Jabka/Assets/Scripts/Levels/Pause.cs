using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static event System.Action<bool> PauseStateChanged;
    private void OnEnable()
    {
        SceneLoader.SceneLoadStarted += PauseOff;
    }

    private void OnDisable()
    {
        SceneLoader.SceneLoadStarted -= PauseOff;
    }

    public void PauseOn()
    {
        PauseStateChanged?.Invoke(true);
        Time.timeScale = 0;
    }

    public void PauseOff(float num)
    {
        PauseStateChanged?.Invoke(false);
        Time.timeScale = 1;
    }
}
