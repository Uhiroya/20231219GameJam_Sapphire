using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessSetting : MonoBehaviour
{
    [SerializeField] PostProcessVolume _postProcessVolume;
    [SerializeField] private float _blendTime;
    [SerializeField,Range(0f,1f)] private float _realVignetteIntensity;
    [SerializeField,Range(0f,1f)] private float _realFogDensity;
    [SerializeField] private Color _realFogColor;
    [SerializeField,Range(0f,1f)] private float _dreamVignetteIntensity;
    [SerializeField,Range(0f,1f)] private float _dreamFogDensity;
    [SerializeField] private Color _dreamFogColor;
    Vignette _vignette;
    private float _currentVignetteIntensity;
    private float _currentFogDensity;
    private Color _currentFogColor;
    private void Start()
    {
        _postProcessVolume.profile.TryGetSettings(out _vignette);
        _vignette.intensity.value = _currentVignetteIntensity = _realVignetteIntensity;
        RenderSettings.fogDensity = _currentFogDensity = _realFogDensity;
        _currentFogColor = _realFogColor;
        Bind();
    }

    private void Bind()
    {
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => OnStartDream()).AddTo(this);
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => OnStartReal()).AddTo(this);
    }
    
    private void OnStartDream()
    {
        //_state = InGameState.Dream;   
        DOVirtual.Float(
            _currentFogDensity,
            _dreamFogDensity,
            _blendTime,
            value =>_vignette.intensity.value = value
        ).OnComplete(() => _currentFogDensity = _dreamFogDensity);
        
        DOVirtual.Float(
            _currentVignetteIntensity,
            _dreamVignetteIntensity,
            _blendTime,
            value => RenderSettings.fogDensity = value
        ).OnComplete(() => _currentVignetteIntensity = _dreamVignetteIntensity);

        DOVirtual.Color(
            _currentFogColor,
            _dreamFogColor,
            _blendTime,
            value => RenderSettings.fogColor = value
        ).OnComplete(() => _currentFogColor = _dreamFogColor);
    }

    private void OnStartReal()
    {
        //_state = InGameState.Dream;   
        DOVirtual.Float(
            _currentFogDensity,
            _realFogDensity,
            _blendTime,
            value =>_vignette.intensity.value = value
        ).OnComplete(() => _currentFogDensity = _realFogDensity);;
        
        DOVirtual.Float(
            _currentVignetteIntensity,
            _realVignetteIntensity,
            _blendTime,
            value => RenderSettings.fogDensity = value
        ).OnComplete(() => _currentVignetteIntensity = _realVignetteIntensity);
        
        DOVirtual.Color(
            _currentFogColor,
            _realFogColor,
            _blendTime,
            value => RenderSettings.fogColor = value
        ).OnComplete(() => _currentFogColor = _realFogColor);
    }
    
}
