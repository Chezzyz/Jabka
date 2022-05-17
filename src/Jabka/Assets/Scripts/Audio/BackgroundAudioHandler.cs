using DG.Tweening;
using UnityEngine;

public class BackgroundAudioHandler : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField]
    private AudioClip _menuMusic;
    [SerializeField]
    [Range(0f,1f)]
    private float _menuMusicVolumeScale;

    [Header("Stage 1")]
    [SerializeField]
    private AudioClip _backgroundMusicStage1;
    [SerializeField]
    private AudioClip _backgroundMusicStage1Level5;
    [SerializeField]
    [Range(0f, 1f)]
    private float _backgroundMusicVolumeScaleStage1;

    [Header("Stage 2")]
    [SerializeField]
    private AudioClip _backgroundMusicStage2;
    [SerializeField]
    [Range(0f, 1f)]
    private float _backgroundMusicVolumeScaleStage2;

    [Header("Stage 3")]
    [SerializeField]
    private AudioClip _backgroundMusicStage3;
    [SerializeField]
    [Range(0f, 1f)]
    private float _backgroundMusicVolumeScaleStage3;

    [Header("Level Completed")]
    [SerializeField]
    private AudioClip _levelCompletedSound;
    [SerializeField]
    [Range(0f, 1f)]
    private float _levelCompletedSoundVolumeScale;

    [Header("Dash Jump Pitch")]
    [SerializeField]
    private float _dashJumpPitch;
    [SerializeField]
    private float _dashJumpPitchDuration;


    private AudioSource _audioSource;
    private static BackgroundAudioHandler _singleton;

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
        DashSuperJump.DashPreparingStarted += OnDashPreparingStarted;
        DashSuperJump.DashPreparingEnded += OnDashPreparingEnded;
        CompletePlace.LevelCompleted += OnLevelCompleted;
        SceneLoader.SceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(string name)
    {
        _audioSource = FindObjectOfType<CameraAudioSource>()?.GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogWarning("CameraAudioSource not found on scene");
        }

        int stage = name == "MenuScene" ? 0 : int.Parse(name.Split('_')[1].Split('-')[0]);
        int level = name == "MenuScene" ? 0 : int.Parse(name.Split('_')[1].Split('-')[1]);
        if (name == "MenuScene") PlayClip(_menuMusic, volumeScale: _menuMusicVolumeScale);
        else if (stage == 1)
        {
            if(level == 5) PlayClip(_backgroundMusicStage1Level5, volumeScale: _backgroundMusicVolumeScaleStage1);
            else PlayClip(_backgroundMusicStage1, volumeScale: _backgroundMusicVolumeScaleStage1);
        }
        else if (stage == 2) PlayClip(_backgroundMusicStage2, volumeScale: _backgroundMusicVolumeScaleStage2);
        else if (stage == 3) PlayClip(_backgroundMusicStage3, volumeScale: _backgroundMusicVolumeScaleStage3);
        else throw new System.NotImplementedException();
    }

    private void OnDashPreparingStarted(float slowmoDuration)
    {
        _audioSource.DOPitch(_dashJumpPitch, _dashJumpPitchDuration);
    }

    private void OnDashPreparingEnded()
    {
        _audioSource.DOPitch(1, 0.5f);
    }

    private void OnLevelCompleted(CompletePlace _)
    {
        _audioSource.PlayOneShot(_levelCompletedSound, _levelCompletedSoundVolumeScale);
    }

    private void PlayClip(AudioClip clip, float pitch = 1f, float volumeScale = 1f)
    {
        _audioSource.pitch = pitch;
        _audioSource.volume = volumeScale;
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void OnDisable()
    {
        DashSuperJump.DashPreparingStarted -= OnDashPreparingStarted;
        DashSuperJump.DashPreparingEnded -= OnDashPreparingEnded;
        SceneLoader.SceneLoaded -= OnSceneLoaded;
        CompletePlace.LevelCompleted -= OnLevelCompleted;
    }
}
