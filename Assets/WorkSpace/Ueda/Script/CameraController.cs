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

    private void Awake()
    {
        _defaultFov = _camera.m_Lens.FieldOfView;
        this.UpdateAsObservable()
            .DistinctUntilChanged()
            .Select(_ => GameMock.Instance.Mode)
            .Subscribe(ChangeFov);
    }

    private void ChangeFov(GameMode gameMode)
    {
        if (gameMode == GameMode.Real)
        {
            _camera.m_Lens.FieldOfView = _defaultFov;
        }
        else if (gameMode == GameMode.Dream)
        {
            _camera.m_Lens.FieldOfView = 80f;
        }
    }
}
