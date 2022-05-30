using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : BaseQuestItem
{
    public static event System.Action<Collectable> Collected;

    protected override void SendEvent()
    {
        Collected?.Invoke(this);
        Debug.Log($"Collectable for quest on level {SceneStatus.GetCurrentLevelNumber()} collected");
    }
}
