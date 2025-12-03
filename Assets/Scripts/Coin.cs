using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private AudioClip coinSound;
    
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool picked = false;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        if (coinSound != null)
        {
            audioSource.clip = coinSound;
            audioSource.pitch = 0.9f + Random.Range(0f, 0.2f);
            audioSource.volume = 0.4f + Random.Range(0f, 0.2f);
        }
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
            
            // Sumar moneda al GameManager
            GameManager.Instance.AddCoins(value);
            
            // Reproducir sonido
            if (audioSource != null && coinSound != null)
            {
                audioSource.Play();
                Destroy(gameObject, coinSound.length + 0.1f);
            }
            else
            {
                Destroy(gameObject);
            }
            
            // Hacer invisible
            if (spriteRenderer != null)
                spriteRenderer.color = Color.clear;
        }
    }
    
    public void ApplyImpulse(Vector2 force)
    {
        if (rb != null)
            rb.velocity = force;
    }
}