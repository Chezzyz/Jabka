using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    private Transform _playerModel;

    private void OnEnable()
    {
        LongSuperJump.LongJumpStarted += OnLongJumpStarted;
        DashSuperJump.DashJumpDashed += OnDashJumpDashed;
    }

    private void OnDashJumpDashed(float duration)
    {
        _playerModel.DOLocalRotate(new Vector3(0, 0, 360f), duration, RotateMode.FastBeyond360);
    }

    private void OnLongJumpStarted(float duration)
    {
        _playerModel.DOLocalRotate(new Vector3(0, 360f, 0), duration * 0.75f, RotateMode.FastBeyond360);
    }

    private void OnDestroy()
    {
        LongSuperJump.LongJumpStarted -= OnLongJumpStarted;
        DashSuperJump.DashJumpDashed -= OnDashJumpDashed;
    }
}
