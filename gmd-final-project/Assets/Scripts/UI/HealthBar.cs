using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health targetHealth;
    private Slider healthSlider;
    private GameObject healthBarGameObject;
    private Coroutine hideCoroutine;

    void Awake()
    {
        healthSlider = GetComponent<Slider>();
        healthBarGameObject = this.gameObject;
    }

    void Start()
    {
        if (targetHealth != null)
        {
            healthSlider.maxValue = targetHealth.GetMaxHealth(); // Ensure max value matches target's max health
            targetHealth.OnHealthChanged += UpdateHealthBar;
            Initialize();
        }
        else
        {
            Debug.LogWarning("Target health not assigned in HealthBar.");
        }
    }

    void OnDestroy()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    public void Initialize()
    {
        if (targetHealth != null)
        {
            healthSlider.value = targetHealth.CurrentHealth;
            healthBarGameObject.SetActive(targetHealth.CompareTag("Player") || targetHealth.CurrentHealth < targetHealth.GetMaxHealth());

            targetHealth.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void UpdateHealthBar()
    {
        if (targetHealth != null)
        {
            healthSlider.value = targetHealth.CurrentHealth;
            if (!targetHealth.CompareTag("Player"))
            {
                ShowHealthBar();
            }
        }
    }

    private void ShowHealthBar()
    {
        healthBarGameObject.SetActive(true);
        Debug.Log($"{targetHealth.gameObject.name} health bar shown");
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(HideHealthBarDelay());
    }

    private IEnumerator HideHealthBarDelay()
    {
        yield return new WaitForSeconds(3);
        healthBarGameObject.SetActive(false);
        Debug.Log($"{targetHealth.gameObject.name} health bar hidden");
    }
}