using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "LevelMetaDataMap", menuName = "ScriptableObjects/MetaData/LevelMetaDataMap", order = 3)]
public class LevelMetaDataMap : ScriptableObject
{
    [SerializeField]
    private List<LevelMetaData> _levelMetaDatas;

    private Dictionary<int, LevelMetaData> _levelMetaDataMap = new Dictionary<int, LevelMetaData>();

    private void OnEnable()
    {
        _levelMetaDatas.ForEach(data => _levelMetaDataMap.Add(data.GetLevelNumber(), data));
    }

    public LevelMetaData GetLevelMetaData(int levelNumber)
    {
        return _levelMetaDataMap[levelNumber];
    }
}
