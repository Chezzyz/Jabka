using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMetricaEvents : BaseGameHandler<AppMetricaEvents>
{
    private int _currentDeathCount;
    private int _currentJumpCount;
    private float _currentLevelTimer;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        CompletePlace.LevelCompleted += OnLevelCompleted;
        SceneStatus.SceneChanged += OnSceneChanged;
        FallZone.PlayerFall += OnPlayerFall;
        SimpleJump.SimpleJumpStarted += OnSimpleJumpStarted;
        JumpController.SuperJumpStarted += OnSuperJumpStarted;
    }

    private void OnLevelCompleted(CompletePlace _)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level_number", SceneStatus.GetCurrentLevelNumber());
        parameters.Add("death_count", _currentDeathCount);
        parameters.Add("time_amount", _currentLevelTimer);
        parameters.Add("jump_count", _currentJumpCount);

        ClearSceneParameters();

        AppMetrica.Instance.ReportEvent("level_finished", parameters);
        AppMetrica.Instance.SendEventsBuffer();
    }

    private void OnSceneChanged(int prev, int current)
    {
        if (prev != current)
        {
            ClearSceneParameters();
            StartCoroutine(LevelTimerCoroutine());
        }
    }

    private IEnumerator LevelTimerCoroutine()
    {
        _currentLevelTimer = 0;
        while (true)
        {
            _currentLevelTimer += Time.deltaTime;
            yield return null;
        }
    }

    private void OnPlayerFall(PlayerTransformController _)
    {
        _currentDeathCount++;
    }

    private void OnSimpleJumpStarted(float _, float _1)
    {
        _currentJumpCount++;
    }

    private void OnSuperJumpStarted(ISuperJump _)
    {
        _currentJumpCount++;
    }

    private void ClearSceneParameters()
    {
        _currentDeathCount = 0;
        _currentLevelTimer = 0;
        _currentJumpCount = 0;
    }
}
