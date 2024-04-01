using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                SoundManager.Instance.PlayHealthPickupSound();
                playerHealth.Heal(20);
                Destroy(gameObject);
            }
        }
    }
}
