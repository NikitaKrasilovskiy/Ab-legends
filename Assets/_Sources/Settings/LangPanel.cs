using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.UI;

public class LangPanel : MonoBehaviour
{
    [SerializeField] private Toggle rusToggle;
    [SerializeField] private Toggle engToggle;
    private Languages _currentLang;
    public const string LANGUAGE = "Language";
    public void Awake()
    {
        _currentLang = (Languages) PlayerPrefs.GetInt(LANGUAGE, 0);
    }

    private void Start()
    {
        rusToggle.onValueChanged.AddListener((b)=>ChangeLang(b, Languages.Russian));
        engToggle.onValueChanged.AddListener((b)=>ChangeLang(b, Languages.English));
        switch (_currentLang)
        {
            case Languages.Russian:
            {
                rusToggle.isOn = true;
                break;
            }
            case Languages.English:
            {
                engToggle.isOn = true;
                break;
            }
        }
    }

    void ChangeLang(bool isOn, Languages currentLang)
    {
        if(!isOn)
            return;
        switch (currentLang)
        {
            case Languages.English:
            {
                LocalizationManager.Language = "English";
                break;
            }
            case Languages.Russian:
            {
                LocalizationManager.Language = "Russian";
                break;
            }
        }
        PlayerPrefs.SetInt(LANGUAGE, (int)currentLang);
    }
}

public enum Languages
{
    Russian,
    English
}
