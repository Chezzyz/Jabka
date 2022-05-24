using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePage : MonoBehaviour
{
    [SerializeField]
    private List<Button> _stageButtons;

    public List<Button> GetStageButtons()
    {
        return _stageButtons;
    }

}
