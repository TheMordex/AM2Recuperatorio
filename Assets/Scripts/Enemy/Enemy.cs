using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int baseHealth = 5;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private int coinReward = 1;
    [SerializeField] private GameObject coinPrefab;

    [Header("Knockback")]
    private bool isKnockback;
    private float knockbackTimer;
    private Vector2 knockbackForce;

    private int currentHealth;
    private PlayerController player;
    private EnemySpawner spawner;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();
        
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void Initialize(Vector3 pos, PlayerController playerController, EnemySpawner spawnerRef, int wave)
    {
        transform.position = pos;
        player = playerController;
        spawner = spawnerRef;
        currentHealth = baseHealth + (wave * 2);
        isKnockback = false;
        rb.velocity = Vector2.zero;

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        // Inicializar IA
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null)
            ai.Initialize(playerController);

        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (player == null || player.IsDead())
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isKnockback)
        {
            rb.velocity = knockbackForce;
            knockbackTimer -= Time.fixedDeltaTime;

            if (knockbackTimer <= 0f)
                isKnockback = false;

            return;
        }

        Vector2 dirToPlayer = (player.transform.position - transform.position).normalized;
        rb.velocity = dirToPlayer * moveSpeed;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    public void ApplyKnockback(Vector2 force, float duration = 0.12f)
    {
        isKnockback = true;
        knockbackForce = force;
        knockbackTimer = duration;
        rb.velocity = Vector2.zero;
    }

    private void Die()
    {
        // Soltar loot
        LootDrop lootDrop = GetComponent<LootDrop>();
        if (lootDrop != null)
            lootDrop.DropLoot();
        
        // Soltar monedas
        int coinDrop = Random.Range(1, 4);
        for (int i = 0; i < coinDrop; i++)
        {
            if (coinPrefab != null)
            {
                GameObject coinObj = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Coin coin = coinObj.GetComponent<Coin>();
                if (coin != null)
                    coin.ApplyImpulse(Random.insideUnitCircle.normalized * 5f);
            }
        }

        rb.velocity = Vector2.zero;
        spawner.ReturnEnemyToPool(this);
    }

    public int GetCoinReward() => coinReward;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => baseHealth;
}