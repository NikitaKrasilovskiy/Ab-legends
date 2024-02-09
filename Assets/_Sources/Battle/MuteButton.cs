using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private AudioMixer audioMixer;
    private const string MASTER_VOLUME = "MasterVolume";
    private void Awake()
    {
        
        
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void Start()
    {
        float f = 0;
        audioMixer.GetFloat(MASTER_VOLUME, out f);
        Debug.Log(f);
        toggle.isOn = Math.Abs(Math.Abs(f) + (-80f)) == 0;
    }

    private void OnValueChanged(bool arg0)
    {
        audioMixer.SetFloat(MASTER_VOLUME, arg0?-80:0);
    }
}
