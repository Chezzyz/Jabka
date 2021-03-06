using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider))]
public class MoveOnTouch : MonoBehaviour
{
    public static event System.Action<GenericPlatformMove> PlayerTouched;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerTransformController>(out _))
        {
            GenericPlatformMove[] generics = GetComponentsInParent<GenericPlatformMove>();
            generics.ToList().ForEach(generic => PlayerTouched?.Invoke(generic));
        }
    }
}
