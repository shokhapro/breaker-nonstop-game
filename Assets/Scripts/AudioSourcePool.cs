using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourcePool : MonoBehaviour
{
    private AudioSource _as;
    private List<AudioSource> _asp = new List<AudioSource>();

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    private AudioSource GetFreeAudioSource()
    {
        if (!_as.isPlaying)
            return _as;

        foreach (AudioSource s in _asp)
            if (!s.isPlaying)
                return s;

        var new_as = new GameObject().AddComponent<AudioSource>();
        new_as.transform.parent = transform;
        new_as.loop = _as.loop;
        new_as.playOnAwake = false;
        new_as.volume = _as.volume;
        new_as.clip = _as.clip;
        _asp.Add(new_as);
        return new_as;
    }

    public void Play()
    {
        GetFreeAudioSource().Play();
    }

    public void Play(AudioClip custom)
    {
        var fas = GetFreeAudioSource();
        fas.clip = custom;
        fas.Play();
    }
}
