using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Object _mainMenuScene;
    [SerializeField]
    private Object _nextLevelScene;
    [SerializeField]
    private float _sceneTransitionDuration;
    [SerializeField]
    private float _retryTransitionDuration;

    public static event System.Action<string> SceneLoaded;
    public static event System.Action<float> SceneLoadStarted;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay(sceneName, _sceneTransitionDuration));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay = -1)
    {
        if(delay == -1)
        {
            delay = _sceneTransitionDuration;
        }
        if (gameObject != null)
        {
            SceneLoadStarted?.Invoke(delay);
        }
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu()
    {
        if(_mainMenuScene is SceneAsset)
        {
            LoadScene(_mainMenuScene.name);
        }
    }

    public void LoadNextLevel()
    {
        if (_nextLevelScene is SceneAsset)
        {
            LoadScene(_nextLevelScene.name);
        }
    }

    public void LoadCurrentScene()
    {
        StartCoroutine(LoadSceneWithDelay(SceneManager.GetActiveScene().name, _retryTransitionDuration));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(gameObject.activeInHierarchy)
        SceneLoaded?.Invoke(scene.name);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
