using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SaveLoadSystem : BaseGameHandler<SaveLoadSystem>
{
    [SerializeField]
    private GameStateMap _gameStateMap;

    public static event Action SaveLoaded;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        SceneLoader.SceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Load();
    }

    private void OnSceneLoaded(string _)
    {
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(CompletePlace _)
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        SaveInts();
        SaveFloats();
        SaveBools();
        PlayerPrefs.Save();
    }

    #region Save
    private void SaveInts()
    {
        Dictionary<string, Func<int>> intValuesMap = _gameStateMap.GetIntGameValuesMap();

        intValuesMap
            .Keys
            .ToList()
            .ForEach(alias => SaveInt(alias, intValuesMap[alias].Invoke()));
    }

    private void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        Debug.Log($"Key {key} with value {value} saved");
    }

    private void SaveFloats()
    {
        Dictionary<string, Func<float>> floatValuesMap = _gameStateMap.GetFloatGameValuesMap();

        floatValuesMap
            .Keys
            .ToList()
            .ForEach(alias => SaveFloat(alias, floatValuesMap[alias].Invoke()));
    }

    private void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        Debug.Log($"Key {key} with value {value} saved");
    }

    private void SaveBools()
    {
        Dictionary<string, Func<bool>> boolValuesMap = _gameStateMap.GetBoolGameValuesMap();

        boolValuesMap
            .Keys
            .ToList()
            .ForEach(alias => SaveBool(alias, boolValuesMap[alias].Invoke() ? 1 : 0));
    }

    private void SaveBool(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        Debug.Log($"Key {key} with value {value} saved");
    }
    #endregion

    private void Load()
    {
        LoadInts();
        LoadFloats();
        LoadBools();
        SaveLoaded?.Invoke();
    }

    #region Load
    private void LoadInts()
    {
        Dictionary<string, Action<int>> intMap = _gameStateMap.GetIntGameValueSettersMap();

        intMap
            .Keys
            .ToList()
            .ForEach(alias => LoadInt(alias, intMap[alias]));
    }

    private void LoadInt(string key, Action<int> action)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int value = PlayerPrefs.GetInt(key);
            action.Invoke(value);
            Debug.Log($"Key {key} loaded with value {value}");
        } 
        else
        {
            Debug.LogWarning($"Key {key} not found in PlayerPrefs");
        }
    }

    private void LoadFloats()
    {
        Dictionary<string, Action<float>> floatMap = _gameStateMap.GetFloatGameValueSettersMap();

        floatMap
            .Keys
            .ToList()
            .ForEach(alias => LoadFloat(alias, floatMap[alias]));
    }

    private void LoadFloat(string key, Action<float> action)
    {
        if (PlayerPrefs.HasKey(key))
        {
            float value = PlayerPrefs.GetFloat(key);
            action.Invoke(value);
            Debug.Log($"Key {key} loaded with value {value}");
        }
        else
        {
            Debug.LogWarning($"Key {key} not found in PlayerPrefs");
        }
    }

    private void LoadBools()
    {
        Dictionary<string, Action<bool>> boolMap = _gameStateMap.GetBoolGameValueSettersMap();

        boolMap
            .Keys
            .ToList()
            .ForEach(alias => LoadBool(alias, boolMap[alias])); 
    }

    private void LoadBool(string key, Action<bool> action)
    {
        if (PlayerPrefs.HasKey(key))
        {
            bool value = PlayerPrefs.GetInt(key) == 1;
            action.Invoke(value);
            Debug.Log($"Key {key} loaded with value {value}");
        }
        else
        {
            Debug.LogWarning($"Key {key} not found in PlayerPrefs");
        }
    }
    #endregion

    [Button]
    public void ResetSaves()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All values reseted");
    }
}
