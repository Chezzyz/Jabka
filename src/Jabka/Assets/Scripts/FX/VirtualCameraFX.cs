using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraFX : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    float defaultFoV;

    Tween currentFovTween;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();    
    }

    private void OnEnable()
    {
        defaultFoV = _virtualCamera.m_Lens.FieldOfView;
        SimpleJump.SimpleJumpStarted += OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted += OnLongJumpStarted;
        DashSuperJump.DashPreparingStarted += OnDashPreparingStarted;
        DashSuperJump.DashJumpDashed += OnDashJumpDashed;

        JumpController.ForceChanged += OnForcedChanged;
    }

    private void OnForcedChanged(JumpData jumpData, PlayerTransformController playerTransform)
    {
        _virtualCamera.m_Lens.FieldOfView = defaultFoV + (jumpData.ForcePercent * 10);
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
        if (forcePercent > 0.75f)
        {
            FoVEffect(20f, duration, 0.8f);
        }
        else
        { 
            FoVEffect(10f, duration, 0.3f);
        }
    }

    private void FoVEffect(float newFoV, float duration, float fovEffectPart)
    {
        currentFovTween = DOTween.To(() => _virtualCamera.m_Lens.FieldOfView, x => _virtualCamera.m_Lens.FieldOfView = x, defaultFoV + newFoV, duration * fovEffectPart);

        currentFovTween.onComplete = () =>
        {
            DOTween.To(() => _virtualCamera.m_Lens.FieldOfView,
                x => _virtualCamera.m_Lens.FieldOfView = x,
                defaultFoV, duration * (1 - fovEffectPart));
        };
    }

    private void KillCurrentFovTween()
    {
        currentFovTween.Kill();
    }

    private void OnDisable()
    {
        SimpleJump.SimpleJumpStarted -= OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted -= OnLongJumpStarted;
        DashSuperJump.DashPreparingStarted -= OnDashPreparingStarted;
        DashSuperJump.DashJumpDashed -= OnDashJumpDashed;
        JumpController.ForceChanged -= OnForcedChanged;
    }
}
