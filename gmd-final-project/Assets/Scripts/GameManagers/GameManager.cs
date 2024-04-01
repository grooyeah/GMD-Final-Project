using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Button restartButton;
    public Button gambleButton;
    public Button headsButton;
    public Button tailsButton;
    public TextMeshProUGUI gambleResultText;
    public GameObject gambleComponent;
    public GameObject player;
    public HealthBarManager healthBarManager;

    private string playerChoice;
    private bool win;
    public float pushBackRadius = 5f;
    public float pushBackForce = 20f;

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

    private void Start()
    {
        restartButton.onClick.AddListener(() =>
        {
            RestartLevel();
            SoundManager.Instance.PlayUIClickSound();
        });

        gambleButton.onClick.AddListener(() => 
        {
            ShowChoiceButtons();
            SoundManager.Instance.PlayUIClickSound();
        });
        headsButton.onClick.AddListener(() =>
        { 
            SetPlayerChoice("Heads"); 
            SoundManager.Instance.PlayUIClickSound(); 
        });
        tailsButton.onClick.AddListener(() =>
        {
            SetPlayerChoice("Tails");
            SoundManager.Instance.PlayUIClickSound();
        });

        restartButton.gameObject.SetActive(false);
        gambleButton.gameObject.SetActive(false);
        gambleComponent.SetActive(false);
    }

    public void PlayerDied()
    {
        healthBarManager.HideHealthBars();
        restartButton.gameObject.SetActive(true);
        gambleButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void ShowChoiceButtons()
    {
        gambleButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        gambleComponent.SetActive(true);
    }

    private void SetPlayerChoice(string choice)
    {
        playerChoice = choice;
        headsButton.gameObject.SetActive(false);
        tailsButton.gameObject.SetActive(false);
        SoundManager.Instance.PlayGambleDrumrollSound();
        StartCoroutine(AnimateGamble());
    }

    private IEnumerator AnimateGamble()
    {
        float elapsedTime = 0f;
        float animationDuration = 3f;
        string[] options = { "Heads", "Tails" };

        while (elapsedTime < animationDuration)
        {
            gambleResultText.text = options[Random.Range(0, options.Length)];
            elapsedTime += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);

        }

        win = (Random.value > 0.5f);
        string result = win ? "Heads" : "Tails";
        gambleResultText.text = result;

        if (playerChoice == result)
        {
            SoundManager.Instance.PlayGambleWinSound();
            gambleResultText.text += "\nYou won! Continue playing.";
            Time.timeScale = 1;
            Invoke(nameof(ContinueGame), 1f);
        }
        else
        {
            SoundManager.Instance.PlayGambleLoseSound();
            gambleResultText.text += "\nYou lost! Restarting...";
            Time.timeScale = 1;
            Invoke(nameof(RestartLevel), 1f);
        }
    }

    private void ContinueGame()
    {
        ResetGambleComponent();
        PushBackEnemies();

        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.Heal(50);
        }
    }

    private void PushBackEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, pushBackRadius);
        foreach (Collider2D enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                Vector2 pushDirection = (enemyCollider.transform.position - player.transform.position).normalized;
                var enemyMovement = enemyCollider.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.ApplyPushBack(pushDirection, pushBackForce);
                }
            }
        }
    }

    public void RestartLevel()
    {
        restartButton.gameObject.SetActive(false);
        gambleButton.gameObject.SetActive(false);
        ResetGambleComponent();
        Time.timeScale = 1;
        LevelManager.Instance.RestartLevel();
    }

    private void ResetGambleComponent()
    {
        gambleComponent.SetActive(false);
        gambleResultText.text = "Pick your choice";
        headsButton.gameObject.SetActive(true);
        tailsButton.gameObject.SetActive(true);
    }
}
