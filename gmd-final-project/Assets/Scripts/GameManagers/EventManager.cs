using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action<IHealth> OnHealthChanged;
    public event Action OnPlayerDied;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HealthChanged(IHealth health)
    {
        OnHealthChanged?.Invoke(health);
    }

    public void PlayerDied()
    {
        OnPlayerDied?.Invoke();
    }
}
