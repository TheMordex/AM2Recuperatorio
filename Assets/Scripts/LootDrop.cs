using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class LootItem
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float dropChance = 0.5f;
}

public class LootDrop : MonoBehaviour
{
    [SerializeField] private LootItem[] lootTable;
    [SerializeField] private float dropForce = 7f;
    
    public void DropLoot()
    {
        if (lootTable == null || lootTable.Length == 0) return;
        
        foreach (LootItem item in lootTable)
        {
            if (item.prefab == null) continue;
            
            if (Random.value < item.dropChance)
            {
                GameObject lootObj = Instantiate(item.prefab, transform.position, Quaternion.identity);
                
                BasePowerup powerup = lootObj.GetComponent<BasePowerup>();
                if (powerup != null)
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    powerup.ApplyImpulse(randomDirection * dropForce);
                }
                
                Coin coin = lootObj.GetComponent<Coin>();
                if (coin != null)
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    coin.ApplyImpulse(randomDirection * dropForce);
                }
                
                HealthPotion potion = lootObj.GetComponent<HealthPotion>();
                if (potion != null)
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    potion.ApplyImpulse(randomDirection * dropForce);
                }
            }
        }
    }
}