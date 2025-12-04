using UnityEngine;
using System.Collections;

public class DamagePowerup : BasePowerup
{
    [Header("Damage Boost")]
    [SerializeField] private int damageBoost = 10;
    
    protected override void ApplyEffect(GameObject player)
    {
        PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
        
        if (playerAttack != null)
        {
            StartCoroutine(ApplyDamageBoost(playerAttack));
        }
    }
    
    private IEnumerator ApplyDamageBoost(PlayerAttack playerAttack)
    {
        int originalDamage = playerAttack.GetDamage();
        int boostedDamage = originalDamage + damageBoost;
        
        playerAttack.SetDamage(boostedDamage);
        Debug.Log($"💥 Power-up de daño activado! Daño: {originalDamage} → {boostedDamage} por {effectDuration}s");
        
        yield return new WaitForSeconds(effectDuration);
        
        if (playerAttack != null)
        {
            playerAttack.SetDamage(originalDamage);
            Debug.Log($"💥 Power-up de daño terminado. Daño restaurado a: {originalDamage}");
        }
    }
}