using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSoundHandler : MonoBehaviour
{
    [Header("Collectable Stage 1")]
    [SerializeField]
    private AudioClip _collectableSoundStage1;
    [SerializeField]
    [Range(0f, 1f)]
    private float _collectableSoundStage1VolumeScale;
    [SerializeField]
    [Range(0f, 1f)]
    private float _collectableSoundStage1Pitch;

    private AudioSource _audioSource;
    private static ItemsSoundHandler _singleton;

    private void Awake()
    {
        if (_singleton == null)
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
        Collectable.Collected += OnCollected;
        Destination.Destinated += OnDestinated;
        SceneLoader.SceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(string name)
    {
        _audioSource = FindObjectOfType<PlayerAudioSource>()?.GetComponent<AudioSource>();
        if (_audioSource == null && name != "MenuScene")
        {
            Debug.LogWarning("PlayerAudioSource not found on scene");
        }
    }

    private void OnCollected(Collectable _)
    {
        PlayClip(_collectableSoundStage1, _collectableSoundStage1Pitch, _collectableSoundStage1VolumeScale);
    }

    private void OnDestinated(Destination _)
    {
        OnCollected(null);
    }

    private void PlayClip(AudioClip clip, float pitch = 1f, float volumeScale = 1f)
    {
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(clip, volumeScale);
    }
}
