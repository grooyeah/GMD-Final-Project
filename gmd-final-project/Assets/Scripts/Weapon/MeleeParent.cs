using System.Collections;
using UnityEngine;

public class MeleeParent : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Collider2D swordCollider;

    [SerializeField]
    private float pushBackForce = 1000f;

    private float delay = 0.3f;
    private bool attackBlocked;
    private bool isAttacking = false;

    void Start()
    {
        swordCollider.enabled = false;
    }

    public void Attack()
    {
        if (attackBlocked)
        {
            return;
        }

        animator.SetTrigger("Attack");
        ServiceLocator.Instance.GetService<ISoundManager>().PlaySwordWhooshSound();
        attackBlocked = true;
        isAttacking = true;
        swordCollider.enabled = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
        isAttacking = false;
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking && collision.GetComponent<EnemyMovement>())
        {
            var enemyHealth = collision.GetComponent<IHealth>();
            if (enemyHealth != null && enemyHealth.IsAlive())
            {
                ServiceLocator.Instance.GetService<ISoundManager>().PlayEnemyHitSound();
                enemyHealth.Damage(100);
                PushBackEnemy(collision.transform);
            }
        }
        else if(isAttacking && collision.CompareTag("Objects"))
        {
            ServiceLocator.Instance.GetService<ISoundManager>().PlayObjectBreakingSound();
            ServiceLocator.Instance.GetService<IScoreManager>().AddScore(5);
            Destroy(collision.gameObject);
        }
    }

    private void PushBackEnemy(Transform enemyTransform)
    {
        Vector2 pushDirection = (enemyTransform.position - transform.position).normalized;
        EnemyMovement enemyMovement = enemyTransform.GetComponent<EnemyMovement>();
        enemyMovement?.ApplyPushBack(pushDirection, pushBackForce);
    }
}
