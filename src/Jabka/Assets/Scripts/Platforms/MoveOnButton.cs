using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnButton : MonoBehaviour
{
    [SerializeField]
    private List<GenericPlatformMove> _platformsToMove;

    public static event System.Action<GenericPlatformMove> ButtonPressed;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerTransformController>(out _))
        {
            foreach(var platform in _platformsToMove)
            {
                ButtonPressed?.Invoke(platform);
            }
        }
    }
}
