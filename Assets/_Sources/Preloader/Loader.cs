using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;

public class Loader : MonoBehaviour
{
    void Awake()
    {
        var _currentLang = (Languages) PlayerPrefs.GetInt(LangPanel.LANGUAGE, 0);
        LocalizationManager.Language = _currentLang.ToString();
    }
}
