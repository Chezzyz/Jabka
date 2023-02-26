using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class SceneStatus : BaseGameHandler<SceneStatus>
{
    [SerializeField]
    LevelMetaDatas _levelMetaDatas;

    public static Action<int, int> SceneChanged;

    private static int _prevSceneIndex = -1;
    private static int _currentSceneIndex = -1;

    private static Dictionary<int, LevelMetaData> _levelMetaDataMap;

    private static Dictionary<string, int> _sceneToLevelNumberMap = new Dictionary<string, int>()
    {
        { "SashaScene", -1 },
        { "DenisScene", -1 },
        { "MenuScene", 0 },
        { "Level_1-1", 1 },
        { "Level_1-2", 2 },
        { "Level_1-3", 3 },
        { "Level_1-4", 4 },
        { "Level_1-5", 5 }
    };

    protected override void Awake()
    {
        base.Awake();
        InitLevelMetaDataMap();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneLoader.SceneLoadStarted += OnSceneLoadStarted;
    }

    public static int GetCurrentLevelNumber()
    {
        return _sceneToLevelNumberMap[SceneManager.GetActiveScene().name];
    }

    public static int GetCurrentStageNumber()
    {
        return GetCurrentLevelNumber() == 0 ? 0 : GetLevelMetaData(GetCurrentLevelNumber()).GetStageNumber();
    }

    public static LevelMetaData GetLevelMetaData(int levelNumber)
    {
        return _levelMetaDataMap[levelNumber];
    }

    public static LevelMetaData GetCurrentMetaData()
    {
        return GetLevelMetaData(GetCurrentLevelNumber());
        
    }

    private void InitLevelMetaDataMap()
    {
        _levelMetaDataMap = new Dictionary<int, LevelMetaData>();
        _levelMetaDatas.GetLevelMetaDatas().ForEach(meta => _levelMetaDataMap.Add(meta.GetLevelNumber(), meta));
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

    [Button]
    public void ClearQuests()
    {
        _levelMetaDatas.GetLevelMetaDatas().ForEach(data => data.GetQuests().ForEach(quest => quest.SetIsCompleted(false)));
        Debug.Log("All quests reseted");
    }
}
