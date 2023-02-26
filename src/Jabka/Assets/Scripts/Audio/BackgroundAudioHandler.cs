using DG.Tweening;
using UnityEngine;

namespace Audio
{
    public class BackgroundAudioHandler : BaseAudioHandler<BackgroundAudioSource>
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

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            DashSuperJump.DashPreparingStarted += OnDashPreparingStarted;
            DashSuperJump.DashPreparingEnded += OnDashPreparingEnded;
            CompletePlace.LevelCompleted += OnLevelCompleted;
            SettingsHandler.MusicStateChanged += OnMusicStateChanged;
        }

        protected override void OnSceneLoaded(string name)
        {
            base.OnSceneLoaded(name);

            int stage = SceneStatus.GetCurrentStageNumber();

       

            System.Action play = stage switch
            {
                0 => () => PlayClip(_menuMusic, volumeScale: _menuMusicVolumeScale),
                1 => () => PlayClip(_backgroundMusicStage1, volumeScale: _backgroundMusicVolumeScaleStage1),
                2 => () => PlayClip(_backgroundMusicStage2, volumeScale: _backgroundMusicVolumeScaleStage2),
                3 => () => PlayClip(_backgroundMusicStage3, volumeScale: _backgroundMusicVolumeScaleStage3),
                _ => () => Debug.LogWarning("Music for this stage is not implemented")
            };
            play();
        }

        protected override void OnVolumeChanged(float coef)
        {
            if (_audioSource != null && SettingsHandler.MusicIsOn())
            {
                _audioSource.volume = _volumeScale * coef;
            }
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

        private void OnMusicStateChanged()
        {
            _audioSource.volume = SettingsHandler.MusicIsOn() ? _volumeScale * SettingsHandler.GetVolumeCoef() : 0;
        }

    }
}
