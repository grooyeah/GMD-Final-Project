using UnityEngine;

public class HealthItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerHealth = collision.GetComponent<IHealth>();

            if (playerHealth != null)
            {
                ServiceLocator.Instance.GetService<ISoundManager>().PlayHealthPickupSound();

                playerHealth.Heal(20);

                Destroy(gameObject);
            }
        }
    }
}
