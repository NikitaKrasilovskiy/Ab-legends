using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStarter : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    
    public void PlayAudio()
    {
        audioSource.Play();
    }
}
