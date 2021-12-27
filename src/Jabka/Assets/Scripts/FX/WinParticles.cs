using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinParticles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particles;

    private void OnEnable()
    {
        CompletePlace.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(CompletePlace place)
    {
        _particles.Play();
    }

    private void OnDisable()
    {
        CompletePlace.LevelCompleted -= OnLevelCompleted;
    }
}
