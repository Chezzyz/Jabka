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
    private List<Button> _ruButtons;
    [SerializeField]
    private List<Button> _enButtons;

    private bool _fromSave;
    
    void Start()
    {
        if (_fromSave)
        {
            _languageCanvas?.gameObject.SetActive(false);
        }
        _ruButtons.ForEach(button => button.onClick.AddListener(OnRuButtonClicked));
        _enButtons.ForEach(button => button.onClick.AddListener(OnEnButtonClicked));
    }

    private void OnRuButtonClicked()
    {
        SetLocale(_ruLocale);
        _languageCanvas?.gameObject.SetActive(false);
    }

    private void OnEnButtonClicked()
    {
        SetLocale(_enLocale);
        _languageCanvas?.gameObject.SetActive(false);
    }

    private void SetLocale(Locale locale)
    {
        LocalizationSettings.SelectedLocale = locale;
    }

    public void SetLocaleFromSave(string locale)
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
        return LocalizationSettings.SelectedLocale.name;
    }
}
