using System.Collections;
using UnityEngine;

public class ReloadOnDead : MonoBehaviour
{
    [SerializeField]
    private SceneLoader _sceneLoader;
    [SerializeField]
    private float _retryTransitionDuration;
    [SerializeField]
    private float _delay;

    public static System.Action<float> LevelReloadStarted;

    private void OnEnable()
    {
        FallZone.PlayerFall += OnPlayerFall;
    }

    private void OnPlayerFall(PlayerTransformController playerTransformController)
    {
        StartCoroutine(ReloadSceneAfterDelay(_delay));
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Checkpoint lastCheckpoint = CheckpointHandler.GetLastCheckpoint();
        //���� ������� �������� ��� ����� ������, �� ������������� ����� ���������, ����� ������ ������������� ������ �� ��������� ��������
        if (lastCheckpoint.GetOrderNumber() == 1)
        {
            //����� � ������������ ����� �� ������, SceneLoader ��� ������
            _sceneLoader.LoadCurrentScene();
        }
        else
        {
            StartCoroutine(lastCheckpoint.SpawnAfterDelay(_retryTransitionDuration));
            LevelReloadStarted?.Invoke(_retryTransitionDuration);
        }
    }

    private void OnDisable()
    {
        FallZone.PlayerFall -= OnPlayerFall;
    }

}
