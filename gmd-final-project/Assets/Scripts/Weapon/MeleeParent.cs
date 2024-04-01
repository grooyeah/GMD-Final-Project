using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeParent : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Collider2D swordCollider; // Collider for the sword hitbox

    [SerializeField]
    private float pushBackForce = 1000f; // Force to push the enemy back

    private float delay = 0.3f;
    private bool attackBlocked;
    private bool isAttacking = false;

    void Start()
    {
        swordCollider.enabled = false; // Start with the collider disabled
    }

    void Update()
    {
        // Additional logic if needed
    }

    public void Attack()
    {
        if (attackBlocked)
        {
            return;
        }

        animator.SetTrigger("Attack");
        SoundManager.Instance.PlaySwordWhooshSound();
        attackBlocked = true;
        isAttacking = true;
        swordCollider.enabled = true; // Enable the collider at the start of the attack
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
        isAttacking = false;
        swordCollider.enabled = false; // Disable the collider after the attack
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking && collision.GetComponent<EnemyMovement>())
        {
            var enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null && enemyHealth.IsAlive())
            {
                SoundManager.Instance.PlayEnemyHitSound();
                enemyHealth.Damage(100);
                PushBackEnemy(collision.transform);
            }
        }
    }

    private void PushBackEnemy(Transform enemyTransform)
    {
        Vector2 pushDirection = (enemyTransform.position - transform.position).normalized;
        EnemyMovement enemyMovement = enemyTransform.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.ApplyPushBack(pushDirection, pushBackForce);
        }
    }
}
