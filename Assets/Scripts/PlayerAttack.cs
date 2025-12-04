using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Vector2 attackSize = new Vector2(1.5f, 1.5f);
    [SerializeField] public int damage = 10;
    [SerializeField] private float knockbackIntensity = 10f;
    [SerializeField] private float attackCooldown = 0.5f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip swingAudio;

    [Header("Visual Debug")]
    [SerializeField] private bool showAttackRange = true;
    [SerializeField] private Color gizmoColor = Color.red;

    private PlayerController playerController;
    private float attackTimer = 0f;
    private Vector2 lastMoveDirection = Vector2.down;

    public event Action OnPlayerAttack;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Actualizar timer
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;

        // Leer dirección de movimiento
        UpdateMoveDirection();

        // Detectar input de ataque (Space, Click izquierdo, o botón que uses)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void UpdateMoveDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(h, v);

        if (input.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = input.normalized;
        }
    }

    private void TryAttack()
    {
        // Si está en cooldown o muerto, no atacar
        if (attackTimer > 0f || playerController.IsDead())
            return;

        // Iniciar cooldown
        attackTimer = attackCooldown;

        // Evento para animaciones
        OnPlayerAttack?.Invoke();

        // Sonido de swing
        if (swingAudio != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(swingAudio);

        // Realizar ataque
        PerformAttack();
    }

    private void PerformAttack()
    {
        // Calcular posición del ataque (delante del jugador)
        Vector2 attackPos = (Vector2)transform.position + lastMoveDirection * (attackSize.x / 2f);

        // Calcular ángulo de rotación del box
        float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;

        // Detectar enemigos en el área de ataque
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, attackSize, angle);

        bool hitSomething = false;

        foreach (Collider2D hit in hits)
        {
            // Ignorar al propio jugador
            if (hit.gameObject == gameObject)
                continue;

            // Intentar dañar enemigos con el componente Enemy
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Aplicar daño
                enemy.TakeDamage(damage);

                // Aplicar knockback
                Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;
                enemy.ApplyKnockback(knockbackDir * knockbackIntensity, 0.2f);

                hitSomething = true;
            }
        }

        // Sonido de impacto si golpeaste algo
        if (hitSomething && hitAudio != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(hitAudio);
        }
    }

    // Métodos públicos para upgrades/stats
    public int GetDamage() => damage;
    public void SetDamage(int dmg) => damage = dmg;
    public float GetKnockback() => knockbackIntensity;
    public void SetKnockback(float kb) => knockbackIntensity = kb;
    public float GetCooldown() => attackCooldown;
    public void SetCooldown(float cd) => attackCooldown = cd;

    // Debug visual en el editor
    private void OnDrawGizmos()
    {
        if (!showAttackRange) return;

        Vector2 attackPos = (Vector2)transform.position + lastMoveDirection * (attackSize.x / 2f);
        
        Gizmos.color = gizmoColor;
        
        // Dibujar el área de ataque
        Gizmos.matrix = Matrix4x4.TRS(attackPos, Quaternion.Euler(0, 0, Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackSize);
    }
}