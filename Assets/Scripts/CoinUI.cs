using UnityEngine;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    
    private void Update()
    {
        if (coinsText != null)
            coinsText.text = ": " + GameManager.Instance.GetCoinsEarned().ToString();
    }
}