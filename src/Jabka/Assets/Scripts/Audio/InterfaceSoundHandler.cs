using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InterfaceSoundHandler : MonoBehaviour
{
    [Header("Collectable Stage 1")]
    [SerializeField]
    private AudioClip _buttonSound;
    [SerializeField]
    [Range(0f, 1f)]
    private float _buttonSoundVolumeScale;

    [Header("Quest Appear")]
    [SerializeField]
    private AudioClip _questAppearSound;
    [SerializeField]
    [Range(0f, 1f)]
    private float _questAppearSoundVolumeScale;


    [Header("Quest Completed")]
    [SerializeField]
    private AudioClip _questCompletedSound;
    [SerializeField]
    [Range(0f, 1f)]
    private float _questCompletedSoundVolumeScale;


    private AudioSource _audioSource;
    private static InterfaceSoundHandler _singleton;

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
        SceneLoader.SceneLoaded += OnSceneLoaded;
        OnCompleteQuestEffect.ReadyForCompleteViewSetted += OnReadyForComplete;
        LevelPreparation.QuestAppearStarted += OnQuestAppearStarted;
    }

    private void OnSceneLoaded(string name)
    {
        _audioSource = FindObjectOfType<CameraAudioSource>()?.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("CameraAudioSource not found on scene");
        }

        //Некоторые кнопки могут не сразу загрузиться после загрузки сцены, поэтому ждем немного
        StartCoroutine(AddButtonsListenerAfterDelay(0.5f));
    }

    private void OnQuestAppearStarted()
    {
        PlayClip(_questAppearSound, volumeScale: _questAppearSoundVolumeScale);
    }

    private void OnReadyForComplete()
    {
        PlayClip(_questCompletedSound, volumeScale: _questCompletedSoundVolumeScale);
    }

    private IEnumerator AddButtonsListenerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        List<Button> buttons = FindObjectsOfType<Button>(true).ToList();
        buttons.ForEach(button => button.onClick.AddListener(() => PlayClip(_buttonSound, volumeScale: _buttonSoundVolumeScale)));
    }

    private void PlayClip(AudioClip clip, float pitch = 1f, float volumeScale = 1f)
    {
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(clip, volumeScale);
    }

}
