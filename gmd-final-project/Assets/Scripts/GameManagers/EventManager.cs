using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep EventManager alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public event Action<Health> OnHealthChanged;

    public void HealthChanged(Health health)
    {
        OnHealthChanged?.Invoke(health);
    }
}
