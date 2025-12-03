using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Enemy magePrefab;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private float minDistanceFromPlayer = 5f;

    [Header("Mage Spawn Settings")]
    [SerializeField] private float mageSpawnChance = 0.10f;
    [SerializeField] private float chanceIncreasePerWave = 0.05f;
    [SerializeField] private int maxMagesPerWave = 1;

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController player;

    private ObjectPool<Enemy> commonPool;
    private ObjectPool<Enemy> magePool;

    private int activeEnemies = 0;
    private int magesSpawnedThisWave = 0;
    private bool hasMagePrefab = false;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        
        if (player == null)
            player = FindObjectOfType<PlayerController>();
        
        if (enemyPrefab == null)
            Debug.LogError("EnemySpawner: No hay enemyPrefab asignado!");

        hasMagePrefab = magePrefab != null;
        
        // Crear pools en Awake
        if (enemyPrefab != null)
        {
            commonPool = new ObjectPool<Enemy>(
                createFunc: () => Instantiate(enemyPrefab),
                actionOnGet: e => e.gameObject.SetActive(true),
                actionOnRelease: e => ResetEnemy(e),
                actionOnDestroy: e => Destroy(e.gameObject),
                maxSize: 100
            );
        }

        if (hasMagePrefab)
        {
            magePool = new ObjectPool<Enemy>(
                createFunc: () => Instantiate(magePrefab),
                actionOnGet: e => e.gameObject.SetActive(true),
                actionOnRelease: e => ResetEnemy(e),
                actionOnDestroy: e => Destroy(e.gameObject),
                maxSize: 50
            );
        }
    }

    public void SpawnWave(int enemyCount, int waveNumber)
    {
        magesSpawnedThisWave = 0;

        if (hasMagePrefab)
        {
            mageSpawnChance += chanceIncreasePerWave;
            mageSpawnChance = Mathf.Clamp01(mageSpawnChance);
        }

        for (int i = 0; i < enemyCount; i++)
            SpawnSingleEnemy(waveNumber);
    }

    private void SpawnSingleEnemy(int waveNumber)
    {
        if (player == null)
        {
            Debug.LogError("EnemySpawner: Player es null, no se puede spawnear");
            return;
        }

        Vector3 spawnPos = GetRandomSpawnPosition();

        int attempts = 0;
        while (Vector3.Distance(spawnPos, player.transform.position) < minDistanceFromPlayer && attempts < 10)
        {
            spawnPos = GetRandomSpawnPosition();
            attempts++;
        }

        bool canSpawnMage = hasMagePrefab && magesSpawnedThisWave < maxMagesPerWave;
        bool spawnMage = canSpawnMage && Random.value < mageSpawnChance;

        Enemy e;

        if (spawnMage)
        {
            e = magePool.Get();
            magesSpawnedThisWave++;
        }
        else
        {
            e = commonPool.Get();
        }

        e.Initialize(spawnPos, player, this, waveNumber);
        activeEnemies++;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnArea == null)
            return transform.position;

        Vector2 dir = Random.insideUnitCircle.normalized;
        return spawnArea.position + (Vector3)(dir * spawnRadius);
    }

    public void ReturnEnemyToPool(Enemy e)
    {
        if (e == null) return;

        activeEnemies--;
        gameManager.OnEnemyDefeated(e.GetCoinReward());

        if (hasMagePrefab && e.CompareTag("Mage"))
            magePool.Release(e);
        else
            commonPool.Release(e);
    }

    private void ResetEnemy(Enemy e)
    {
        Rigidbody2D rb = e.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

        e.gameObject.SetActive(false);
    }

    public int GetActiveEnemyCount() => activeEnemies;

    private void OnDrawGizmosSelected()
    {
        if (spawnArea == null) return;

        Gizmos.color = Color.cyan;
        DrawCircle(spawnArea.position, spawnRadius, 32);

        if (player != null)
        {
            Gizmos.color = Color.red;
            DrawCircle(player.transform.position, minDistanceFromPlayer, 16);
        }
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}