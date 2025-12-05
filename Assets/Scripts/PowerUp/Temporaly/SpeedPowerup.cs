using UnityEngine;

public class SpeedPowerup : BasePowerup
{
    [Header("Powerup UI")]
    [SerializeField] private Sprite icon; 

    [Header("Speed Boost")]
    [SerializeField] private float speedBoost = 3f;
    
    protected override void ApplyEffect(GameObject player)
    {
        PowerupUIManager.Instance.AddPowerupIcon("Speed", icon, effectDuration);

        PlayerController playerController = player.GetComponent<PlayerController>();
        
        if (playerController != null)
        {
            playerController.ApplySpeedBoost(speedBoost, effectDuration);
        }
    }
}
