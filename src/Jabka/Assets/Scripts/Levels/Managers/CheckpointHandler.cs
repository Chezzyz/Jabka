using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointHandler : GameHandler<CheckpointHandler>
{

    private static Checkpoint _lastCheckpoint;

    protected override void Awake()
    {
        base.Awake();
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
        //�������� �� ������ ��������� ���� ����� �� ������� � ��������/������ �� ������ ���������/������ �� ����
        if (currentIndex != 0) 
        {
            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
            _lastCheckpoint = checkpoints.Where(checkpoint => checkpoint.GetOrderNumber() == 1).First();
            _lastCheckpoint.SpawnPlayer();
        }
    }

    private void OnCheckpointActivated(Checkpoint checkpoint)
    {
        _lastCheckpoint = checkpoint;
    }

}
