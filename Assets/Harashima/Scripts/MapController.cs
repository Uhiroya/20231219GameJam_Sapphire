using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private Transform _mapParent;
    [SerializeField] private Image _bulletImagePrefab;
    [SerializeField] private Image _enemyImagePrefab;

    [SerializeField] private Transform _enemy;
    [SerializeField] private Transform _player;

    [SerializeField, Header("マップのサイズ")] private float _mapSize = 10f;

    private Image _enemyImage;
    private void Start()
    {
        _enemyImage = Instantiate(_enemyImagePrefab, _mapParent);
    }

    private void Update()
    {
        _mapParent.rotation = Quaternion.Euler(0, 0, _player.eulerAngles.y);
        var pos = (_enemy.position - _player.position) / _mapSize * 100f;
        _enemyImage.rectTransform.anchoredPosition = new Vector3(pos.x,pos.z, 0);

    }
}
