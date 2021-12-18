using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [Inject]
    public void Construct(PlayerTransformController playerTransform)
    {
        playerTransform.SetPosition(transform.position);
        playerTransform.SetRotation(transform.rotation.eulerAngles);
    }
}
