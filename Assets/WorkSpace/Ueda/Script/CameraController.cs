using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] [Range(0.9f, 1f)] private float _objectWithInView;
    [SerializeField] private float _blendFovTime;

    private float _defaultFov;

    //private InGameState _state;
    private void Start()
    {
        _defaultFov = _camera.m_Lens.FieldOfView;
        Bind();
    }

    private void Bind()
    {
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => OnStartDream()).AddTo(this);
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => OnStartReal()).AddTo(this);
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

    public bool JudgeWithinPlayerView(Vector3 target, out float dot)
    {
        var targetDir = target - _cameraTransform.position;
        dot = Vector3.Dot(targetDir.normalized, _cameraTransform.forward);
        if (_objectWithInView < dot) return true;
        return false;
    }

    private void OnStartDream()
    {
        //_state = InGameState.Dream;   
        var nextFov = _defaultFov * 1.5f;
        DOVirtual.Float(
            _defaultFov,
            nextFov,
            _blendFovTime,
            value => _camera.m_Lens.FieldOfView = value
        );
    }

    private void OnStartReal()
    {
        //_state = InGameState.Real;
        _camera.m_Lens.FieldOfView = _defaultFov;
    }
}
