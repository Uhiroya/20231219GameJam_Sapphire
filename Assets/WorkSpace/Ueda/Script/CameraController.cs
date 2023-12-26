using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] [Range(0.9f, 1f)] private float _objectWithInView;
    [SerializeField] private float _blendFovTime;
    private float _currentFov;
    private float _defaultFov;
    public bool IsActiveMouseDelta { get; set; } = false;

    private void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        Cursor.lockState = CursorLockMode.Locked;
        _defaultFov = _camera.m_Lens.FieldOfView;
        Bind();
        IsActiveMouseDelta = true;
    }


    private void Bind()
    {
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => OnStartDream()).AddTo(this);
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => OnStartReal()).AddTo(this);
        InGameManager.Instance.OnFinishGame.Subscribe(_ => OnFinishGame()).AddTo(this);
    }
    
    public Vector3 GetMoveDirection(Vector2 input)
    {
        var cameraForward = _cameraTransform.forward;
        var cameraRight = _cameraTransform.right;
        transform.forward = new Vector3(cameraForward.x, 0f, cameraForward.z);

        var moveX = new Vector3(cameraRight.x, 0f, cameraRight.z) * input.x;
        var moveZ = new Vector3(cameraForward.x, 0f, cameraForward.z) * input.y;
        return (moveX + moveZ).normalized;
    }

    public void LookTarget(Transform target)
    {
        _camera.enabled = false;
        var cameraTransform = Camera.main.transform;
        cameraTransform.position += target.forward.normalized * 1.5f;
        cameraTransform.DOLookAt(target.position, 0.5f);
    }
    private float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (IsActiveMouseDelta) return Input.GetAxis("Mouse X");
            return 0;
        }

        if (axisName == "Mouse Y")
        {
            if (IsActiveMouseDelta) return Input.GetAxis("Mouse Y");
            return 0;
        }

        return Input.GetAxis(axisName);
    }

    public bool JudgeWithinPlayerView(Vector3 target, out float dot)
    {
        var targetDir = target - _cameraTransform.position;
        dot = Vector3.Dot(targetDir.normalized, _cameraTransform.forward);
        if (_objectWithInView < dot) return true;
        return false;
    }

    public void CameraShakeByWalk()
    {
        var force = new Vector3(Random.Range(-0.5f, 0.5f), -1f, 0f);
        _impulseSource.GenerateImpulse(force);
    }

    private void OnStartDream()
    {
        //_state = InGameState.Dream;   
        _currentFov = _defaultFov * 1.5f;
        DOVirtual.Float(
            _defaultFov,
            _currentFov,
            _blendFovTime,
            value => _camera.m_Lens.FieldOfView = value
        );
    }

    private void OnStartReal()
    {
        DOVirtual.Float(
            _currentFov,
            _defaultFov,
            _blendFovTime,
            value => _camera.m_Lens.FieldOfView = value
        ).OnComplete(() => _currentFov = _defaultFov);
    }

    private void OnFinishGame()
    {
        IsActiveMouseDelta = false;
    }
}
