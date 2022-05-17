using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStatus : MonoBehaviour
{
    public static SceneStatus Instance;

    public static Action<int, int> SceneChanged;

    private static int _prevScene = -1;
    private static int _currentScene = -1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneLoader.SceneLoadStarted += OnSceneLoadStarted;
        DontDestroyOnLoad(gameObject);
    }

    public static int GetCurrentLevelNumber()
    {
        return _currentScene;
    }

    private void OnSceneLoadStarted(float delay)
    {
        _prevScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        _currentScene = scene.buildIndex;

        SceneChanged?.Invoke(_prevScene, _currentScene);
    }
}
