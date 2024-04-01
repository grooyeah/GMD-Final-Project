using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _health = 100;
    private int maxHealth = 100;

    public delegate void HealthChanged();
    public event HealthChanged OnHealthChanged;

    public int CurrentHealth
    {
        get { return _health; }
    }

    public void Damage(int damageAmount)
    {
        if (_health > 0)
        {
            _health -= damageAmount;
            if (_health < 0) _health = 0;
            OnHealthChanged?.Invoke();
            Debug.Log($"{gameObject.name} took damage, current health: {_health}");
            if (_health == 0 && gameObject.CompareTag("Player"))
            {
                GameManager.Instance.PlayerDied();
                SoundManager.Instance.PlayPlayerDeathSound();
            }
        }
    }

    public void Heal(int healAmount)
    {
        if (_health >= 0)
        {
            _health += healAmount;
            if (_health > maxHealth) _health = maxHealth;
            OnHealthChanged?.Invoke();
        }
    }

    public bool IsAlive()
    {
        return _health > 0;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        _health = maxHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
