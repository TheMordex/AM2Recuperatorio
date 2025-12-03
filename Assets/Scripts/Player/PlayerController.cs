using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.25f;  // Aumentado
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    private Vector2 knockbackVelocity;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityTime = 0.5f;
    private float invulnerabilityTimer = 0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead) return;

        // Leer input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // Timer de invulnerabilidad
        if (invulnerabilityTimer > 0f)
            invulnerabilityTimer -= Time.deltaTime;

        // Timer de knockback
        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockback = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        // Si está en knockback, aplicar velocidad de knockback con decay
        if (isKnockback)
        {
            // Decrecer el knockback gradualmente
            knockbackVelocity = Vector2.Lerp(knockbackVelocity, Vector2.zero, 10f * Time.fixedDeltaTime);
            rb.velocity = knockbackVelocity;
            return;
        }

        // Movimiento normal
        rb.velocity = moveInput * moveSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || invulnerabilityTimer > 0f) return;

        currentHealth -= damage;
        invulnerabilityTimer = invulnerabilityTime;

        Debug.Log($"Player tomó {damage} de daño. Vida: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (isDead) return;

        isKnockback = true;
        knockbackVelocity = force;
        knockbackTimer = knockbackDuration;
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"Vida: {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        Debug.Log("Player murió!");
    }

    public bool IsDead() => isDead;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}