using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }

    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _helpPanel;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _helpCloseButton;
    [SerializeField] private Button _startExitButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _endExitButton;

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
        _startScreen.SetActive(true);
        _helpPanel.SetActive(false);
        _endScreen.SetActive(false);

        Time.timeScale = 0;

        _startButton.onClick.AddListener(() =>
        {
            StartGame();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        _helpButton.onClick.AddListener(() =>
        {
            ShowHelp();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        _helpCloseButton.onClick.AddListener(() =>
        {
            CloseHelp();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        _startExitButton.onClick.AddListener(() =>
        {
            QuitGame();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        _restartButton.onClick.AddListener(() =>
        {
            RestartGame();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });

        _endExitButton.onClick.AddListener(() =>
        {
            QuitGame();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayUIClickSound();
        });
    }

    public void StartGame()
    {
        _startScreen.SetActive(false);

        Time.timeScale = 1;
    }

    public void ShowHelp()
    {
        _helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        _helpPanel.SetActive(false);
    }

    public void EndGame()
    {
        _endScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        _endScreen.SetActive(false);

        Time.timeScale = 1;

        ServiceLocator.Instance.GetService<ILevelManager>().ResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
