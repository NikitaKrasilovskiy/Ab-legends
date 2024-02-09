using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public static class SoundEngine
{
    [Inject] private static List<NamedSound> _sounds;
    private static bool isInit = false;

    public static void Init()
    {
        _sounds = Resources.Load<AudioCollection>("AudioCollection").sounds;
        isInit = true;
    }

    public static async UniTask<bool> PlayEffect(string id)
    {
         return await PlayEffect(id, Vector3.zero);
    }

    private static async UniTask<bool> PlayEffect(string id, Vector3 position)
    {
        if(!isInit)
            Init();
        var soundEmitor = new GameObject("Audio_" + id).AddComponent<AudioSource>();
        soundEmitor.transform.position = position;
        soundEmitor.playOnAwake = false;
        soundEmitor.loop = false;
        var namedSound = _sounds.Find(sound => sound.name == id);
        if (namedSound == null)
        {
            Debug.LogError("No sound with that name: "+id);
            return false;
        }
        var audioClip = namedSound.audioClip;
        if (!audioClip)
        {
            Debug.LogError("AudioClip is null: "+id);
            return false;
        }
        soundEmitor.clip = audioClip;
        soundEmitor.outputAudioMixerGroup = namedSound.audioMixerGroup;
        soundEmitor.Play();
        await UniTask.WaitUntil(() => soundEmitor.isPlaying);
        if(soundEmitor)
            await UniTask.WaitUntil(() => !soundEmitor.isPlaying);
        GameObject.Destroy(soundEmitor.gameObject);
        return true;
    }
}
