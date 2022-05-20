using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    private static CheckpointHandler _singleton;

    private static Checkpoint _lastCheckpoint;

    private void Awake()
    {
        if (_singleton == null)
        {
            _singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneStatus.SceneChanged += OnSceneChanged;
        Checkpoint.CheckpointActivated += OnCheckpointActivated;
    }

    public static Checkpoint GetLastCheckpoint()
    {
        return _lastCheckpoint;
    }

    private void OnSceneChanged(int prevIndex, int currentIndex)
    {
        //Грузимся на первом чекпоинте если зашли на уровень не с рестарта
        if(prevIndex != currentIndex && currentIndex != 0)
        {
            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
            _lastCheckpoint = checkpoints.Where(checkpoint => checkpoint.GetOrderNumber() == 1).First();
            _lastCheckpoint?.SpawnPlayer();
        }
    }

    private void OnCheckpointActivated(Checkpoint checkpoint)
    {
        _lastCheckpoint = checkpoint;
    }

}
