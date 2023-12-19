using System;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _realSpeed;
    [SerializeField] private float _dreamSpeed;

    private readonly float _defaultSensitivity;
    private Vector2 _input;
    private Transform _cameraTransform;

    private void Start()
    {
        SetCameraTransform(Camera.main?.transform);
    }

    public void SetCameraTransform(Transform transform) 
    {
        _cameraTransform = transform;
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _input = new Vector2( horizontal ,vertical).normalized;
    }

    private void FixedUpdate()
    { 
        var cameraForward = _cameraTransform.forward;
        var cameraRight = _cameraTransform.right;
        transform.forward = new Vector3(cameraForward.x, 0f, cameraForward.z ).normalized;
        
        var  moveX= new Vector3(cameraRight.x, 0f, cameraRight.z).normalized * _input.x;
        var  moveZ = new Vector3(cameraForward.x , 0f, cameraForward.z).normalized * _input.y ;
        var dir = (moveX + moveZ).normalized * _realSpeed;
        var newVelocity = new Vector3(dir.x ,_rigidBody.velocity.y , dir.z);
        if (GameMock.Instance.Mode == GameMode.Real)
        {
            newVelocity *= _realSpeed;
        }
        else if (GameMock.Instance.Mode == GameMode.Dream)
        {
            newVelocity *= _dreamSpeed;
        }
        _rigidBody.velocity = newVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameMock.Instance.Mode == GameMode.Real)
            if (other.tag.Equals("Enemy"))
                OnPlayerDeath?.Invoke();
    }

    public event Action OnPlayerDeath;
}
