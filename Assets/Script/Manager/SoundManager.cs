using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSoucrces = new AudioSource[System.Enum.GetValues(typeof(Sound)).Length];
    Dictionary<string, AudioClip> _clipDics = new Dictionary<string, AudioClip>();

    public void Initialize()
    {
        string objName = "@Sound";
        GameObject go = GameObject.Find(objName);
        if (go == null)
        {
            go = new GameObject(objName);
            Object.DontDestroyOnLoad(go);

            string[] soundNames = System.Enum.GetNames(typeof(Sound));
            for (int i = 0; i < soundNames.Length; i++) 
            {
                GameObject soundGo = new GameObject(soundNames[i]);
                _audioSoucrces[i] = soundGo.AddComponent<AudioSource>();
                _audioSoucrces[i].playOnAwake = false;
                soundGo.transform.parent = go.transform;
            }

            _audioSoucrces[(int)Sound.Bgm].loop = true;
        } 
    }

    public void Play(string clipName, Sound sound = Sound.UI)
    {
        AudioClip clip = GetAudioClip(clipName);
        if (clip == null)
            return;

        AudioSource audioSource = _audioSoucrces[(int)sound];

        switch (sound)
        {
            case Sound.Bgm:
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.clip = clip;
                audioSource.Play();
                break;
            case Sound.InGmae:
                audioSource.PlayOneShot(clip);
                break;
            case Sound.UI:
                audioSource.PlayOneShot(clip);
                break;
        }
    }
    AudioClip GetAudioClip(string clipName)
    {
        clipName = $"AudioClip/{clipName}";

        if (_clipDics.TryGetValue(clipName, out AudioClip dicsClip))
        {
            return dicsClip;
        }
        else
        {
            AudioClip resourcesClip = Managers.Resources.Load<AudioClip>(clipName);
            _clipDics.Add(clipName, resourcesClip);

            if (resourcesClip == null)
            {
                UnityHelper.LogError_H($"SoundManager GetAudioClip Clip Null Error\nclipName : {clipName}");
            }

            return resourcesClip;
        }
    }
    public void Clear()
    {
        foreach (var source in _audioSoucrces)
        {
            source.clip = null;
            source.Stop();
        }

        _clipDics.Clear();
    }
}
