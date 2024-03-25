using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _lifeText;

    private void Awake()
    {
        if(_lifeText != null)
        {
            _lifeText.text = "❤❤❤";
        }
    }
}
