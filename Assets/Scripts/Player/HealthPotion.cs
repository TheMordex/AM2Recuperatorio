using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 20;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private float collectAnimationDuration = 0.3f;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Collider2D potionCollider;
    private bool picked = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        potionCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void HandleCollision(GameObject collidedObject)
    {
        if (!collidedObject.CompareTag("Player") || picked) return;

        picked = true;

        // Desactivar collider
        if (potionCollider != null)
            potionCollider.enabled = false;

        // Curar al jugador
        PlayerController player = collidedObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log($"🧪 Poción curó {healAmount} HP");
        }

        // Reproducir sonido
        if (audioSource != null && healSound != null)
            audioSource.PlayOneShot(healSound);
        
        AnimateCollection();

        // Destruir después de animación + sonido
        float delay = Mathf.Max(
            collectAnimationDuration,
            healSound != null ? healSound.length : 0f
        );

        Destroy(gameObject, delay + 0.1f);
    }

    private void AnimateCollection()
    {
        if (spriteRenderer == null) return;

        // Escalar
        LeanTween.scale(gameObject, Vector3.one * 1.5f, collectAnimationDuration)
            .setEaseOutQuad();

        // Fade-out
        LeanTween.alpha(gameObject, 0f, collectAnimationDuration)
            .setEaseOutQuad();

        // Movimiento hacia arriba
        Vector3 targetPos = transform.position + Vector3.up * 0.5f;
        LeanTween.move(gameObject, targetPos, collectAnimationDuration)
            .setEaseOutQuad();
    }

    public void ApplyImpulse(Vector2 force)
    {
        Movement movement = GetComponent<Movement>();
        if (movement != null)
            movement.Impulse(force);
    }
}
