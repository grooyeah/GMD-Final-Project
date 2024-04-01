using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField] private float playerAwarenessDistance;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float pushBackForce = 500f;
    [SerializeField] public float attackDistance = 2f;

    private Health enemyHealth;
    public Transform player; 
    private float attackTimer;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        enemyHealth = GetComponent<Health>();
        if (this.gameObject.tag.Equals("Level Boss"))
        {
            enemyHealth.SetMaxHealth(200);
        }
    }

    private void Update()
    {
        if (!enemyHealth.IsAlive())
        {
            Debug.Log($"Enemy {gameObject.name} died.");

            if (this.gameObject.tag.Equals("Enemy"))
            {
                ScoreManager.Instance.AddScore(10);
                SoundManager.Instance.PlayEnemyDeathSound();
                LevelManager.Instance.RegularEnemyDefeated();
            }
            else if (this.gameObject.tag.Equals("Level Boss"))
            {
                ScoreManager.Instance.AddScore(50);
                SoundManager.Instance.PlayEnemyDeathSound();
                LevelManager.Instance.BossDefeated();
            }

            LevelManager.Instance.SetEnemiesAmountText();
            Destroy(gameObject);
            return;
        }

        UpdateAwareness();
        attackTimer -= Time.deltaTime;
    }

    private void UpdateAwareness()
    {
        Vector2 distanceToPlayer = player.position - transform.position;
        DirectionToPlayer = distanceToPlayer.normalized;
        AwareOfPlayer = distanceToPlayer.magnitude <= playerAwarenessDistance;
    }

    public void TryAttackPlayer()
    {
        if (attackTimer <= 0f && DistanceToPlayer() <= attackDistance)
        {
            SoundManager.Instance.PlayEnemyChargeSound();
            AttackPlayer();
            attackTimer = attackCooldown;
        }
    }

    private float DistanceToPlayer()
    {
        return Vector2.Distance(player.position, transform.position);
    }

    private void AttackPlayer()
    {
        var playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.Damage(attackDamage);
            Vector2 pushDirection = (player.position - transform.position).normalized;
            player.GetComponent<PlayerController>()?.ApplyPushBack(pushDirection, pushBackForce);
        }
    }
}

