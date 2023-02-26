 using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class SettingsHandler : BaseGameHandler<SettingsHandler>
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
        SaveLoadSystem.SaveLoaded += OnSaveLoaded;
    }

    private void OnSaveLoaded()
    {
        SetValues();
    }

    private void OnSceneLoaded(string name)
    {
        _volumeSprites = new List<Sprite>() { _volumeSpriteOff, _volumeSpriteLevel1, _volumeSpriteLevel2, _volumeSpriteLevel3 };

        _musicButton = FindObjectOfType<MusicButton>().GetComponent<Button>();
        _volumeButton = FindObjectOfType<VolumeButton>().GetComponent<Button>();

        _musicButton.onClick.AddListener(() => ChangeMusicState());
        _volumeButton.onClick.AddListener(() => IncrementVolumeLevel());

        _verticalSensSlider = FindObjectOfType<VerticalSensSlider>()?.GetComponent<Slider>();
        _horizontalSensSlider = FindObjectOfType<HorizontalSensSlider>()?.GetComponent<Slider>();

        if(_verticalSensSlider != null && _horizontalSensSlider != null)
        {
            _verticalSensSlider.onValueChanged.AddListener(value => SetVerticalSensetivity(_verticalSensSlider.normalizedValue));
            _horizontalSensSlider.onValueChanged.AddListener(value => SetHorizontalSensetivity(_horizontalSensSlider.normalizedValue));
        }

        SetValues();
    }

    private void SetValues()
    {
        _musicButton.GetComponent<Image>().sprite = _musicIsOn ? _musicSpriteOn : _musicSpriteOff;
        _volumeButton.GetComponent<Image>().sprite = _volumeSprites[_volumeLevel];

        if (_verticalSensSlider != null && _horizontalSensSlider != null)
        {
            _verticalSensSlider.value = Mathf.Lerp(_verticalSensSlider.minValue, _verticalSensSlider.maxValue, _verticalSensetivityCoef);
            _horizontalSensSlider.value = Mathf.Lerp(_horizontalSensSlider.minValue, _horizontalSensSlider.maxValue, _horizontalSensetivityCoef);
        }
    }

    #region VolumeLevel
    public static int GetVolumeLevel()
    {
        return Instance._volumeLevel;
    }

    public void SetVolumeLevel(int value)
    {
        _volumeLevel = value;
        _volumeButton.GetComponent<Image>().sprite = _volumeSprites[value];
        VolumeLevelChanged?.Invoke(GetVolumeCoef());
    }

    public static float GetVolumeCoef()
    {
        return ((float)Instance._volumeLevel) / 3;
    }
    #endregion

    #region MusicIsOn
    public static bool MusicIsOn()
    {
        return Instance._musicIsOn;
    }

    public void SetMusicIsOn(bool value)
    {
        _musicIsOn = value;
        _musicButton.GetComponent<Image>().sprite = _musicIsOn ? _musicSpriteOn : _musicSpriteOff;
        MusicStateChanged?.Invoke();
    }
    #endregion

    #region VerticalSens

    public static float GetVerticalSensCoef()
    {
        return Instance._verticalSensetivityCoef;
    }

    public void SetVerticalSensetivity(float value)
    {
        _verticalSensetivityCoef = value;
    }

    public static (float min, float max) GetVerticalSensetivityBounds()
    {
        float start = Instance._verticalSensStart;
        float endMin = Instance._verticalSensEndMin;
        float endMax = Instance._verticalSensEndMax;
        float coef = Instance._verticalSensetivityCoef;

        return (start, Mathf.Lerp(endMax, endMin, coef));
    }
    #endregion

    #region HorizontalSens
    public static float GetHorizontalSensCoef()
    {
        return Instance._horizontalSensetivityCoef;
    }

    public void SetHorizontalSensetivity(float value)
    {
        _horizontalSensetivityCoef = value;
    }

    public static float GetHorizontalSensetivity()
    {
        float coef = Instance._horizontalSensetivityCoef;
        float min = Instance._horizontalSensMin;
        float max = Instance._horizontalSensMax;

        return Mathf.Lerp(min, max, coef);
    }
    #endregion

    private void ChangeMusicState()
    {
        SetMusicIsOn(!_musicIsOn);
    }

    private void IncrementVolumeLevel()
    {
        SetVolumeLevel((_volumeLevel + 1) % 4);
    }
    
}
