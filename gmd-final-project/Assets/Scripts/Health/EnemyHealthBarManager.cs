using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHealthBarManager : MonoBehaviour, IEnemyHealthBarManager
{
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private Canvas _mainCanvas;

    private Dictionary<IHealth, GameObject> _healthBars = new Dictionary<IHealth, GameObject>();

    public void InstantiateHealthBars()
    {
        var healthScripts = FindObjectsOfType<Health>().Cast<IHealth>().Where(h => !((Component)h).CompareTag("Player"));

        foreach (var health in healthScripts)
        {
            CreateHealthBar(health);
        }
    }

    private void Update()
    {
        UpdateHealthBars();
    }

    private void UpdateHealthBars()
    {
        var toRemove = new List<IHealth>();

        foreach (var kvp in _healthBars)
        {
            var health = kvp.Key;
            var healthBarGO = kvp.Value;

            if (health != null && healthBarGO != null && !ReferenceEquals(((Component)health).transform, null))
            {
                UpdateHealthBarPosition(health, healthBarGO);
            }
            else
            {
                toRemove.Add(health);
            }
        }

        foreach (var health in toRemove)
        {
            if (_healthBars.ContainsKey(health))
            {
                if (_healthBars[health] != null)
                {
                    Destroy(_healthBars[health]);
                }

                _healthBars.Remove(health);
            }
        }
    }

    private void UpdateHealthBarPosition(IHealth health, GameObject healthBar)
    {
        if (!ReferenceEquals(((Component)health).transform, null))
        {
            var placeholder = ((Component)health).transform.Find("Healthbar Placeholder");

            healthBar.transform.position = Camera.main.WorldToScreenPoint(placeholder?.position ?? ((Component)health).transform.position + new Vector3(0, 1.5f, 0));
        }
    }

    private void CreateHealthBar(IHealth health)
    {
        if (_healthBars.ContainsKey(health))
        {
            return;
        }

        var healthBarGO = Instantiate(_healthBarPrefab, _mainCanvas.transform);

        UpdateHealthBarPosition(health, healthBarGO);

        var healthBar = healthBarGO.GetComponent<HealthBar>();

        healthBar.targetHealth = (Health)health;

        healthBar.Initialize();

        _healthBars[health] = healthBarGO;

        health.OnHealthDestroyed += HandleHealthDestroyed;
    }

    private void HandleHealthDestroyed(IHealth health)
    {
        if (_healthBars.ContainsKey(health))
        {
            if (_healthBars[health] != null)
            {
                Destroy(_healthBars[health]);
            }

            _healthBars.Remove(health);
        }
    }

    public void HideHealthBars()
    {
        foreach (var healthBar in _healthBars.Values)
        {
            healthBar.SetActive(false);
        }
    }

    public void ClearHealthBars()
    {
        foreach (var kvp in _healthBars)
        {
            if (kvp.Value != null)
            {
                Destroy(kvp.Value);
            }
        }

        _healthBars.Clear();
    }
}


