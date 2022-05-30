using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Progress", menuName = "ScriptableObjects/MetaData/Progress", order = 2)]
public class ProgressData : ScriptableObject
{
    [SerializeField]
    private int _currentStage = 1;
    [SerializeField]
    private int _currentLevel = 1;
    [SerializeField]
    private LevelMetaDatas _levelMetaDatas;

    public int GetCurrentStage()
    {
        return _currentStage;
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public int GetCompletedQuestsCount()
    {
        int sum = 0;
        _levelMetaDatas.GetLevelMetaDatas().ForEach(data => data.GetQuests().ForEach(quest => sum += quest.IsCompleted() ? 1 : 0));
        return sum;
    }

    public int GetQuestsCount()
    {
        int sum = 0;
        _levelMetaDatas.GetLevelMetaDatas().ForEach(data => data.GetQuests().ForEach(quest => sum += 1));
        return sum;
    }

    public void SetCurrentStage(int stage) 
    { 
        _currentStage = stage; 
    }

    public void SetCurrentLevel(int level) 
    { 
        _currentLevel = level; 
    }
}
