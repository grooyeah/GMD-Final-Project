using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour, ILevelManager
{
    public static ILevelManager Instance { get; private set; }

    [SerializeField] private GameObject[] _levelPrefabs;
    [SerializeField] private GameObject _portalPrefab;

    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _enemiesAmount;
    
    private int _currentLevelIndex = -1;

    private GameObject _currentLevel;
    private GameObject _portal;

    private Transform _playerSpawnPoint;
    private Transform _portalSpawnPoint;

    private int _bossesRemaining = 0;
    private int _regularEnemiesRemaining = 0;

    private ICollection<GameObject> _bosses;

    private IEnemyHealthBarManager _enemyHealthBarManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _bosses = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }

        _enemyHealthBarManager = ServiceLocator.Instance.GetService<IEnemyHealthBarManager>();
    }

    private void Start()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        if (_currentLevelIndex >= 0 && _currentLevel != null)
        {
            Destroy(_currentLevel);
        }

        _currentLevelIndex++;

        if (_currentLevelIndex < _levelPrefabs.Length)
        {
            _currentLevel = Instantiate(_levelPrefabs[_currentLevelIndex]);
            FindSpawnPoints();
            PositionPlayerAtSpawnPoint();
            Destroy(_portal);
            SetLevelNameText();

            InitializeEnemies();
        }
        else
        {
            UIManager.Instance.EndGame();
        }
    }

    public void endgame()
    {
        UIManager.Instance.EndGame();
    }

    private void InitializeEnemies()
    {
        var enemies = _currentLevel.transform.Find($"Level {_currentLevelIndex + 1} Enemies").GetComponentsInChildren<EnemyBehavior>();
        _regularEnemiesRemaining = enemies.Count(e => e.CompareTag("Enemy"));
        _bossesRemaining = enemies.Count(e => e.CompareTag("Level Boss"));

        _bosses.Clear();
        foreach (var boss in enemies.Where(e => e.CompareTag("Level Boss")))
        {
            _bosses.Add(boss.gameObject);
            boss.gameObject.SetActive(false);
        }
        SetEnemiesAmountText();
        StartCoroutine(InitializeHealthBars());
    }

    private IEnumerator InitializeHealthBars()
    {
        yield return new WaitForEndOfFrame();
        _enemyHealthBarManager.InstantiateHealthBars();
    }

    private void FindSpawnPoints()
    {
        _playerSpawnPoint = _currentLevel.transform.Find("Level " + (_currentLevelIndex + 1) + " Spawnpoint");
        _portalSpawnPoint = _currentLevel.transform.Find("Level " + (_currentLevelIndex + 1) + " Portal Spawnpoint");

        if (_playerSpawnPoint == null || _portalSpawnPoint == null)
        {
            Debug.LogError("Spawn points not found in the level prefab.");
        }
    }

    private void PositionPlayerAtSpawnPoint()
    {
        var player = ServiceLocator.Instance.GetService<IPlayerController>().PlayerTransform.gameObject;
        player.transform.position = _playerSpawnPoint.position;
    }

    public void SpawnPortal()
    {
        if (_bossesRemaining == 0)
        {
            _portal = Instantiate(_portalPrefab, _portalSpawnPoint.position, Quaternion.identity);
            _portal.transform.position = _portalSpawnPoint.transform.position;
            StartCoroutine(ScalePortal());
        }
    }

    private IEnumerator ScalePortal()
    {
        float elapsedTime = 0f;
        float scalingDuration = 0.5f;
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
        ServiceLocator.Instance.GetService<ISoundManager>().PlayPortalOpenSound();
        while (elapsedTime < scalingDuration)
        {
            _portal.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / scalingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _portal.transform.localScale = targetScale;
    }

    public void RestartLevel()
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        _enemyHealthBarManager.ClearHealthBars();
        var player = ServiceLocator.Instance.GetService<IPlayerController>().PlayerTransform.gameObject;
        player.GetComponent<IHealth>().Heal(100);
        _currentLevel = Instantiate(_levelPrefabs[_currentLevelIndex]);
        FindSpawnPoints();
        PositionPlayerAtSpawnPoint();
        InitializeEnemies();
        ServiceLocator.Instance.GetService<IScoreManager>().ResetScore();
    }

    public void SetLevelNameText()
    {
        _levelName.text = _currentLevel.name.Replace("(Clone)", "");
    }

    public void SetEnemiesAmountText()
    {
        if (_regularEnemiesRemaining > 0)
        {
            _enemiesAmount.text = $"{_regularEnemiesRemaining} enemies left.";
        }
        else if (_regularEnemiesRemaining == 0 && _bossesRemaining > 0)
        {
            _enemiesAmount.text = $"{_bossesRemaining} bosses left.";
        }
        else if (_regularEnemiesRemaining == 0 && _bossesRemaining == 0)
        {
            _enemiesAmount.text = "Level done! Go to the portal.";
        }
    }

    public void RegularEnemyDefeated()
    {
        _regularEnemiesRemaining--;
        SetEnemiesAmountText();
        if (_regularEnemiesRemaining <= 0)
        {
            ActivateBosses();
        }
    }

    private void ActivateBosses()
    {
        foreach (var boss in _bosses)
        {
            if (boss != null)
            {
                boss.gameObject.SetActive(true);
            }
        }
        SetEnemiesAmountText();
    }

    public void BossDefeated()
    {
        _bossesRemaining--;
        if (_bossesRemaining <= 0)
        {
            SpawnPortal();
        }
        SetEnemiesAmountText();
    }

    public void ResetGame()
    {
        _currentLevelIndex = -1;
        ServiceLocator.Instance.GetService<IScoreManager>().ResetScore();
        var player = ServiceLocator.Instance.GetService<IPlayerController>().PlayerTransform.gameObject;
        player.GetComponent<IHealth>().Heal(100);

        LoadNextLevel();
    }
}
