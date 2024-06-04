using System;
using UnityEngine;

public interface IHealth
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    event Action OnHealthChanged;
    event Action<IHealth> OnHealthDestroyed;
    Transform Transform { get; }
    void Damage(int damageAmount);
    void Heal(int healAmount);
    bool IsAlive();
    void SetMaxHealth(int newMaxHealth);
}
