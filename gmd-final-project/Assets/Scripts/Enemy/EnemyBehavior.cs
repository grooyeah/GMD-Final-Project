using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    [Header("Component Values")]
    [SerializeField] private float _playerAwarenessDistance;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private float _pushBackForce = 500f;
    [SerializeField] public float _attackDistance = 2f;

    private Health _enemyHealth;
    public Transform _player; 
    private float _attackTimer;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().transform;
        _enemyHealth = GetComponent<Health>();

        if (CompareTag("Level Boss"))
        {
            _enemyHealth.SetMaxHealth(200);
        }
    }

    private void Update()
    {
        if (!_enemyHealth.IsAlive())
        {
            HandleEnemyDeath();
            return;
        }

        UpdateAwareness();
        _attackTimer -= Time.deltaTime;
    }

    private void UpdateAwareness()
    {
        Vector2 distanceToPlayer = _player.position - transform.position;
        DirectionToPlayer = distanceToPlayer.normalized;
        int layerMask = LayerMask.GetMask("Player", "Obstacles");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, DirectionToPlayer, _playerAwarenessDistance, layerMask);

        Debug.DrawRay(transform.position, DirectionToPlayer * _playerAwarenessDistance, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            AwareOfPlayer = true;
        }
        else
        {
            AwareOfPlayer = false;
        }
    }

    private void HandleEnemyDeath()
    {
        if (CompareTag("Enemy"))
        {
            ScoreManager.Instance.AddScore(10);
            SoundManager.Instance.PlayEnemyDeathSound();
            LevelManager.Instance.RegularEnemyDefeated();
        }
        else if (CompareTag("Level Boss"))
        {
            ScoreManager.Instance.AddScore(50);
            SoundManager.Instance.PlayEnemyDeathSound();
            LevelManager.Instance.BossDefeated();
        }

        LevelManager.Instance.SetEnemiesAmountText();
        Destroy(gameObject);
    }

    public void TryAttackPlayer()
    {
        if (_attackTimer <= 0f)
        {
            float distanceToPlayer = DistanceToPlayer();

            if (distanceToPlayer <= _attackDistance)
            {
                SoundManager.Instance.PlayEnemyChargeSound();
                MeleeAttackPlayer();
                _attackTimer = _attackCooldown;
            }
        }
    }

    private void MeleeAttackPlayer()
    {
        var playerHealth = _player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.Damage(_attackDamage);
            Vector2 pushDirection = (_player.position - transform.position).normalized;
            _player.GetComponent<PlayerController>()?.ApplyPushBack(pushDirection, _pushBackForce);
        }
    }
    private float DistanceToPlayer()
    {
        return Vector2.Distance(_player.position, transform.position);
    }
}

