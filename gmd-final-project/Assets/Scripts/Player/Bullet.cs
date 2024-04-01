using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _lifetime = 5f;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ActivateBullet()
    {
        _rb.velocity = transform.up * _bulletSpeed;
    }

    private void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMovement>())
        {
            var enemyHealth = collision.GetComponent<Health>();
            enemyHealth.Damage(50);
            DeactivateBullet();
        }
        else if (collision.transform.name.Equals("Walls"))
        {
            DeactivateBullet();
        }

        CancelInvoke("DestroyBullet");

    }

    private void OnEnable()
    {

        Invoke("DestroyBullet", _lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyBullet");
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
