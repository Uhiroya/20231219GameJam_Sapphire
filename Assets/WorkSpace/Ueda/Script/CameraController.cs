using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;
using UniRx.Triggers;
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;

    private float _defaultFov;

    private void Start()
    {
        _defaultFov = _camera.m_Lens.FieldOfView;
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => OnStartDream());
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => OnStartReal());
    }
    private void OnStartDream()
    {
        _camera.m_Lens.FieldOfView = 80f;
    }

    private void OnStartReal()
    {
        _camera.m_Lens.FieldOfView = _defaultFov;
    }
}
