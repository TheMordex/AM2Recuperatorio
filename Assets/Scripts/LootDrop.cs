using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class LootItem
{
    public GameObject prefab;
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
                GameObject lootObj = Instantiate(item.prefab);
                lootObj.transform.position = transform.position;
                lootObj.GetComponent<Movement>().Impulse(Random.insideUnitCircle * dropForce);
            }
        }
    }
}