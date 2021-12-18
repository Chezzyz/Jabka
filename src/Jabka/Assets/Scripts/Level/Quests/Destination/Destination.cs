using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Destination : MonoBehaviour
{
    public static event System.Action<Destination> Destinated;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerTransformController>(out var player))
        {
            Destinated?.Invoke(this);
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
