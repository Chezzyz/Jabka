using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguagePicker : BaseGameHandler<LanguagePicker>
{
    [SerializeField]
    private Canvas _languageCanvas;

    [SerializeField]
    private Locale _ruLocale;
    [SerializeField]
    private Locale _enLocale;

    [SerializeField]
    private Button _ruButton;
    [SerializeField]
    private Button _enButton;

    private bool _fromSave;
    
    void Start()
    {
        if (_fromSave)
        {
            _languageCanvas.gameObject.SetActive(false);
        }
        _ruButton.onClick.AddListener(OnRuButtonClicked);
        _enButton.onClick.AddListener(OnEnButtonClicked);
    }

    private void OnRuButtonClicked()
    {
        SetLocale(_ruLocale);
        _languageCanvas.gameObject.SetActive(false);
    }

    private void OnEnButtonClicked()
    {
        SetLocale(_enLocale);
        _languageCanvas.gameObject.SetActive(false);
    }

    private void SetLocale(Locale locale)
    {
        LocalizationSettings.SelectedLocale = locale;
    }

    public void SetLocale(string locale)
    {
        if(locale == "Russian (ru)")
        {
            SetLocale(_ruLocale);
        }
        if(locale == "English (en)")
        {
            SetLocale(_enLocale);
        }
        _fromSave = true;
    }

    public string GetLocale()
    {
        return LocalizationSettings.SelectedLocale.LocaleName;
    }
}
