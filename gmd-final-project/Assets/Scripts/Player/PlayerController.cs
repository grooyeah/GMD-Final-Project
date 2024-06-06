using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _pushBackDuration = 0.2f;
    [SerializeField] private float _pushBackSpeed = 20f;

    private Rigidbody2D _rb;

    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private MeleeParent _meleeParent;
    
    private float _pushBackTimer;
    private bool _isPushedBack;

    public Transform PlayerTransform => transform;
    public Rigidbody2D PlayerRigidbody => _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _meleeParent = GetComponentInChildren<MeleeParent>();
    }

    private void FixedUpdate()
    {
        if (_isPushedBack)
        {
            _pushBackTimer -= Time.deltaTime;

            if (_pushBackTimer <= 0)
            {
                _isPushedBack = false;
            }
        }

        Move();
        RotateDirectionOnInput();
    }

    private void RotateDirectionOnInput()
    {
        if (_smoothMovementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _rb.MoveRotation(rotation);
        }
    }

    private void Move()
    {
        _smoothMovementInput = Vector2.SmoothDamp(
            _smoothMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f
        );

        _rb.velocity = _isPushedBack ? _rb.velocity : _smoothMovementInput * _moveSpeed;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void OnSwing(InputValue inputValue)
    {
        _meleeParent.Attack();
    }

    private void OnApplicationExit(InputValue inputValue)
    {
        Application.Quit();
    }

    public void ApplyPushBack(Vector2 pushDirection, float pushForce)
    {
        ServiceLocator.Instance.GetService<ISoundManager>().PlayPlayerHitSound();

        _isPushedBack = true;
        _pushBackTimer = _pushBackDuration;
        _rb.velocity = pushDirection * _pushBackSpeed;
    }
}
