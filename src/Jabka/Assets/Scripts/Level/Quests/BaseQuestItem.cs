using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class BaseQuestItem : MonoBehaviour
{
    [SerializeField]
    protected int _levelNumber;

    public int GetLevelNumber()
    {
        return _levelNumber;
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerTransformController>(out _))
        {
            SendEvent();
        }
    }

    protected abstract void SendEvent();

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}