using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDebugger : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("=== BUSCANDO ELEMENTOS UI ===");
        
        GameObject coinsText = GameObject.Find("CoinsText");
        Debug.Log($"CoinsText encontrado: {coinsText != null}");
        if (coinsText != null)
        {
            Debug.Log($"CoinsText tiene TMP: {coinsText.GetComponent<TextMeshProUGUI>() != null}");
            Debug.Log($"CoinsText texto actual: {coinsText.GetComponent<TextMeshProUGUI>()?.text}");
        }
        
        GameObject staminaText = GameObject.Find("StaminaText");
        Debug.Log($"StaminaText encontrado: {staminaText != null}");
        if (staminaText != null)
        {
            Debug.Log($"StaminaText tiene TMP: {staminaText.GetComponent<TextMeshProUGUI>() != null}");
            Debug.Log($"StaminaText texto actual: {staminaText.GetComponent<TextMeshProUGUI>()?.text}");
        }
        
        GameObject staminaBar = GameObject.Find("StaminaBar");
        Debug.Log($"StaminaBar encontrado: {staminaBar != null}");
        if (staminaBar != null)
        {
            Debug.Log($"StaminaBar tiene Image: {staminaBar.GetComponent<Image>() != null}");
        }
        
        Debug.Log("=== VERIFICANDO MANAGERS ===");
        Debug.Log($"CurrencyManager existe: {CurrencyManager.Instance != null}");
        Debug.Log($"StaminaManager existe: {StaminaManager.Instance != null}");
        
        if (CurrencyManager.Instance != null)
        {
            Debug.Log($"Monedas totales: {CurrencyManager.Instance.GetTotalCoins()}");
        }
        
        if (StaminaManager.Instance != null)
        {
            Debug.Log($"Stamina actual: {StaminaManager.Instance.GetCurrentStamina()}");
            Debug.Log($"Stamina máxima: {StaminaManager.Instance.GetMaxStamina()}");
        }
    }
}