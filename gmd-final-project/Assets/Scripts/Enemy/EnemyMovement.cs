using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Components and values")]
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _patrolTime = 2f;
    [SerializeField] private float _stopDistance = 5f;
    [SerializeField] private bool _patrolHorizontally = true;
    [SerializeField] private float _predictionDistance = 10f;
    [SerializeField] private GameObject _graphicsObject;

    private Rigidbody2D _rb;
    private EnemyBehavior _enemyBehavior;
    private Vector2 _targetDirection;

    private float _pushBackDuration = 1f;
    private float _direction = 1f;
    private float _pushBackTimer;
    private float _patrolTimer;
    private bool _isPushedBack;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _enemyBehavior = GetComponent<EnemyBehavior>();
        _targetDirection = _patrolHorizontally ? Vector2.right : Vector2.up;
        _patrolTimer = _patrolTime;
    }

    private void Update()
    {
        if (!_isPushedBack && !_enemyBehavior.AwareOfPlayer)
        {
            Patrol();
        }

        if (_isPushedBack)
        {
            _pushBackTimer -= Time.deltaTime;
            if (_pushBackTimer <= 0)
            {
                _isPushedBack = false;
                _rb.velocity = Vector2.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isPushedBack) return;

        if (_enemyBehavior.AwareOfPlayer)
        {
            HandlePlayerTargeting();
        }
        else
        {
            _targetDirection = _patrolHorizontally ? Vector2.right * _direction : Vector2.up * _direction;
            RotateTowards(transform.position + (Vector3)_targetDirection);
            SetVelocity();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            _direction *= -1f;
            _patrolTimer = _patrolTime;
        }
    }

    private void Patrol()
    {
        _patrolTimer -= Time.deltaTime;
        if (_patrolTimer <= 0f)
        {
            _direction *= -1f;
            _patrolTimer = _patrolTime + Random.Range(-1f, 1f);
        }

        if (Random.value < 0.1f)
        {
            _rb.velocity = Vector2.zero;
        }
        else
        {
            SetVelocity();
            RotateTowards(transform.position + (Vector3)_targetDirection);
        }
    }

    private void HandlePlayerTargeting()
    {
        Vector2 playerVelocity = _enemyBehavior._player.GetComponent<Rigidbody2D>().velocity;
        Vector2 playerPosition = _enemyBehavior._player.position;
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (distanceToPlayer > _predictionDistance)
        {
            Vector2 futurePosition = playerPosition + playerVelocity * 0.5f;
            _targetDirection = (futurePosition - (Vector2)transform.position).normalized;
            RotateTowards(futurePosition);
        }
        else if (distanceToPlayer > _stopDistance && distanceToPlayer <= _predictionDistance)
        {
            _targetDirection = (playerPosition - (Vector2)transform.position).normalized;
            RotateTowards(playerPosition);
        }
        else if (distanceToPlayer <= _stopDistance)
        {
            _rb.velocity = Vector2.zero;
            RotateTowards(playerPosition);
            _enemyBehavior.TryAttackPlayer();
        }

        SetVelocity();
    }

    private void RotateTowards(Vector2 target)
    {
        if (_graphicsObject != null)
        {
            Vector2 direction = target - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _graphicsObject.transform.rotation = Quaternion.RotateTowards(
                _graphicsObject.transform.rotation,
                targetRotation,
                _rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    private void SetVelocity()
    {
        _rb.velocity = _targetDirection * _speed;
    }

    public void ApplyPushBack(Vector2 pushDirection, float pushForce)
    {
        _isPushedBack = true;
        _pushBackTimer = _pushBackDuration;
        _rb.velocity = pushDirection * pushForce;
    }
}
