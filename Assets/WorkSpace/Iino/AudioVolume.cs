using DG.Tweening;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    public void SetAudioVolumeAdjustment()
    {
        var audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.DOFade(
            0.0f,   // 音量
            5.0f    // 段々と小さくなっていく時間（秒)
        ).OnStart(() =>
        {
            audioSource.volume = 1.0f;
        });
    }

    public void Start()
    {
        SetAudioVolumeAdjustment();
    }
}
