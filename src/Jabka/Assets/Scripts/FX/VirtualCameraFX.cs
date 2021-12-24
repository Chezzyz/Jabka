using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraFX : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private Sequence fovSequence;
    float defaultFoV;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();    
    }

    private void OnEnable()
    {
        defaultFoV = _virtualCamera.m_Lens.FieldOfView;
        SimpleJump.SimpleJumpStarted += OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted += OnLongJumpStarted;
        DashSuperJump.DashJumpStarted += OnDashJumpStarted;
        DashSuperJump.DashJumpDashed += OnDashJumpDashed;
    }

    private void OnDashJumpDashed(float duration)
    {
        KillFovSequence();
        FoVEffect(15f, duration, 0.8f);
    }

    private void OnDashJumpStarted(float duration)
    {
        FoVEffect(-5f, duration, 0.9f);
    }

    private void OnLongJumpStarted(float duration)
    {
        FoVEffect(15f, duration, 0.8f);
    }

    private void OnSimpleJumpStarted(float forcePercent, float duration)
    {
        if (forcePercent > 0.75f)
        {
            FoVEffect(10f, duration, 0.2f);
        }
    }

    private void FoVEffect(float newFoV, float duration, float fovEffectPart)
    {
        var fovTween = DOTween.To(() => _virtualCamera.m_Lens.FieldOfView, x => _virtualCamera.m_Lens.FieldOfView = x, defaultFoV + newFoV, duration * fovEffectPart).onComplete = () =>
        {
            DOTween.To(() => _virtualCamera.m_Lens.FieldOfView, x => _virtualCamera.m_Lens.FieldOfView = x, defaultFoV, duration * (1 - fovEffectPart));
        };

        fovSequence.AppendCallback(fovTween);
    }

    private void KillFovSequence()
    {
        fovSequence.Kill();
    }

    private void OnDestroy()
    {
        SimpleJump.SimpleJumpStarted -= OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted -= OnLongJumpStarted;
        DashSuperJump.DashJumpStarted -= OnDashJumpStarted;
        DashSuperJump.DashJumpDashed -= OnDashJumpDashed;
    }
}
