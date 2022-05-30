using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMetaDatas", menuName = "ScriptableObjects/MetaData/LevelMetaDatas", order = 2)]
public class LevelMetaDatas : ScriptableObject
{
    [SerializeField]
    List<LevelMetaData> _levelMetaDatas;

    public List<LevelMetaData> GetLevelMetaDatas()
    {
        return _levelMetaDatas;
    }
}
