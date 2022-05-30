using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAudioHandler<T> : GameHandler<BaseAudioHandler<T>> where T : MonoBehaviour
{
    protected AudioSource _audioSource;

    protected float _volumeScale;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void OnEnable()
    {
        SceneLoader.SceneLoaded += OnSceneLoaded;
        SettingsHandler.VolumeLevelChanged += OnVolumeChanged;
    }

    protected virtual void OnSceneLoaded(string name)
    {
        _audioSource = FindObjectOfType<T>()?.GetComponent<AudioSource>();
        if (_audioSource == null && name != "MenuScene")
        {
            Debug.LogWarning(typeof(T) + " not found on scene");
        }
    }

    protected virtual void OnVolumeChanged(float coef)
    {
        if (_audioSource != null && !(this is BackgroundAudioHandler && !SettingsHandler.MusicIsOn()))
        {
            _audioSource.volume = coef;
        }
    }

    protected virtual void PlayClip(AudioClip clip, float pitch = 1f, float volumeScale = 1f)
    {
        _audioSource.pitch = pitch;
        _audioSource.volume = SettingsHandler.MusicIsOn() ? volumeScale * SettingsHandler.GetVolumeCoef() : 0;
        _audioSource.clip = clip;
        _audioSource.Play();

        _volumeScale = volumeScale;
    }

    protected virtual void PlayOneShot(AudioClip clip, float pitch = 1f, float volumeScale = 1f)
    {
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(clip, volumeScale * SettingsHandler.GetVolumeCoef());
    }

}
