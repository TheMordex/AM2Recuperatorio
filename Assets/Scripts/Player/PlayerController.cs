using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private float baseSpeed;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.25f;
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    private Vector2 knockbackVelocity;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityTime = 0.5f;
    private float invulnerabilityTimer = 0f;
    private bool isInvulnerable = false;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private FloatingJoystick joystick;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        currentHealth = maxHealth;
        baseSpeed = moveSpeed;
        
        Debug.Log($"✅ PlayerController inicializado - Vida: {currentHealth}/{maxHealth}");
    }
    
    private void Start()
    {
        // Buscar el joystick
        joystick = FindObjectOfType<FloatingJoystick>();
        if (joystick == null)
            Debug.LogError("❌ FloatingJoystick no encontrado en la escena");
        
        // Aplicar mejoras guardadas
        if (UpgradeDataManager.Instance != null)
        {
            maxHealth += UpgradeDataManager.Instance.GetCurrentMaxHealthBonus();
            currentHealth = maxHealth;
            Debug.Log($"💪 Vida máxima aumentada: {maxHealth}");
        }
    }

    private void Update()
    {
        if (isDead) return;

        // Leer input del joystick
        if (joystick != null)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        else
        {
            // Fallback a input de teclado si no hay joystick
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        }
        
        moveInput.Normalize();

        if (invulnerabilityTimer > 0f)
            invulnerabilityTimer -= Time.deltaTime;

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

        if (isKnockback)
        {
            knockbackVelocity = Vector2.Lerp(knockbackVelocity, Vector2.zero, 10f * Time.fixedDeltaTime);
            rb.velocity = knockbackVelocity;
            return;
        }

        rb.velocity = moveInput * moveSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || invulnerabilityTimer > 0f || isInvulnerable) return;

        currentHealth -= damage;
        invulnerabilityTimer = invulnerabilityTime;

        Debug.Log($"🗡️ Player tomó {damage} de daño. Vida: {currentHealth}/{maxHealth}");

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
        if (isDead) return;
        
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"💚 Vida: {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        rb.velocity = Vector2.zero;
        
        Debug.Log("💀 Player murió!");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndLevel(false);
        }
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
    
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    
    public void ApplySpeedBoost(float boost, float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(boost, duration));
    }
    
    private IEnumerator SpeedBoostCoroutine(float boost, float duration)
    {
        float boostedSpeed = baseSpeed + boost;
        moveSpeed = boostedSpeed;
        Debug.Log($"⚡ Velocidad: {baseSpeed} → {boostedSpeed}");
        
        yield return new WaitForSeconds(duration);
        
        moveSpeed = baseSpeed;
        Debug.Log($"⚡ Velocidad restaurada a: {baseSpeed}");
    }
    
    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }
    
    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    public Vector2 GetMoveInput() => moveInput;

    public bool IsDead() => isDead;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}