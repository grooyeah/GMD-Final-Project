using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab; // Assign in inspector
    public Canvas mainCanvas;          // Assign the main UI canvas in the inspector
    public GameObject playerHealthBar; // Assign the player's health bar in the inspector

    public Dictionary<Health, GameObject> HealthBars = new Dictionary<Health, GameObject>();

    void Start()
    {
        InitializePlayerHealthBar();
        Invoke("InstantiateHealthBars", 0.1f); // Delay instantiation slightly to ensure all enemies are active
    }

    public void InstantiateHealthBars()
    {
        Health[] healthScripts = FindObjectsOfType<Health>();
        foreach (Health health in healthScripts)
        {
            if (health.gameObject.tag != "Player")
            {
                CreateHealthBar(health);
            }
        }
    }

    private void InitializePlayerHealthBar()
    {
        Health playerHealth = FindObjectOfType<PlayerController>().GetComponent<Health>();
        if (playerHealth != null)
        {
            HealthBar healthBar = playerHealthBar.GetComponent<HealthBar>();
            healthBar.targetHealth = playerHealth;
            healthBar.Initialize(); // Initialize the health bar value
            Debug.Log("Player Health Bar Initialized.");
        }
        else
        {
            Debug.LogWarning("Player health component not found.");
        }
    }

    void Update()
    {
        UpdateHealthBars();
        RemoveDestroyedHealthBars();
    }

    private void UpdateHealthBars()
    {
        foreach (var kvp in HealthBars)
        {
            Health health = kvp.Key;
            GameObject healthBarGO = kvp.Value;

            if (health != null && healthBarGO != null)
            {
                UpdateHealthBarPosition(health, healthBarGO);
            }
        }
    }

    private void RemoveDestroyedHealthBars()
    {
        List<Health> toRemove = new List<Health>();

        foreach (var kvp in HealthBars)
        {
            Health health = kvp.Key;
            GameObject healthBarGO = kvp.Value;

            if (health == null || healthBarGO == null)
            {
                toRemove.Add(health);
            }
        }

        foreach (Health health in toRemove)
        {
            if (HealthBars.ContainsKey(health))
            {
                Destroy(HealthBars[health]);
                HealthBars.Remove(health);
            }
        }
    }

    private void UpdateHealthBarPosition(Health health, GameObject healthBar)
    {
        Transform placeholder = health.transform.Find("Healthbar Placeholder");
        if (placeholder != null)
        {
            healthBar.transform.position = Camera.main.WorldToScreenPoint(placeholder.position);
        }
        else
        {
            healthBar.transform.position = Camera.main.WorldToScreenPoint(health.transform.position + new Vector3(0, 1.5f, 0)); // Adjust Y offset as needed
            Debug.LogWarning($"Healthbar Placeholder not found on {health.gameObject.name}");
        }
    }

    private void CreateHealthBar(Health health)
    {
        if (HealthBars.ContainsKey(health))
        {
            return; // Avoid creating duplicate health bars
        }

        GameObject healthBarGO = Instantiate(healthBarPrefab, mainCanvas.transform);
        Transform placeholder = health.transform.Find("Healthbar Placeholder");
        if (placeholder != null)
        {
            healthBarGO.transform.position = Camera.main.WorldToScreenPoint(placeholder.position);
        }
        else
        {
            healthBarGO.transform.position = Camera.main.WorldToScreenPoint(health.transform.position + new Vector3(0, 1.5f, 0)); // Adjust Y offset as needed
            Debug.LogWarning($"Healthbar Placeholder not found on {health.gameObject.name}");
        }
        HealthBar healthBar = healthBarGO.GetComponent<HealthBar>();
        healthBar.targetHealth = health;
        healthBar.Initialize(); // Initialize health bar with current health
        HealthBars[health] = healthBarGO;
    }

    public void HideHealthBars()
    {
        foreach (var healthBar in HealthBars.Values)
        {
            healthBar.SetActive(false);
        }
    }
}