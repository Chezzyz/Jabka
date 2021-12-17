using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Collectable : MonoBehaviour
{
    public static event System.Action<Collectable> Collected;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.TryGetComponent<PlayerTransformController>(out var player))
        {
            Collected?.Invoke(this);
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
