using UnityEngine;

public class EnemyMage : MonoBehaviour
{
    [Header("Ranged Attack")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float shootRange = 5f;
    [SerializeField] private float shootCooldown = 1.3f;

    [Header("Melee Attack")]
    [SerializeField] private float meleeRange = 1.2f;
    [SerializeField] private int meleeDamage = 8;
    [SerializeField] private float meleeKnockback = 12f;

    [Header("Audio")]
    [SerializeField] private AudioClip castSound;
    [SerializeField] private AudioClip meleeSound;

    [Header("Visual Debug")]
    [SerializeField] private bool showRanges = false;
    [SerializeField] private Color meleeColor = Color.red;
    [SerializeField] private Color shootColor = Color.blue;

    private PlayerController player;
    private EnemyAnimation anim;
    private float shootTimer;
    private float meleeTimer;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<EnemyAnimation>();
    }

    private void Update()
    {
        if (player == null || player.IsDead())
            return;

        float dist = Vector2.Distance(transform.position, player.transform.position);
        shootTimer -= Time.deltaTime;
        meleeTimer -= Time.deltaTime;

        // Prioridad 1: Ataque melee si está muy cerca
        if (dist <= meleeRange && meleeTimer <= 0f)
        {
            DoMelee();
            return;
        }

        // Prioridad 2: Disparar si está en rango
        if (dist <= shootRange && shootTimer <= 0f)
        {
            CastFireball();
        }
    }

    private void CastFireball()
    {
        shootTimer = shootCooldown;

        // Animación de ataque
        if (anim) anim.PlayAttackAnimation();

        // Sonido de casteo
        if (castSound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(castSound);

        // Crear fireball
        GameObject obj = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        // Calcular dirección y ángulo
        Vector2 dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Configurar el proyectil
        FireballProjectile fb = obj.GetComponent<FireballProjectile>();
        if (fb != null)
        {
            fb.SetAngle(angle);
        }

        Debug.Log($"Mago lanzó fireball hacia el jugador. Ángulo: {angle}°");
    }

    private void DoMelee()
    {
        meleeTimer = shootCooldown; 

        // Animación de ataque melee
        if (anim) anim.PlayAttackAnimation();
        
        if (meleeSound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(meleeSound);

        // Aplicar daño
        player.TakeDamage(meleeDamage);

        // Calcular knockback
        Vector2 dir = (player.transform.position - transform.position).normalized;
        Vector2 knockback = dir * meleeKnockback;
        
        // Aplicar knockback al jugador
        player.ApplyKnockback(knockback);

        Debug.Log($"Mago golpeó al jugador en melee! Daño: {meleeDamage}, Knockback: {knockback.magnitude}");
    }

    // Métodos públicos para configurar stats
    public void SetMeleeDamage(int dmg) => meleeDamage = dmg;
    public void SetMeleeKnockback(float kb) => meleeKnockback = kb;
    public void SetShootCooldown(float cd) => shootCooldown = cd;

    // Debug visual
    private void OnDrawGizmosSelected()
    {
        if (!showRanges) return;

        // Rango de melee (rojo)
        Gizmos.color = meleeColor;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        // Rango de disparo (azul)
        Gizmos.color = shootColor;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}