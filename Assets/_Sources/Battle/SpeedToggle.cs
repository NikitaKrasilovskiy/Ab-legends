using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SpeedToggle : MonoBehaviour
{
    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnSpeedChange);
        _toggle.isOn = PlayerPrefs.GetInt("BattleSpeed", 0) > 0;
    }

    void OnSpeedChange(bool fast)
    {
        BattleDataContainer.timeScale = fast ? 5 : 3;
        PlayerPrefs.SetInt("BattleSpeed", fast ? 1 : 0);
    }
}
