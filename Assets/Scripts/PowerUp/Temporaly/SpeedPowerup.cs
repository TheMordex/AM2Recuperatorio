using UnityEngine;

public class SpeedPowerup : BasePowerup
{
    [Header("Speed Boost")]
    [SerializeField] private float speedBoost = 3f;
    
    protected override void ApplyEffect(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        
        if (playerController != null)
        {
            // Usar el método del PlayerController para aplicar el boost
            playerController.ApplySpeedBoost(speedBoost, effectDuration);
            Debug.Log($"⚡ Power-up de velocidad activado! +{speedBoost} por {effectDuration}s");
        }
    }
}