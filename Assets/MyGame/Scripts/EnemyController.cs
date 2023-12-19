using UnityEngine;
using UnityEngine.AI;
using UniRx;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Tooltip("巡回する場所")]
    private Transform[] _patrolPoints;

    [SerializeField, Tooltip("巡回する速度")]
    private float _patrolSpeed = 3;

    [SerializeField, Tooltip("追いかける速度")]
    private float _chaseSpeed = 5;

    [SerializeField, Tooltip("Playerを追いかける距離")]
    private float _chaseDistance = 6;

    [SerializeField]//仮
    private Transform _playerTransform;

    //現在のパトロール地点
    private int _targetPoint = 0;

    private NavMeshAgent _agent;

    private EnemyState _enemyState = EnemyState.Patrol;

    private void Start()
    {
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => _enemyState = EnemyState.Stop)
            .AddTo(this);
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => _enemyState = EnemyState.Chase)
            .AddTo(this);

        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
        _agent.speed = _patrolSpeed;
        Patrol();
    }

    private void Update()
    {
        switch (_enemyState)
        {
            case EnemyState.Patrol:

                Search();
                if (_agent.remainingDistance <= 0)
                {
                    Patrol();//巡回先の変更
                }

                break;

            case EnemyState.Chase:

                Chase();

                break;
            case EnemyState.Stop:

                _agent.destination = transform.position;

                break;
        }

    }

    /// <summary>
    /// Playerを探す
    /// </summary>
    private void Search()
    {
        var raycastAll = Physics.RaycastAll(transform.position, transform.forward, _chaseDistance);

        foreach (var hit in raycastAll)
        {
            //PlayerにRayが当たったら追いかける
            if (hit.collider.name == _playerTransform.gameObject.name)
            {
                _enemyState = EnemyState.Chase;
            }
            break;
        }
    }

    /// <summary>
    /// 巡回
    /// </summary>
    private void Patrol()
    {
        if (_patrolPoints.Length == 0 || _patrolPoints[_targetPoint] == null)
        {
            Debug.LogError("パトロール地点が設定されていません");
            return;
        }

        _agent.destination = _patrolPoints[_targetPoint].position;

        _targetPoint = (_targetPoint + 1) % _patrolPoints.Length;
    }

    /// <summary>
    /// Playerを追いかける
    /// </summary>
    private void Chase()
    {
        _agent.speed = _chaseSpeed;
        _agent.destination = _playerTransform.position;
    }

    /// <summary>
    /// Enemyを破棄する
    /// </summary>
    public void BulletHit()
    {
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * _chaseDistance);
    }
}
public enum EnemyState
{
    Patrol,
    Chase,
    Stop,
}
