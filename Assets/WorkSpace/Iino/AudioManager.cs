using UnityEngine;

public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    public enum GameBGM
    {
        Main = 0
    }

    public enum GameSE
    {
        BackToReality = 0,
        CloseDoor = 1,
        DoorDestruction = 2,
        GameStart = 3,
        GetItems = 4,
        GoToDream = 5,
        TrashDisappear = 6,
        Shot = 7,
        Run = 8,
        Walk = 9,
        GameOver = 10,
    }

    [SerializeField] private AudioSource _audioBGMSource;

    [SerializeField] private AudioSource _audioSESource;

    [SerializeField] private AudioClip[] _SEClips;

    [SerializeField] private AudioClip[] _BGMClips;

    public void PlaySE(GameSE soundIndex)
    {
        _audioSESource.PlayOneShot(_SEClips[(int)soundIndex]);
    }
    public void PlaySE(int index)
    {
        _audioSESource.PlayOneShot(_SEClips[index]);
    }
    public void PlayBGM(GameBGM soundIndex)
    {
        _audioBGMSource.clip = _BGMClips[(int)soundIndex];
        _audioBGMSource.Play();
    }

    public void StopBGM()
    {
        _audioBGMSource.Stop();
    }
}
