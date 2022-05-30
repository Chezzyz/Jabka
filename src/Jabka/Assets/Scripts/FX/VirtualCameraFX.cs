using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraFX : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    private float _defaultFoV;

    private Tween _currentFovTween;

    private Tween _onCompleteFovTween;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();    
    }

    private void OnEnable()
    {
        _defaultFoV = _virtualCamera.m_Lens.FieldOfView;
        SimpleJump.SimpleJumpStarted += OnSimpleJumpStarted;
        JumpController.SimpleJumpCancelled += OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted += OnLongJumpStarted;
        DashSuperJump.DashPreparingStarted += OnDashPreparingStarted;
        DashSuperJump.DashJumpDashed += OnDashJumpDashed;
        JumpController.ForceChanged += OnForcedChanged;
    }

    private void OnForcedChanged(ScriptableJumpData jumpData)
    {
        if(jumpData is SimpleJumpData data && data.GetForcePercent() > 0)
        {
            KillCurrentFovTween();
            _virtualCamera.m_Lens.FieldOfView = _defaultFoV + (data.GetForcePercent() * 10);
        }
    }

    private void OnDashPreparingStarted(float duration)
    {
        FoVEffect(-10f, duration, 0.9f);
    }

    private void OnDashJumpDashed(float duration)
    {
        KillCurrentFovTween();
        FoVEffect(30f, duration, 0.2f);
    }

    private void OnLongJumpStarted(float duration)
    {
        FoVEffect(15f, duration, 0.2f);
    }

    private void OnSimpleJumpStarted(float forcePercent, float duration)
    {
        if (forcePercent > 0)
        {
            FoVEffect(15f * forcePercent, duration * 0.75f, 0.2f);
        }
    }

    private void FoVEffect(float offsetFoV, float duration, float fovEffectPart)
    {
        _currentFovTween = DOTween.To(() => _virtualCamera.m_Lens.FieldOfView, x => _virtualCamera.m_Lens.FieldOfView = x, _defaultFoV + offsetFoV, duration * fovEffectPart);
        _onCompleteFovTween = DOTween.To(() => _virtualCamera.m_Lens.FieldOfView,x => _virtualCamera.m_Lens.FieldOfView = x, _defaultFoV, duration * (1 - fovEffectPart)).Pause();
        _currentFovTween.onComplete = () => _onCompleteFovTween.Play();
        _currentFovTween.Play();
    }

    private void KillCurrentFovTween()
    {
        _currentFovTween.Kill();
        _onCompleteFovTween.Kill();
    }

    private void OnDisable()
    {
        SimpleJump.SimpleJumpStarted -= OnSimpleJumpStarted;
        JumpController.SimpleJumpCancelled -= OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted -= OnLongJumpStarted;
        DashSuperJump.DashPreparingStarted -= OnDashPreparingStarted;
        DashSuperJump.DashJumpDashed -= OnDashJumpDashed;
        JumpController.ForceChanged -= OnForcedChanged;
    }
}
