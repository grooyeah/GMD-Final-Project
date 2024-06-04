using UnityEngine;

public class PlayerHealthBarManager : MonoBehaviour, IPlayerHealthBarManager
{
    [SerializeField] private GameObject _playerHealthBar; 

    private void Start()
    {
        InitializePlayerHealthBar();
    }

    public void InstantiateHealthBars()
    {
        InitializePlayerHealthBar();
    }

    private void InitializePlayerHealthBar()
    {
        var playerHealth = ServiceLocator.Instance.GetService<IPlayerController>().PlayerTransform.GetComponent<IHealth>();

        if (playerHealth != null && _playerHealthBar != null)
        {
            var healthBar = _playerHealthBar.GetComponent<HealthBar>();

            healthBar.targetHealth = playerHealth;

            healthBar.Initialize();
        }
        else
        {
            Debug.LogWarning("Player health component or health bar not found.");
        }
    }

    public void HideHealthBars()
    {
        if (_playerHealthBar != null)
        {
            _playerHealthBar.SetActive(false);
        }
    }
}

