using System;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _simpleJumpTrail;

    [SerializeField]
    private ParticleSystem _poweredSimpleJumpTrail;

    [SerializeField]
    private ParticleSystem _longJumpTrail;

    [SerializeField]
    private ParticleSystem _jumpDust;

    [SerializeField]
    private ParticleSystem _dashCircles;

    private void OnEnable()
    {
        JumpController.JumpStarted += OnJumpStarted;
        BaseJump.JumpEnded += OnJumpEnded;
    }

    private void OnJumpEnded()
    {
        _simpleJumpTrail.Stop();
        _poweredSimpleJumpTrail.Stop();
        _longJumpTrail.Stop();
    }

    private void OnJumpStarted(float forcePercent, ISuperJump superJump)
    {
        if (forcePercent > 0.25f && forcePercent < 0.75f && superJump == null)
        {
            _simpleJumpTrail.Play();
        }
        else if (forcePercent > 0.75f && superJump == null)
        {
            _poweredSimpleJumpTrail.Play();
            InstantiateEffect(_jumpDust, 2f);
        }
        else if (superJump != null)
        {
            string jumpName = superJump.GetJumpName();

            if (jumpName == "Long")
            {
                _longJumpTrail.Play();
            }
            else if (jumpName == "Dash")
            {
                InstantiateEffect(_dashCircles, 3f);
            }
        }
    }

    private void InstantiateEffect(ParticleSystem particleSystem, float destroyTime)
    {
        var effect = Instantiate(_jumpDust, transform.position, transform.rotation); //Нужно через PlayerTransformController
        Destroy(effect.gameObject, destroyTime);
    }

    private void OnDestroy()
    {
        JumpController.JumpStarted -= OnJumpStarted;
        BaseJump.JumpEnded -= OnJumpEnded;
    }
}
