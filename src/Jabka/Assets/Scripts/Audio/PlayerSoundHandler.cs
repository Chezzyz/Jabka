using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    [Header("Low Force Jump")]
    [SerializeField]
    private AudioClip _lowForceJumpSound;
    [Range(0f, 1f)]
    [SerializeField]
    private float _lowForceJumpVolumeScale;

    [Header("Medium Force Jump")]
    [SerializeField]
    private AudioClip _mediumForceJumpSound;
    [Range(0f, 1f)]
    [SerializeField]
    private float _mediumForceJumpVolumeScale;

    [Header("High Force Jump")]
    [SerializeField]
    private AudioClip _highForceJumpSound;
    [Range(0f, 1f)]
    [SerializeField]
    private float _highForceJumpVolumeScale;

    [Header("Long SuperJump")]
    [SerializeField]
    private AudioClip _longJumpSound;
    [Range(0f, 1f)]
    [SerializeField]
    private float _longJumpVolumeScale;

    [Header("Dash Jump Started")]
    [SerializeField]
    private AudioClip _dashJumpStartedSound;
    [Range(0f, 1f)]
    [SerializeField]
    private float _dashJumpStartedVolumeScale;

    [Header("Dash Jump Dashed")]
    [SerializeField]
    private AudioClip _dashJumpDashedSound;
    [Range(0f, 1f)]
    [SerializeField]
    private float _dashJumpDashedVolumeScale;
    [SerializeField]
    private AudioClip _dashJumpDashedSound2;
    [Range(0f, 1f)]
    [SerializeField]
    private float _dashJumpDashedVolumeScale2;

    [Header("Random Pitch")]
    [SerializeField]
    private float _minPitch;
    [SerializeField]
    private float _maxPitch;

    private static PlayerSoundHandler _singleton;
    private AudioSource _playerAudioSource;

    private void Awake()
    {
        if(_singleton == null)
        {
            _singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneLoader.SceneLoaded += OnSceneLoaded;
        SimpleJump.SimpleJumpStarted += OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted += OnLongJumpStarted;
        DashSuperJump.DashJumpStarted += OnDashJumpStarted;
        DashSuperJump.DashJumpDashed += OnDashJumpDashed;
    }

    private void OnSceneLoaded(string name)
    {
        _playerAudioSource = FindObjectOfType<PlayerAudioSource>()?.GetComponent<AudioSource>();
        if(_playerAudioSource == null && name != "MenuScene")
        {
            Debug.LogWarning("PlayerAudioSource not found on scene");
        }
    }

    private void OnSimpleJumpStarted(float forcePercent, float duration)
    {
        if(forcePercent <= 0.25f) PlayClipRandomPitch(_lowForceJumpSound, _lowForceJumpVolumeScale);
        else if(forcePercent > 0.25f && forcePercent <= 0.75f) PlayClipRandomPitch(_mediumForceJumpSound, _mediumForceJumpVolumeScale);
        else if(forcePercent > 0.75f) PlayClipRandomPitch(_highForceJumpSound, _highForceJumpVolumeScale); 
        else throw new System.NotImplementedException(); 
    }

    private void OnLongJumpStarted(float duration)
    {
        PlayClip(_longJumpSound, volumeScale: _longJumpVolumeScale);
    }

    private void OnDashJumpStarted()
    {
        PlayClip(_dashJumpStartedSound, volumeScale: _dashJumpStartedVolumeScale);
    }

    private void OnDashJumpDashed(float duration)
    {
        PlayClip(_dashJumpDashedSound, 0.5f, _dashJumpDashedVolumeScale);
        PlayClip(_dashJumpDashedSound2, 1f, _dashJumpDashedVolumeScale2);
    }

    private void PlayClip(AudioClip clip, float pitch = 1f, float volumeScale = 1f)
    {
        _playerAudioSource.pitch = pitch;
        _playerAudioSource.PlayOneShot(clip, volumeScale);
    }

    private void PlayClipRandomPitch(AudioClip clip, float volumeScale = 1f)
    {
        float pitch = Random.Range(_minPitch, _maxPitch);
        PlayClip(clip, pitch, volumeScale);
    }

    private void OnDisable()
    {
        SceneLoader.SceneLoaded -= OnSceneLoaded;
        SimpleJump.SimpleJumpStarted -= OnSimpleJumpStarted;
        LongSuperJump.LongJumpStarted -= OnLongJumpStarted;
        DashSuperJump.DashJumpStarted -= OnDashJumpStarted;
        DashSuperJump.DashJumpDashed -= OnDashJumpDashed;
    }

}
