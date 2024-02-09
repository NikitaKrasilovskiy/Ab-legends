using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectEmitor : MonoBehaviour
{
    [SerializeField] private string id;

    public void Play()
    {
        SoundEngine.PlayEffect(id);
    }
    
    public void Play(string i)
    {
        SoundEngine.PlayEffect(i);
    }
}
