using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer _audioMixerGroup;
    [SerializeField] private Image volumeIcon;
    [SerializeField] private Sprite volumeOn;
    [SerializeField] private Sprite volumeOff;
    private bool _soundIsPlay = false;
    private const string MASTER_VOLUME = "MasterVolume";
    private void OnEnable()
    {
        float volume = 0;
         _audioMixerGroup.GetFloat(MASTER_VOLUME, out volume);
         slider.value = volume;
    }

    public void SetVolume(float i)
    {
        _audioMixerGroup.SetFloat(MASTER_VOLUME, i);
        if (i > slider.minValue)
            volumeIcon.sprite = volumeOn;
        else
            volumeIcon.sprite = volumeOff;
        PlaySound();
    }

    async void PlaySound()
    {
        if(_soundIsPlay)
            return;
        _soundIsPlay = true;
        await SoundEngine.PlayEffect("click");
        _soundIsPlay = false;
    }
}
