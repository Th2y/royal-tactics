using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ChooseLanguage : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private const string PLAYER_PREF_KEY = "selected_language";

    private bool _isInitialized = false;

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        _isInitialized = true;

        dropdown.ClearOptions();

        List<string> options = new();

        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            options.Add(locale.Identifier.CultureInfo.NativeName);
        }

        dropdown.AddOptions(options);
        dropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        dropdown.onValueChanged.AddListener(SetLanguage);

        LoadSavedLanguage();
    }

    public void SetLanguage(int localeIndex)
    {
        if (!_isInitialized) return;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];

        PlayerPrefs.SetInt(PLAYER_PREF_KEY, localeIndex);
        PlayerPrefs.Save();
    }

    private void LoadSavedLanguage()
    {
        if (!PlayerPrefs.HasKey(PLAYER_PREF_KEY))
        {
            SetLanguage(0);

            return;
        }

        int index = PlayerPrefs.GetInt(PLAYER_PREF_KEY);

        if (index < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}