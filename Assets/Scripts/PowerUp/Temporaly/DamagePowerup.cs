using UnityEngine;
using System.Collections;

public class DamagePowerup : BasePowerup
{
    [Header("Powerup UI")]
    [SerializeField] private Sprite icon;

    [Header("Damage Boost")]
    [SerializeField] private int damageBoost = 10;
    
    protected override void ApplyEffect(GameObject player)
    {
        
        PowerupUIManager.Instance.AddPowerupIcon("Damage", icon, effectDuration);

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

        yield return new WaitForSeconds(effectDuration);
        
        if (playerAttack != null)
        {
            playerAttack.SetDamage(originalDamage);
        }
    }
}
