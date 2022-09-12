using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public List<AudioClip> clips = new List<AudioClip>();
    public AudioSource source;

    public static void Play(AudioType type) => Instance.play(type);

    public void play(AudioType type)
    {
        source.PlayOneShot(clips[(int)type]);
    }
}

public enum AudioType
{
    Attack,
    Clear,
    Drop,
    Failure,
    Success,
    Walk,
}
