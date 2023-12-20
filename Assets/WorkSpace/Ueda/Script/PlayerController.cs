using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static readonly string[] ActivateObjectsTag = { "Bullet", "HideObject", "Enemy" };
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Collider _hitCollider;
    [SerializeField] private Collider _objectDetectCollider;
    [SerializeField] private float _realSpeed;
    [SerializeField] private float _dreamSpeed;
    [SerializeField] private int _firstBulletCount = 1;
    [SerializeField] private float _shakePerWalkingTime;
    private readonly List<Collider> _hitList = new(10);
    private readonly bool _isWaiting = false;
    private Vector2 _currentInput;
    private InGameState _state = InGameState.Real;
    private float _walkingTime;
    public int BulletCount { get; private set; }

    private void Start()
    {
        Cursor.visible = false;
        Bind();
        BulletCount = _firstBulletCount;
        UiManager.Instance.SetBulletCountText(BulletCount);
    }

    private void Bind()
    {
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => OnStartDream()).AddTo(this);
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => OnStartReal()).AddTo(this);
        _hitCollider.OnTriggerEnterAsObservable().Subscribe(CheckHit).AddTo(this);
        _objectDetectCollider.OnTriggerEnterAsObservable().Subscribe(x => _hitList.Add(x)).AddTo(this);
        _objectDetectCollider.OnTriggerExitAsObservable().Subscribe(x => _hitList.Remove(x)).AddTo(this);
    }

    private void Update()
    {
        if (_isWaiting) return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _currentInput = new Vector2(horizontal, vertical).normalized;
        if (_state == InGameState.Real && vertical > 0)
        {
            _walkingTime += Time.deltaTime;
            if (_walkingTime > _shakePerWalkingTime)
            {
                _cameraController.CameraShakeByWalk();
                _walkingTime = 0f;
            }
        }
        else
        {
            _walkingTime = 0f;
        }
        if (Input.GetMouseButtonDown(0)) ActivateObject();
    }

    /// <summary>
    ///     遷移中は待機させるためにGameStateをWaitingに切り替えたい。
    ///     切り替え開始、終わりのイベントがあると嬉しい。
    /// </summary>
    private void FixedUpdate()
    {
        if (_isWaiting) return;

        var dir = _cameraController.GetMoveDirection(_currentInput) *
                  (_state == InGameState.Real ? _realSpeed : _dreamSpeed);
        _rigidBody.velocity = new Vector3(dir.x, _rigidBody.velocity.y, dir.z);
    }

    /// <summary>
    ///     カメラの中心により近いアクティブ可能なオブジェクトをアクティブにする。
    /// </summary>
    private void ActivateObject()
    {
        Collider activateCollider = null;

        //中心に一番近いオブジェクトの判定
        float dotMax = -1;
        foreach (var collider in _hitList.Where(x => ActivateObjectsTag.Contains(x.tag)))
            if (_cameraController.JudgeWithinPlayerView(collider.transform.position, out var dot))
                if (dot > dotMax)
                {
                    dotMax = dot;
                    activateCollider = collider;
                }

        if (!activateCollider) return;

        switch (activateCollider.tag)
        {
            case "Bullet":
                print("GetBullet");
                BulletCount++;
                UiManager.Instance.SetBulletCountText(BulletCount);

                Destroy(activateCollider.gameObject);
                _hitList.Remove(activateCollider);
                break;
            case "HideObject":
                print("Hide");
                InGameManager.Instance?.ChangeInGameState(InGameState.Dream);

                Destroy(activateCollider.gameObject);
                _hitList.Remove(activateCollider);
                break;
            case "Enemy":
                if (BulletCount > 0)
                {
                    BulletCount--;
                    UiManager.Instance.SetBulletCountText(BulletCount);
                    print("KillEnemy");

                    //Destroy(activateCollider.gameObject);
                    activateCollider.GetComponent<EnemyController>().BulletHit();
                    _hitList.Remove(activateCollider);
                }

                break;
        }
    }

    private void CheckHit(Collider other)
    {
        switch (_state)
        {
            case InGameState.Real:
                if (other.tag.Equals("Enemy"))
                {
                    InGameManager.Instance?.FinishGame(ResultType.Lose);
                    //負けの処理
                    print("負け");
                }

                break;
            case InGameState.Dream:
                if (other.tag.Equals("Enemy"))
                {
                    //マップにアイコンを表示させる。
                    MapController.Instance.SetActiveEnemy(other.gameObject.GetComponent<EnemyController>(),true);
                    //ミニマップに表示可能なImageをここで生成してもいいかもしれない。
                }
                else if (other.tag.Equals("Bullet"))
                {
                    //マップにアイコンを表示させる。
                    MapController.Instance.SetActiveItem(other.gameObject,true);
                    //ミニマップに表示可能なImageをここで生成してもいいかもしれない。
                }

                break;
        }
    }

    private void OnStartDream()
    {
        _state = InGameState.Dream;
    }

    private void OnStartReal()
    {
        _state = InGameState.Real;
    }
}
