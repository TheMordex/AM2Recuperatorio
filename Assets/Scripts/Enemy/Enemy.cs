using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int baseHealth = 5;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private int coinReward = 1;

    [Header("Swarm Behaviour")]
    [SerializeField] private float separationRadius = 1.0f;
    [SerializeField] private float separationStrength = 2.0f;
    [SerializeField] private float cohesionStrength = 0.4f;
    [SerializeField] private float alignmentStrength = 0.4f;

    [Header("Combat")]
    [SerializeField] private float invulnerabilityTime = 0.1f;
    [SerializeField] private Color damageFlashColor = Color.red;

    private int currentHealth;
    private PlayerController player;
    private EnemySpawner spawner;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Knockback
    private bool isKnockback;
    private float knockbackTimer;
    private Vector2 knockbackForce;

    // Invulnerability
    private float invulnerabilityTimer;
    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        invulnerabilityTimer = 0f;
        rb.velocity = Vector2.zero;

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        // Timer de invulnerabilidad
        if (invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.deltaTime;

            // Flash effect
            if (spriteRenderer != null)
            {
                float flash = Mathf.PingPong(Time.time * 10f, 1f);
                spriteRenderer.color = Color.Lerp(originalColor, damageFlashColor, flash);
            }
        }
        else if (spriteRenderer != null && spriteRenderer.color != originalColor)
        {
            spriteRenderer.color = originalColor;
        }
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

        // Comportamiento de swarm normal
        Vector2 dirToPlayer = (player.transform.position - transform.position).normalized;

        Vector2 separation = CalculateSeparation();
        Vector2 alignment = CalculateAlignment();
        Vector2 cohesion = CalculateCohesion(dirToPlayer);

        Vector2 finalDir =
            dirToPlayer +
            separation * separationStrength +
            alignment * alignmentStrength +
            cohesion * cohesionStrength;

        finalDir.Normalize();

        rb.velocity = finalDir * moveSpeed;
    }

    private Vector2 CalculateSeparation()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        Vector2 force = Vector2.zero;
        int count = 0;

        foreach (var h in hits)
        {
            if (h.gameObject == gameObject) continue;

            Enemy other = h.GetComponent<Enemy>();
            if (other == null) continue;

            Vector2 diff = (Vector2)(transform.position - other.transform.position);
            if (diff.sqrMagnitude > 0.001f)
            {
                force += diff.normalized / diff.magnitude;
                count++;
            }
        }

        if (count > 0)
            force /= count;

        return force;
    }

    private Vector2 CalculateAlignment()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, separationRadius * 1.5f);
        Vector2 avgVelocity = Vector2.zero;
        int count = 0;

        foreach (var h in hits)
        {
            if (h.gameObject == gameObject) continue;

            Enemy other = h.GetComponent<Enemy>();
            if (other == null) continue;

            avgVelocity += other.rb.velocity;
            count++;
        }

        if (count > 0)
            avgVelocity /= count;

        return avgVelocity.normalized;
    }

    private Vector2 CalculateCohesion(Vector2 dirToPlayer)
    {
        return dirToPlayer;
    }

    public void TakeDamage(int dmg)
    {
        // Si está en invulnerabilidad, ignorar daño
        if (invulnerabilityTimer > 0f)
            return;

        currentHealth -= dmg;
        invulnerabilityTimer = invulnerabilityTime;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        rb.velocity = Vector2.zero;
        spawner.ReturnEnemyToPool(this);
    }

    public void ApplyKnockback(Vector2 force, float duration = 0.12f)
    {
        isKnockback = true;
        knockbackForce = force;
        knockbackTimer = duration;
        rb.velocity = Vector2.zero;
    }

    public int GetCoinReward() => coinReward;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => baseHealth;
}