using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        if (_scoreText != null)
        {
            _scoreText.text = "00000";
        }
    }
}
