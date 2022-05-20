using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStatus : MonoBehaviour
{
    [SerializeField]
    List<LevelMetaData> _levelMetaDatas;

    public static Action<int, int> SceneChanged;

    public static SceneStatus Instance;

    private static int _prevSceneIndex = -1;
    private static int _currentSceneIndex = -1;

    private static Dictionary<int, LevelMetaData> _levelMetaDataMap;

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
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneLoader.SceneLoadStarted += OnSceneLoadStarted;
    }

    private void Start()
    {
        InitLevelMetaDataMap();
    }

    public static int GetCurrentLevelNumber()
    {
        return _currentSceneIndex;
    }

    public static LevelMetaData GetLevelMetaData(int levelNumber)
    {
        return _levelMetaDataMap[levelNumber];
    }

    private void InitLevelMetaDataMap()
    {
        _levelMetaDataMap = new Dictionary<int, LevelMetaData>();
        _levelMetaDatas.ForEach(meta => _levelMetaDataMap.Add(meta.GetLevelNumber(), meta));
    }

    private void OnSceneLoadStarted(float delay)
    {
        _prevSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        _currentSceneIndex = scene.buildIndex;
        SceneChanged?.Invoke(_prevSceneIndex, _currentSceneIndex);
    }

}
