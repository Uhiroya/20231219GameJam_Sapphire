using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    public enum SEType
    {
        BackToReality = 0,
        ClaseTheDoor,
        DoornobuDestruction,
        GameStart,
        GetItems,
        GoToDream,
        GomibakoDisappear,
        Gun,
        Run,
        Walk,
    }

    [SerializeField]
    AudioSource _audioSource;
    [SerializeField]
    AudioClip[] _SEClips;
    [SerializeField]
    AudioClip _BGMClip;

    public void PlaySE(SEType soundIndex)
        => _audioSource.PlayOneShot(_SEClips[(int)soundIndex]);

    public void PlayBGM()
    {
        _audioSource.clip = _BGMClip;
        _audioSource.Play();
    }
}
