using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField]
    private SceneLoader _sceneLoader;
    [SerializeField]
    private float _delay;

    private void OnEnable()
    {
        DeadZone.PlayerFall += OnPlayerFall;
    }

    private void OnPlayerFall(PlayerTransformController playerTransformController)
    {
        StartCoroutine(ReloadSceneAfterDelay(_delay));
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _sceneLoader.LoadCurrentScene();
    }

    private void OnDisable()
    {
        DeadZone.PlayerFall -= OnPlayerFall;
    }
}
