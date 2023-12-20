using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : SingletonMonoBehavior<EnemyManager>
{
    private Dictionary<int, EnemyController> _enemyDictionary = new Dictionary<int, EnemyController>();

    void Start()
    {
        var enemies = FindObjectsOfType<EnemyController>();

        for (int i = 0; i < enemies.Length; i++)
        {
            _enemyDictionary.Add(i, enemies[i]);
        }

        UiManager.Instance.SetEnemyCountText(enemies.Length);
    }

    public void DeadEnemy(EnemyController enemyController)
    {
        if (_enemyDictionary.ContainsValue(enemyController) == false)
        {
#if UNITY_EDITOR
            Debug.LogWarning("敵が登録されてない");
#endif
            return;
        }

        UiManager.Instance.SetEnemyCountText(_enemyDictionary.Values.Count(x => x.IsDead == false));


        if (_enemyDictionary.Values.All(x => x.IsDead))
        {
            Debug.Log("敵全員死にました");
            InGameManager.Instance.FinishGame(ResultType.Win);
        }
    }
}
