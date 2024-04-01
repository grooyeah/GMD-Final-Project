using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private GameObject[] levelPrefabs;
    private int currentLevelIndex = -1;
    private GameObject currentLevel;

    public GameObject portalPrefab;
    private GameObject portal;

    private Transform playerSpawnPoint;
    private Transform portalSpawnPoint;

    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI enemiesAmount;

    private int bossesRemaining = 0;
    private int regularEnemiesRemaining = 0;

    private ICollection<GameObject> bosses;

    private HealthBarManager healthBarManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            bosses = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }

        healthBarManager = FindObjectOfType<HealthBarManager>();
    }

    private void Start()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex >= 0 && currentLevel != null)
        {
            Destroy(currentLevel);
        }

        currentLevelIndex++;

        if (currentLevelIndex < levelPrefabs.Length)
        {
            currentLevel = Instantiate(levelPrefabs[currentLevelIndex]);
            FindSpawnPoints();
            PositionPlayerAtSpawnPoint();
            Destroy(portal);
            SetLevelNameText();

            // Count the number of regular enemies and bosses in this level
            var enemies = currentLevel.transform.Find($"Level {currentLevelIndex + 1} Enemies").GetComponentsInChildren<EnemyBehavior>();
            regularEnemiesRemaining = enemies.Count(e => e.CompareTag("Enemy"));
            bossesRemaining = enemies.Count(e => e.CompareTag("Level Boss"));

            // Disable bosses at the start
            foreach (var boss in enemies.Where(e => e.CompareTag("Level Boss")))
            {
                bosses.Add(boss.gameObject);
                boss.gameObject.SetActive(false);
            }
            SetEnemiesAmountText();

            StartCoroutine(InitializeHealthBars());
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    private IEnumerator InitializeHealthBars()
    {
        // Wait for end of frame to ensure all objects are instantiated and active
        yield return new WaitForEndOfFrame();
        healthBarManager.InstantiateHealthBars();
    }

    private void FindSpawnPoints()
    {
        playerSpawnPoint = currentLevel.transform.Find("Level " + (currentLevelIndex + 1) + " Spawnpoint");
        portalSpawnPoint = currentLevel.transform.Find("Level " + (currentLevelIndex + 1) + " Portal Spawnpoint");

        if (playerSpawnPoint == null || portalSpawnPoint == null)
        {
            Debug.LogError("Spawn points not found in the level prefab.");
        }
    }

    private void PositionPlayerAtSpawnPoint()
    {
        GameObject player = GameManager.Instance.player;
        player.transform.position = playerSpawnPoint.position;
    }

    public void SpawnPortal()
    {
        if (bossesRemaining == 0)
        {
            portal = Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);
            portal.transform.position = portalSpawnPoint.transform.position;
            StartCoroutine(ScalePortal());
        }
    }

    private IEnumerator ScalePortal()
    {
        float elapsedTime = 0f;
        float scalingDuration = 0.5f;
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
        SoundManager.Instance.PlayPortalOpenSound();
        while (elapsedTime < scalingDuration)
        {
            portal.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / scalingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        portal.transform.localScale = targetScale;
    }

    public void RestartLevel()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel);
        }
        GameObject player = GameManager.Instance.player;
        player.GetComponent<Health>().Heal(100);
        currentLevel = Instantiate(levelPrefabs[currentLevelIndex]);
        FindSpawnPoints();
        PositionPlayerAtSpawnPoint();
    }

    public void SetLevelNameText()
    {
        levelName.text = currentLevel.name.Replace("(Clone)", "");
    }

    public void SetEnemiesAmountText()
    {
        if (regularEnemiesRemaining > 0)
        {
            enemiesAmount.text = $"{regularEnemiesRemaining} enemies left.";
        }
        else if (regularEnemiesRemaining == 0 && bossesRemaining > 0)
        {
            enemiesAmount.text = $"{bossesRemaining} bosses left.";
        }
        else if (regularEnemiesRemaining == 0 && bossesRemaining == 0)
        {
            enemiesAmount.text = "Level done! Go to the portal.";
        }
    }

    public void RegularEnemyDefeated()
    {
        regularEnemiesRemaining--;
        SetEnemiesAmountText();
        if (regularEnemiesRemaining <= 0)
        {
            ActivateBosses();
        }
    }

    private void ActivateBosses()
    {
        foreach (var boss in bosses)
        {
            if (boss != null) // Ensure the boss object exists
            {
                boss.gameObject.SetActive(true);
            }
        }
        SetEnemiesAmountText();
    }

    public void BossDefeated()
    {
        bossesRemaining--;
        if (bossesRemaining <= 0)
        {
            SpawnPortal();
        }
        SetEnemiesAmountText();
    }
}
