using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class SettingsHandler : GameHandler<SettingsHandler>
{
    [Header("Volume")]
    [SerializeField]
    private Sprite _volumeSpriteLevel1;
    [SerializeField]
    private Sprite _volumeSpriteLevel2;
    [SerializeField]
    private Sprite _volumeSpriteLevel3;
    [SerializeField]
    private Sprite _volumeSpriteOff;
    private int _volumeLevel = 3;
    private List<Sprite> _volumeSprites;
    private Button _volumeButton;

    [Header("Music")]
    [SerializeField]
    private Sprite _musicSpriteOff;
    [SerializeField]
    private Sprite _musicSpriteOn;
    private Button _musicButton;
    private bool _musicIsOn = true;

    [Header("Sensetivity")]
    [SerializeField]
    private Slider _verticalSensSlider;
    [SerializeField]
    private float _verticalSensStart;
    [SerializeField]
    private float _verticalSensEndMin;
    [SerializeField]
    private float _verticalSensEndMax;
    
    [SerializeField]
    private Slider _horizontalSensSlider;
    [SerializeField]
    private float _horizontalSensMin;
    [SerializeField]
    private float _horizontalSensMax;
    
    private float _verticalSensetivityCoef = 0.5f;
    private float _horizontalSensetivityCoef = 0.5f;

    public static event System.Action MusicStateChanged;
    public static event System.Action<float> VolumeLevelChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        SceneLoader.SceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(string _)
    {
        _volumeSprites = new List<Sprite>() { _volumeSpriteOff, _volumeSpriteLevel1, _volumeSpriteLevel2, _volumeSpriteLevel3 };

        _musicButton = FindObjectOfType<MusicButton>().GetComponent<Button>();
        _volumeButton = FindObjectOfType<VolumeButton>().GetComponent<Button>();

        _musicButton.GetComponent<Image>().sprite = _musicIsOn ? _musicSpriteOn : _musicSpriteOff;
        _volumeButton.GetComponent<Image>().sprite = _volumeSprites[_volumeLevel];

        _musicButton.onClick.AddListener(() => ChangeMusicState());
        _volumeButton.onClick.AddListener(() => ChangeVolumeLevel());

        _verticalSensSlider = FindObjectOfType<VerticalSensSlider>()?.GetComponent<Slider>();
        _horizontalSensSlider = FindObjectOfType<HorizontalSensSlider>()?.GetComponent<Slider>();

        if(_verticalSensSlider != null && _horizontalSensSlider != null)
        {
            _verticalSensSlider.onValueChanged.AddListener(value => ChangeVerticalSensetivity(_verticalSensSlider.normalizedValue));
            _horizontalSensSlider.onValueChanged.AddListener(value => ChangeHorizontalSensetivity(_horizontalSensSlider.normalizedValue));

            _verticalSensSlider.value = Mathf.Lerp(_verticalSensSlider.minValue, _verticalSensSlider.maxValue, _verticalSensetivityCoef);
            _horizontalSensSlider.value = Mathf.Lerp(_horizontalSensSlider.minValue, _horizontalSensSlider.maxValue, _horizontalSensetivityCoef);
        }
    }

    public static float GetVolumeCoef()
    {
        return (float)((SettingsHandler)Instance)._volumeLevel / 3;
    }

    public static bool MusicIsOn()
    {
        return ((SettingsHandler)Instance)._musicIsOn;
    }

    public static (float min, float max) GetVerticalSensetivityBounds()
    {
        float start = ((SettingsHandler)Instance)._verticalSensStart;
        float endMin = ((SettingsHandler)Instance)._verticalSensEndMin;
        float endMax = ((SettingsHandler)Instance)._verticalSensEndMax;
        float coef = ((SettingsHandler)Instance)._verticalSensetivityCoef;

        return (start, Mathf.Lerp(endMax, endMin, coef));
    }

    public static float GetHorizontalSensetivity()
    {
        float coef = ((SettingsHandler)Instance)._horizontalSensetivityCoef;
        float min = ((SettingsHandler)Instance)._horizontalSensMin;
        float max = ((SettingsHandler)Instance)._horizontalSensMax;

        return Mathf.Lerp(min, max, coef);
    }

    private void ChangeMusicState()
    {
        _musicIsOn = !_musicIsOn;
        _musicButton.GetComponent<Image>().sprite = _musicIsOn ? _musicSpriteOn : _musicSpriteOff;
        MusicStateChanged?.Invoke();
    }

    private void ChangeVolumeLevel()
    {
        _volumeLevel = (_volumeLevel + 1) % 4;
        _volumeButton.GetComponent<Image>().sprite = _volumeSprites[_volumeLevel];
        VolumeLevelChanged?.Invoke(GetVolumeCoef());
    }

    private void ChangeVerticalSensetivity(float value)
    {
        _verticalSensetivityCoef = value;
    }

    private void ChangeHorizontalSensetivity(float value)
    {
        _horizontalSensetivityCoef = value;
    }
    
}
