using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateMap", menuName = "ScriptableObjects/GameStateMap", order = 5)]
public class GameStateMap : ScriptableObject
{
    [SerializeField]
    private LevelMetaDatas _levelMetaDatas;

    [SerializeField]
    private ProgressData _progressData;

    public static readonly string CURRENT_STAGE_ALIAS = "CurrentStage";

    public static readonly string CURRENT_LEVEL_ALIAS = "CurrentLevel";

    public static readonly string VOLUME_LEVEL_ALIAS = "VolumeLevel";

    public static readonly string MUSIC_IS_ON_ALIAS = "MusicIsOn";

    public static readonly string VERTICAL_SENS_ALIAS = "VerticalSens";

    public static readonly string HORIZONTAL_SENS_ALIAS = "HorizontalSens";

    private static readonly string LEVEL_QUEST_PREFIX_ALIAS = "LevelQuest_";

    private static readonly string LANGUAGE_ALIAS = "Language";

    public Dictionary<string, Func<int>> GetIntGameValuesMap()
    {
        return new Dictionary<string, Func<int>>
        {
            { CURRENT_STAGE_ALIAS, () => _progressData.GetCurrentStage() },
            { CURRENT_LEVEL_ALIAS, () => _progressData.GetCurrentLevel() },
            { VOLUME_LEVEL_ALIAS, () => SettingsHandler.GetVolumeLevel() }
        };
    }

    public Dictionary<string, Action<int>> GetIntGameValueSettersMap()
    {
        return new Dictionary<string, Action<int>>
        {
            { CURRENT_STAGE_ALIAS, (value) => _progressData.SetCurrentStage(value) },
            { CURRENT_LEVEL_ALIAS, (value) => _progressData.SetCurrentLevel(value) },
            { VOLUME_LEVEL_ALIAS, (value) => SettingsHandler.Instance.SetVolumeLevel(value) }
        };
    }

    public Dictionary<string, Func<float>> GetFloatGameValuesMap()
    {
        return new Dictionary<string, Func<float>>
        {
            { VERTICAL_SENS_ALIAS, () => SettingsHandler.GetVerticalSensCoef() },
            { HORIZONTAL_SENS_ALIAS, () => SettingsHandler.GetHorizontalSensCoef()}
        };
    }

    public Dictionary<string, Action<float>> GetFloatGameValueSettersMap()
    {
        return new Dictionary<string, Action<float>>
        {
            { VERTICAL_SENS_ALIAS, (value) => SettingsHandler.Instance.SetVerticalSensetivity(value) },
            { HORIZONTAL_SENS_ALIAS, (value) => SettingsHandler.Instance.SetHorizontalSensetivity(value)}
        };
    }

    public Dictionary<string, Func<bool>> GetBoolGameValuesMap()
    {
        Dictionary<string, Func<bool>> boolsMap = new Dictionary<string, Func<bool>>
        {
            { MUSIC_IS_ON_ALIAS, () => SettingsHandler.MusicIsOn() }
        };

        _levelMetaDatas
            .GetLevelMetaDatas()
            .ForEach(data => data
            .GetQuests()
            .ForEach(quest => boolsMap.Add($"{LEVEL_QUEST_PREFIX_ALIAS}{quest.GetId()}", () => quest.IsCompleted())));

        return boolsMap;
    }

    public Dictionary<string, Action<bool>> GetBoolGameValueSettersMap()
    {
        Dictionary<string, Action<bool>> boolsMap = new Dictionary<string, Action<bool>>
        {
            { MUSIC_IS_ON_ALIAS, (value) => SettingsHandler.Instance.SetMusicIsOn(value) }
        };

        _levelMetaDatas
            .GetLevelMetaDatas()
            .ForEach(data => data
            .GetQuests()
            .ForEach(quest => boolsMap.Add($"{LEVEL_QUEST_PREFIX_ALIAS}{quest.GetId()}", (value) => quest.SetIsCompleted(value))));

        return boolsMap;
    }

    public Dictionary<string, Func<string>> GetStringGameValuesMap()
    {
        return new Dictionary<string, Func<string>>
        {
            { LANGUAGE_ALIAS, () => LanguagePicker.Instance.GetLocale() }
        };
    }

    public Dictionary<string, Action<string>> GetStringGameValueSettersMap()
    {
        return new Dictionary<string, Action<string>>
        {
            { LANGUAGE_ALIAS, (value) => LanguagePicker.Instance.SetLocale(value) }
        };
    }
}
