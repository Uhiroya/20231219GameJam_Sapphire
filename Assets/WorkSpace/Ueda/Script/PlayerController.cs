using Cinemachine;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _realSpeed;
    [SerializeField] private float _dreamSpeed;

    private readonly float _defaultSensitivity;
    private Transform _cameraTransform;
    private Vector2 _input;
    private InGameState _state;

    private void Start()
    {
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => OnStartDream());
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => OnStartReal());
        _cameraTransform = _camera.transform;
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _input = new Vector2(horizontal, vertical).normalized;
    }

    private void FixedUpdate()
    {
        var cameraForward = _cameraTransform.forward;
        var cameraRight = _cameraTransform.right;
        transform.forward = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;

        var moveX = new Vector3(cameraRight.x, 0f, cameraRight.z).normalized * _input.x;
        var moveZ = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized * _input.y;
        var dir = (moveX + moveZ).normalized * _realSpeed;
        var newVelocity = new Vector3(dir.x, _rigidBody.velocity.y, dir.z);
        if (_state == InGameState.Real)
            newVelocity *= _realSpeed;
        else if (_state == InGameState.Dream) 
            newVelocity *= _dreamSpeed;
        _rigidBody.velocity = newVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (InGameManager.Instance.CurrentState == InGameState.Real)
            if (other.tag.Equals("Enemy"))
                InGameManager.Instance?.FinishGame(ResultType.Lose);
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
