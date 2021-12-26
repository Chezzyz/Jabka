using DG.Tweening;
using UnityEngine;

public class ScreenFX : MonoBehaviour
{
    [SerializeField]
    private Material _shockWaveFX;
    private const string SHOCK_WAVE = "ShockWaveProgress";

    private void OnEnable()
    {
        DashSuperJump.DashJumpDashed += OnDashJumpDashed;
    }

    private void OnDashJumpDashed(float duration)
    {
        //_shockWaveFX.SetFloat
        _shockWaveFX.DOFloat(1, SHOCK_WAVE, duration)
        .onComplete = () =>
        {
            _shockWaveFX.SetFloat(SHOCK_WAVE, -0.2f);
        };
    }

    private void OnDisable()
    {
        DashSuperJump.DashJumpDashed -= OnDashJumpDashed;
    }
}
