using TMPro;
using UnityEngine;

public class LeveInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelTitle;

    [SerializeField] 
    private TextMeshProUGUI _levelDetails;


    private void Awake()
    {
        if(_levelTitle != null && _levelDetails != null)
        {
            _levelTitle.text = "Level 1";
            _levelDetails.text = "x enemies left";
        }
    }
}
