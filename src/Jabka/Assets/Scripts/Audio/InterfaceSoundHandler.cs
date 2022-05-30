using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InterfaceSoundHandler : BaseAudioHandler<InterfaceAudioSource>
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

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnCompleteQuestEffect.ReadyForCompleteViewSetted += OnReadyForComplete;
        LevelPreparation.QuestAppearStarted += OnQuestAppearStarted;
    }

    protected override void OnSceneLoaded(string name)
    {
        base.OnSceneLoaded(name);

        //Некоторые кнопки могут не сразу загрузиться после загрузки сцены, поэтому ждем немного
        StartCoroutine(AddButtonsListenerAfterDelay(0.5f));
    }

    private void OnQuestAppearStarted()
    {
        PlayOneShot(_questAppearSound, volumeScale: _questAppearSoundVolumeScale);
    }

    private void OnReadyForComplete()
    {
        PlayOneShot(_questCompletedSound, volumeScale: _questCompletedSoundVolumeScale);
    }

    private IEnumerator AddButtonsListenerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        List<Button> buttons = FindObjectsOfType<Button>(true).ToList();
        buttons.ForEach(button => button.onClick.AddListener(() => PlayOneShot(_buttonSound, volumeScale: _buttonSoundVolumeScale)));
    }

}
