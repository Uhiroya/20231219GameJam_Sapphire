using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessSetting : MonoBehaviour
{
    [SerializeField]
    PostProcessVolume _postProcessVolume;

    Vignette _vignette;

    private void Start()
    {
        _postProcessVolume.profile.TryGetSettings(out _vignette);
    }

    /// <summary>
    /// PostProcessVolumeのVignette.intensityと
    /// RenderSettingsのfogDensityを変更します。
    /// 必ず0～1の範囲の値を入れてください
    /// </summary>
    /// <param name="vignetteIntensityValue">PostProcessVolumeのVignette.intensity</param>
    /// <param name="fogDensityValue">RenderSettingsのfogDensity</param>
    public void SetPostEffects(float vignetteIntensityValue, float fogDensityValue)
    {
        _vignette.intensity.value = vignetteIntensityValue;
        RenderSettings.fogDensity = fogDensityValue;
    }
}
