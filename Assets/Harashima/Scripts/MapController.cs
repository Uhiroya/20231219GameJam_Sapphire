using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MapController : SingletonMonoBehavior<MapController>
{
    [SerializeField] private Transform _mapParent;
    [SerializeField] private Image _bulletImagePrefab;
    [SerializeField] private Image _enemyImagePrefab;
    
    [SerializeField] private Transform _player;

    [SerializeField, Header("マップのサイズ")] private float _mapSize = 10f;

    private Dictionary<EnemyController, Image> _enemies = new Dictionary<EnemyController, Image>();
    private Dictionary<GameObject, Image> _items = new Dictionary<GameObject, Image>();

    private bool _isEnd = false;

    private void Start()
    {
        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {
            var ui = Instantiate(_bulletImagePrefab, _mapParent);
            ui.gameObject.SetActive(false);
            _items.Add(bullet, ui);
        }

        InGameManager.Instance.OnFinishGame.Subscribe(_ =>
        {
            _isEnd = true;
        });
    }

    public void CreateEnemyUI(EnemyController[] enemyControllers)
    {
        foreach (var enemy in enemyControllers)
        {
            var ui = Instantiate(_enemyImagePrefab, _mapParent);
            ui.gameObject.SetActive(false);
            _enemies.Add(enemy, ui);
        }
    }

    public void SetActiveItem(GameObject item,bool isActive)
    {
        if (_items.ContainsKey(item) == false)
        {
#if UNITY_EDITOR
            Debug.LogWarning("アイテムが登録されていません");
#endif
            return;
        }
        _items[item].gameObject.SetActive(isActive);
    }

    public void SetActiveEnemy(EnemyController enemyController, bool isActive)
    {
        if (_enemies.ContainsKey(enemyController) == false)
        {
#if UNITY_EDITOR
            Debug.LogWarning("アイテムが登録されていません");
#endif
            return;
        }
        _enemies[enemyController].gameObject.SetActive(isActive);
    }

    public void HideAllIcon()
    {
        foreach (var icon in _enemies.Values)
        {
            icon.gameObject.SetActive(false);
        }
        
        foreach (var icon in _items.Values)
        {
            icon.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_isEnd)
        {
            return;
        }
        _mapParent.rotation = Quaternion.Euler(0, 0, _player.eulerAngles.y);

        foreach (var enemy in _enemies)
        {
            var pos = (enemy.Key.transform.position - _player.position) / _mapSize * 100f;
            enemy.Value.rectTransform.anchoredPosition = new Vector3(pos.x, pos.z, 0);
        }

        foreach (var bullet in _items)
        {
            var pos = (bullet.Key.transform.position - _player.position) / _mapSize * 100f;
            bullet.Value.rectTransform.anchoredPosition = new Vector3(pos.x, pos.z, 0);
        }
    }
}
