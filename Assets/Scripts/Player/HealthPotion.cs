using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 20;
    [SerializeField] private AudioClip healSound;
    
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool picked = false;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
        if (collidedObject.CompareTag("Player") && !picked)
        {
            picked = true;
            
            // Curar al jugador
            PlayerController player = collidedObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Heal(healAmount);
                Debug.Log($"Poción curó {healAmount} HP");
            }
            
            // Sonido
            if (audioSource != null && healSound != null)
            {
                audioSource.PlayOneShot(healSound);
                Destroy(gameObject, healSound.length + 0.1f);
            }
            else
            {
                Destroy(gameObject);
            }
            
            // Desaparecer
            if (spriteRenderer != null)
                spriteRenderer.color = Color.clear;
        }
    }
    
    public void ApplyImpulse(Vector2 force)
    {
        Movement movement = GetComponent<Movement>();
        if (movement != null)
            movement.Impulse(force);
    }
}