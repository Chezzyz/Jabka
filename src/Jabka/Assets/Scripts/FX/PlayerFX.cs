using System;
using System.Collections;
using Player.Jumps;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _simpleJumpTrail;

    [SerializeField]
    private TrailRenderer _poweredSimpleJumpTrail;

    [SerializeField]
    private ParticleSystem _longJumpCircles;

    [SerializeField]
    private ParticleSystem _jumpDust;

    [SerializeField]
    private ParticleSystem _dashCircles;

    [SerializeField] 
    private ParticleSystem _highJump;
    
    private void OnEnable()
    {
        SimpleJump.SimpleJumpStarted += OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted += OnLongJumpStarted;
        DashSuperJump.DashJumpDashed += OnDashed;
        HighSuperJump.HighJumpStarted += OnHighJumpStarted; 
        BaseJump.JumpEnded += OnJumpEnded;
    }

    private void OnHighJumpStarted(float obj)
    {
        InstantiateEffect(_highJump, Vector3.zero, 2f);
    }

    private void OnLongJumpStarted(float obj)
    {
        _longJumpCircles.Play();
    }

    private void OnSimpleJumpStarted(float forcePercent, float duration)
    {
        if (forcePercent > 0.25f && forcePercent <= 0.75f)
        {
            _simpleJumpTrail.gameObject.SetActive(true);
        }
        else if (forcePercent > 0.75f)
        {
            _poweredSimpleJumpTrail.gameObject.SetActive(true);
            InstantiateEffect(_jumpDust, Vector3.zero, 2f);
        }
    }

    private void OnDashed(float duration)
    {
        InstantiateEffect(_dashCircles, transform.forward * 3, duration);
    }

    private void OnJumpEnded(BaseJump _)
    {
        StartCoroutine(TurnOffTrail(_simpleJumpTrail, _simpleJumpTrail.time));
        StartCoroutine(TurnOffTrail(_poweredSimpleJumpTrail, _poweredSimpleJumpTrail.time));
    }

    private void InstantiateEffect(ParticleSystem particleSystem, Vector3 offset, float destroyTime)
    {
        var effect = Instantiate(particleSystem, transform.position + offset, Quaternion.identity); //Нужно через PlayerTransformController
        effect.transform.rotation = transform.rotation;
        Destroy(effect.gameObject, destroyTime);
    }

    private IEnumerator TurnOffTrail(TrailRenderer trailRenderer, float delay)
    {
        yield return new WaitForSeconds(delay);
        trailRenderer.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SimpleJump.SimpleJumpStarted -= OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted -= OnLongJumpStarted;
        DashSuperJump.DashJumpDashed -= OnDashed;
        HighSuperJump.HighJumpStarted -= OnHighJumpStarted; 
        BaseJump.JumpEnded -= OnJumpEnded;
    }
}
