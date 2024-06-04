using System;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{

    [SerializeField] private int _health = 100;
    [SerializeField] private int _maxHealth = 100;

    public event Action OnHealthChanged;
    public event Action<IHealth> OnHealthDestroyed;
    public int CurrentHealth => _health;
    public int MaxHealth => _maxHealth;
    public Transform Transform => transform;

    public void Damage(int damageAmount)
    {
        if (_health > 0)
        {
            _health -= damageAmount;

            if (_health < 0) _health = 0;

            OnHealthChanged?.Invoke();

            if (gameObject.CompareTag("Player") || gameObject.CompareTag("Enemy") || gameObject.CompareTag("Level Boss"))
            {
                var flashEffect = GetComponentInChildren<SpriteRendererFlash>();

                if (flashEffect != null)
                {
                    flashEffect.StartFlash();
                }
            }

            if (gameObject.CompareTag("Player") && !IsAlive())
            {
                GameManager.Instance.PlayerDied();
                SoundManager.Instance.PlayPlayerDeathSound();
            }

            if (!IsAlive())
            {
                OnHealthDestroyed?.Invoke(this);
            }
        }
    }

    public void Heal(int healAmount)
    {
        if (_health >= 0)
        {
            _health += healAmount;

            if (_health > _maxHealth) _health = _maxHealth;

            OnHealthChanged?.Invoke();
        }
    }

    public bool IsAlive()
    {
        return _health > 0;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;

        _health = _maxHealth;
    }
}