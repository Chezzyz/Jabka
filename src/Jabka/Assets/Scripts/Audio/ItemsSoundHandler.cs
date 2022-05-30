using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSoundHandler : BaseAudioHandler<PlayerAudioSource>
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

    public static new ItemsSoundHandler Instance;

    protected override void Awake()
    {
        //Из-за конфликта с PlayerSoundHandler скрываем от GameHandler<>.Awake()
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Collectable.Collected += OnCollected;
        Destination.Destinated += OnDestinated;
    }

    protected override void OnSceneLoaded(string name)
    {
        base.OnSceneLoaded(name);
    }

    private void OnCollected(Collectable _)
    {
        PlayOneShot(_collectableSoundStage1, _collectableSoundStage1Pitch, _collectableSoundStage1VolumeScale);
    }

    private void OnDestinated(Destination _)
    {
        OnCollected(null);
    }

}
