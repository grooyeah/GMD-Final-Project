using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreManager
{
    public static IScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _score;

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
        _score = 0;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        _score += amount;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        _score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = "Score: " + _score;
    }
}
