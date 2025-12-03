using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private PlayerController player;
    
    private int maxHealth;
    
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player != null)
            maxHealth = player.GetMaxHealth();
    }
    
    private void Update()
    {
        if (player == null) return;
        
        if (maxHealth == 0)
            maxHealth = player.GetMaxHealth();
        
        int currentHealth = player.GetCurrentHealth();
        float fillAmount = (float)currentHealth / maxHealth;
        
        healthBar.fillAmount = fillAmount;
        
        // Cambiar color según salud
        if (fillAmount > 0.5f)
            healthBar.color = Color.green;
        else if (fillAmount > 0.25f)
            healthBar.color = Color.yellow;
        else
            healthBar.color = Color.red;
    }
}