using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float angle = 0f;        // Dirección en grados (inicial)
    [SerializeField] private float amplitude = 0.5f;   // Altura del zigzag
    [SerializeField] private float frequency = 3f;     // Frecuencia del zigzag
    [SerializeField] private float lifetime = 3f;      // Duración antes de desaparecer

    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackAmount = 12f;
    [SerializeField] private AudioClip hitSound;

    private Rigidbody2D rb;
    private float startTime;
    private Vector3 startPosition;
    private Vector2 lastPos;
    private Vector2 currentDirection;

    private bool angleSetExternally = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        // es kinematic para controlar movimiento desde script
        rb.isKinematic = true;
    }

    private void Start()
    {
        startTime = Time.time;
        startPosition = transform.position;
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        // eliminar si pasa su lifetime
        if (Time.time - startTime > lifetime)
        {
            Destroy(gameObject);
            return;
        }

        float t = (Time.time - startTime);

        // zigzag con curva senoidal
        float yOffset = Mathf.Sin(t * frequency) * amplitude;

        // desplazamiento lineal + zigzag (x crece con t)
        Vector2 pos = new Vector2(t * frequency, yOffset);

        // giramos segun el ángulo
        pos = Rotate(pos, angle);

        // movemos el proyectil desde la posición original
        rb.MovePosition(startPosition + (Vector3)pos);

        // calcular dirección actual (para knockback)
        Vector2 newDir = ((Vector2)transform.position - lastPos);
        if (newDir.sqrMagnitude > 0.0001f)
            currentDirection = newDir.normalized;

        lastPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Si tocamos al PlayerController actual
        PlayerController player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            // Aplicar daño al jugador
            player.TakeDamage(damage);

            // Calcular dirección del knockback
            Vector2 kbDir = currentDirection;
            if (kbDir.sqrMagnitude < 0.0001f)
                kbDir = (player.transform.position - transform.position).normalized;

            // USAR EL MÉTODO ApplyKnockback del PlayerController
            Vector2 knockbackForce = kbDir * knockbackAmount;
            player.ApplyKnockback(knockbackForce);

            // reproducir sonido
            if (hitSound != null && AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(hitSound);

            Debug.Log($"Fireball impactó al jugador! Daño: {damage}, Knockback: {knockbackForce.magnitude}");

            Destroy(gameObject);
            return;
        }

        // Si toca muro / layer 8 → destruir (ajustalo al layer que uses para obstáculos)
        if (col.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

    // ---- ROTACIÓN SIMPLE EN 2D ----
    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }

    // --- Método público para configurar el ángulo desde fuera (por EnemyMage) ---
    public void SetAngle(float degrees)
    {
        angle = degrees;
        angleSetExternally = true;

        // También rotamos visualmente el sprite (opcional):
        // transform.rotation = Quaternion.Euler(0, 0, degrees);
    }

    // También exposición opcional para configurar daño/knockback en runtime
    public void SetDamage(int d) => damage = d;
    public void SetKnockback(float kb) => knockbackAmount = kb;
}