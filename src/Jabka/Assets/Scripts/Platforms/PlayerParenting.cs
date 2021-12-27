using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParenting : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerTransformController>(out var playerTransformController))
        {
            if (playerTransformController.IsOnHorizontalSurface(-collision.GetContact(0).normal)) 
            {
                playerTransformController.SetTransformParent(transform.parent);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerTransformController>(out var playerTransformController))
        {
            playerTransformController.SetTransformParent(null);
        }
    }
}
