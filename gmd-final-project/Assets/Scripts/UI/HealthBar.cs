using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public IHealth targetHealth;
    private Slider _healthSlider;
    private GameObject _healthBarGameObject;
    private Coroutine _hideCoroutine;

    void Awake()
    {
        _healthSlider = GetComponent<Slider>();
        _healthBarGameObject = this.gameObject;
    }

    void Start()
    {
        if (targetHealth != null)
        {
            _healthSlider.maxValue = targetHealth.MaxHealth;

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
            _healthSlider.value = targetHealth.CurrentHealth;

            _healthBarGameObject.SetActive(targetHealth.Transform.CompareTag("Player") || targetHealth.CurrentHealth < targetHealth.MaxHealth);

            targetHealth.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void UpdateHealthBar()
    {
        if (targetHealth != null)
        {
            _healthSlider.value = targetHealth.CurrentHealth;

            if (!targetHealth.Transform.CompareTag("Player"))
            {
                ShowHealthBar();
            }
        }
    }

    private void ShowHealthBar()
    {
        _healthBarGameObject.SetActive(true);

        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
        }

        _hideCoroutine = StartCoroutine(HideHealthBarDelay());
    }

    private IEnumerator HideHealthBarDelay()
    {
        yield return new WaitForSeconds(3);
        _healthBarGameObject.SetActive(false);
    }
}
