using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] public Button _restartButton;
    [SerializeField] public Button _gambleButton;
    [SerializeField] public Button _headsButton;
    [SerializeField] public Button _tailsButton;
    [SerializeField] public TextMeshProUGUI _gambleResultText;
    [SerializeField] public GameObject _gambleComponent;
    [SerializeField] public GameObject _player;

    [Header("Values")]
    public float _pushBackRadius = 5f;
    public float _pushBackForce = 20f;
    
    private IPlayerHealthBarManager _playerHealthBarManager;
    private IEnemyHealthBarManager _enemyHealthBarManager;
    private IScoreManager _scoreManager;

    private string _playerChoice;
    private bool _win;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            RegisterServices();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _restartButton.onClick.AddListener(() =>
        {
            RestartLevel();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        _gambleButton.onClick.AddListener(() =>
        {
            ShowChoiceButtons();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        _headsButton.onClick.AddListener(() =>
        {
            SetPlayerChoice("Heads");
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        _tailsButton.onClick.AddListener(() =>
        {
            SetPlayerChoice("Tails");
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        DeactivateButtons();
        ResetGambleComponent();
    }

    private void RegisterServices()
    {
        ServiceLocator.Instance.RegisterService<IPlayerController>(FindObjectOfType<PlayerController>());
        ServiceLocator.Instance.RegisterService<ISoundManager>(FindObjectOfType<SoundManager>());
        ServiceLocator.Instance.RegisterService<IScoreManager>(FindObjectOfType<ScoreManager>());
        ServiceLocator.Instance.RegisterService<ILevelManager>(FindObjectOfType<LevelManager>());
        ServiceLocator.Instance.RegisterService<IEnemyHealthBarManager>(FindObjectOfType<EnemyHealthBarManager>());
        ServiceLocator.Instance.RegisterService<IPlayerHealthBarManager>(FindObjectOfType<PlayerHealthBarManager>());

        _enemyHealthBarManager = ServiceLocator.Instance.GetService<IEnemyHealthBarManager>();
        _scoreManager = ServiceLocator.Instance.GetService<IScoreManager>();
    }

    public void PlayerDied()
    {
        _enemyHealthBarManager.HideHealthBars();
        _restartButton.gameObject.SetActive(true);
        _gambleButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void ShowChoiceButtons()
    {
        DeactivateButtons();
        _gambleComponent.SetActive(true);
    }

    private void SetPlayerChoice(string choice)
    {
        _playerChoice = choice;
        _headsButton.gameObject.SetActive(false);
        _tailsButton.gameObject.SetActive(false);
        ServiceLocator.Instance.GetService<ISoundManager>().PlayGambleDrumrollSound();
        StartCoroutine(AnimateGamble());
    }

    private IEnumerator AnimateGamble()
    {
        float elapsedTime = 0f;
        float animationDuration = 3f;
        string[] options = { "Heads", "Tails" };

        while (elapsedTime < animationDuration)
        {
            _gambleResultText.text = options[Random.Range(0, options.Length)];
            elapsedTime += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        _win = (Random.value > 0.5f);
        string result = _win ? "Heads" : "Tails";
        _gambleResultText.text = result;

        if (_playerChoice == result)
        {
            ServiceLocator.Instance.GetService<ISoundManager>().PlayGambleWinSound();
            _gambleResultText.text += "\nYou won! Continue playing.";
            Time.timeScale = 1;
            Invoke(nameof(ContinueGame), 1f);
        }
        else
        {
            ServiceLocator.Instance.GetService<ISoundManager>().PlayGambleLoseSound();
            _gambleResultText.text += "\nYou lost! Restarting...";
            Time.timeScale = 1;
            Invoke(nameof(RestartLevel), 1f);
        }
    }

    private void ContinueGame()
    {
        DeactivateButtons();
        ResetGambleComponent();
        PushBackEnemies();

        var playerHealth = _player.GetComponent<IHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(50);
        }
    }

    private void PushBackEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_player.transform.position, _pushBackRadius);
        foreach (Collider2D enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                Vector2 pushDirection = (enemyCollider.transform.position - _player.transform.position).normalized;
                var enemyMovement = enemyCollider.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.ApplyPushBack(pushDirection, _pushBackForce);
                }
            }
        }
    }

    public void RestartLevel()
    {
        _enemyHealthBarManager.ClearHealthBars();
        DeactivateButtons();
        ResetGambleComponent();
        Time.timeScale = 1;
        ServiceLocator.Instance.GetService<ILevelManager>().RestartLevel();
        _scoreManager.ResetScore();
    }

    private void DeactivateButtons()
    {
        _restartButton.gameObject.SetActive(false);
        _gambleButton.gameObject.SetActive(false);
    }

    private void ResetGambleComponent()
    {
        _gambleComponent.SetActive(false);
        _gambleResultText.text = "Pick your choice";
        _headsButton.gameObject.SetActive(true);
        _tailsButton.gameObject.SetActive(true);
    }
}

