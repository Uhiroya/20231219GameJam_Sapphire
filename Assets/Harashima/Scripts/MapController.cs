using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private Transform _mapParent;
    [SerializeField] private Image _bulletImage;
    [SerializeField] private Image _enemyImage;

    [SerializeField] private Transform _enemy;
    [SerializeField] private Transform _player;

    private void Start()
    {
        var dir = _enemy.position * 3.5f;
        var ui = Instantiate(_bulletImage, _mapParent);
        ui.rectTransform.localPosition = new Vector3(dir.z,dir.x, 0);
    }
}
