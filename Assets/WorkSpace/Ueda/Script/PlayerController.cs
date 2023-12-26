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
    private Vector2 _currentInput;
    private bool _isDead;
    private InGameState _state = InGameState.Real;
    private float _walkingTime;
    private int BulletCount { get; set; }

    private void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.GameBGM.Main);
        AudioManager.Instance.PlaySE(AudioManager.GameSE.GameStart);
        Cursor.visible = false;
        Bind();
        BulletCount = _firstBulletCount;
        UiManager.Instance.SetBulletCountText(BulletCount);
        InGameManager.Instance.OnFinishGame.Subscribe(_ => { Cursor.lockState = CursorLockMode.None; });
    }

    private void Update()
    {
        if (_isDead) return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _currentInput = new Vector2(horizontal, vertical).normalized;
        if (_state == InGameState.Real)
        {
            if (vertical > 0)
            {
                _walkingTime += Time.deltaTime;
                if (_walkingTime > _shakePerWalkingTime)
                {
                    AudioManager.Instance.PlaySE(AudioManager.GameSE.Walk);
                    _cameraController.CameraShakeByWalk();
                    _walkingTime = 0f;
                }
            }
            else
            {
                _walkingTime = 0f;
            }
            ActivateObject();
        }
    }


    private void FixedUpdate()
    {
        if (_isDead) return;

        var dir = _cameraController.GetMoveDirection(_currentInput) *
                  (_state == InGameState.Real ? _realSpeed : _dreamSpeed);
        _rigidBody.velocity = new Vector3(dir.x, 0f, dir.z);
    }

    private void Bind()
    {
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => OnStartDream()).AddTo(this);
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => OnStartReal()).AddTo(this);
        InGameManager.Instance.OnFinishGame.Subscribe(_ => OnFinishGame()).AddTo(this);
        _hitCollider.OnTriggerEnterAsObservable().Subscribe(CheckHit).AddTo(this);
        _objectDetectCollider.OnTriggerEnterAsObservable().Subscribe(x => _hitList.Add(x)).AddTo(this);
        _objectDetectCollider.OnTriggerExitAsObservable().Subscribe(x => _hitList.Remove(x)).AddTo(this);
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
        if (Input.GetMouseButtonDown(0))
        {
            switch (activateCollider.tag)
            {
                case "Bullet":
                    print("GetBullet");
                    BulletCount++;
                    UiManager.Instance.SetBulletCountText(BulletCount);
                    AudioManager.Instance.PlaySE(AudioManager.GameSE.GetItems);
                    activateCollider.gameObject.SetActive(false);
                    MapController.Instance.SetActiveItem(activateCollider.gameObject, false);

                    _hitList.Remove(activateCollider);
                    break;
                case "HideObject":
                    print("Hide");
                    InGameManager.Instance?.ChangeInGameState(InGameState.Dream);
                    AudioManager.Instance?.PlaySE(AudioManager.GameSE.TrashDisappear);
                    Destroy(activateCollider.gameObject);
                    _hitList.Remove(activateCollider);
                    break;
                case "Enemy":
                    if (BulletCount > 0)
                    {
                        BulletCount--;
                        UiManager.Instance.SetBulletCountText(BulletCount);
                        print("KillEnemy");
                        AudioManager.Instance.PlaySE(AudioManager.GameSE.Shot);
                        //Destroy(activateCollider.gameObject);
                        var ec = activateCollider.GetComponent<EnemyController>();
                        ec.BulletHit();
                        MapController.Instance.SetActiveEnemy(ec, false);
                        _hitList.Remove(activateCollider);
                    }
                    break;
            }
        }

    }

    private void CheckHit(Collider other)
    {
        if (_isDead) return;
        switch (_state)
        {
            case InGameState.Real:
                if (other.tag.Equals("Enemy"))
                {
                    AudioManager.Instance.PlaySE(AudioManager.GameSE.GameOver);
                    InGameManager.Instance?.FinishGame(ResultType.Lose);
                    _cameraController.LookTarget(other.transform);
                    //負けの処理
                    print("負け");
                }

                break;
            case InGameState.Dream:
                if (other.tag.Equals("Enemy"))
                    //マップにアイコンを表示させる。
                    MapController.Instance.SetActiveEnemy(other.gameObject.GetComponent<EnemyController>(), true);
                //ミニマップに表示可能なImageをここで生成してもいいかもしれない。
                else if (other.tag.Equals("Bullet"))
                    //マップにアイコンを表示させる。
                    MapController.Instance.SetActiveItem(other.gameObject, true);
                //ミニマップに表示可能なImageをここで生成してもいいかもしれない。
                break;
        }
    }

    private void OnStartDream()
    {
        _state = InGameState.Dream;
        AudioManager.Instance.PlaySE(AudioManager.GameSE.GoToDream);
    }

    private void OnStartReal()
    {
        _state = InGameState.Real;
        AudioManager.Instance.PlaySE(AudioManager.GameSE.BackToReality);
    }

    private void OnFinishGame()
    {
        _isDead = true;
        _rigidBody.constraints = RigidbodyConstraints.FreezePosition;
    }
}
