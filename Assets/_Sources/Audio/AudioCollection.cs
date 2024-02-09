using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

[CreateAssetMenu(fileName = "AudioCollection", menuName = "ArgyBargy/AudioCollection", order = 1)]
public class AudioCollection : ScriptableObjectInstaller<AudioCollection>
{
    [SerializeField] public List<NamedSound> sounds;

    public override void InstallBindings()
    {
        Container.BindInstance(sounds).IfNotBound();
    }
}

[Serializable]
public class NamedSound
{
    public string name;
    public AudioClip audioClip;
    public AudioMixerGroup audioMixerGroup;
}
