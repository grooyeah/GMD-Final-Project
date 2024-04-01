using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float patrolTime = 2f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private bool patrolHorizontally = true;
    [SerializeField] private GameObject graphicsObject;

    private Rigidbody2D rb;
    private EnemyBehavior enemyBehavior;
    private Vector2 targetDirection;
    private float direction = 1f;
    private float patrolTimer;
    private bool isPushedBack;
    private float pushBackDuration = 1f;
    private float pushBackTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        enemyBehavior = GetComponent<EnemyBehavior>();
        targetDirection = patrolHorizontally ? Vector2.right : Vector2.up;
        patrolTimer = patrolTime;
    }

    private void Update()
    {
        if (!isPushedBack && !enemyBehavior.AwareOfPlayer)
        {
            Patrol();
        }

        if (isPushedBack)
        {
            pushBackTimer -= Time.deltaTime;
            if (pushBackTimer <= 0)
            {
                isPushedBack = false;
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isPushedBack) return;

        if (enemyBehavior.AwareOfPlayer)
        {
            HandlePlayerTargeting();
        }
        else
        {
            targetDirection = patrolHorizontally ? Vector2.right * direction : Vector2.up * direction;
            RotateTowardsTarget();
            SetVelocity();
        }
    }

    private void Patrol()
    {
        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0f)
        {
            direction *= -1f;
            patrolTimer = patrolTime;
        }
    }

    private void HandlePlayerTargeting()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, enemyBehavior.player.position);
        if (distanceToPlayer > stopDistance)
        {
            targetDirection = enemyBehavior.DirectionToPlayer;
            SetVelocity();
        }
        else if (distanceToPlayer <= stopDistance && distanceToPlayer > enemyBehavior.attackDistance)
        {
            targetDirection = enemyBehavior.DirectionToPlayer;
            SetVelocity();
        }
        else if (distanceToPlayer <= enemyBehavior.attackDistance)
        {
            rb.velocity = Vector2.zero;
            enemyBehavior.TryAttackPlayer();
        }
    }

    private void RotateTowardsTarget()
    {
        if (graphicsObject != null)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            graphicsObject.transform.rotation = Quaternion.RotateTowards(
                graphicsObject.transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    private void SetVelocity()
    {
        rb.velocity = targetDirection * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            direction *= -1f;
            patrolTimer = patrolTime;
        }
    }

    public void ApplyPushBack(Vector2 pushDirection, float pushForce)
    {
        isPushedBack = true;
        pushBackTimer = pushBackDuration;
        rb.velocity = pushDirection * pushForce;
    }
}
