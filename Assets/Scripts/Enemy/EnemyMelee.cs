using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackForce = 15f;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;

    [Header("Visual Debug")]
    [SerializeField] private bool showAttackRange = false;
    [SerializeField] private Color gizmoColor = Color.yellow;

    private Enemy enemy;
    private PlayerController player;
    private EnemyAnimation anim;
    private float attackTimer;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        anim = GetComponent<EnemyAnimation>();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (player == null || player.IsDead()) return;

        attackTimer -= Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist <= attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        attackTimer = attackCooldown;

        // Reproducir animación de ataque
        if (anim != null)
            anim.PlayAttackAnimation();

        // Sonido de ataque
        if (attackSound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(attackSound);

        // Aplicar daño
        player.TakeDamage(damage);

        // Calcular dirección del knockback (del enemigo hacia el jugador)
        Vector2 dir = (player.transform.position - transform.position).normalized;
        Vector2 knockback = dir * knockbackForce;
        
        // Aplicar knockback al jugador
        player.ApplyKnockback(knockback);

        Debug.Log($"Enemigo golpeó al jugador! Daño: {damage}, Knockback: {knockback.magnitude}");
    }

    // Métodos públicos para configurar stats
    public void SetDamage(int dmg) => damage = dmg;
    public void SetKnockback(float kb) => knockbackForce = kb;
    public void SetCooldown(float cd) => attackCooldown = cd;

    // Debug visual
    private void OnDrawGizmosSelected()
    {
        if (!showAttackRange) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}