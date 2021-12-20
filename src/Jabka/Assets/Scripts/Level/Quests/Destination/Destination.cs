using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : BaseQuestItem
{
    public static event System.Action<Destination> Destinated;

    protected override void SendEvent()
    {
        Destinated?.Invoke(this);
        Debug.Log($"Destination for quest {_questId} destinated");
    }
}
