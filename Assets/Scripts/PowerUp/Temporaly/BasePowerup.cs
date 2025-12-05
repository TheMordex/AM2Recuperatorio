using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public abstract class BasePowerup : MonoBehaviour
{
    [Header("Powerup Settings")]
    [SerializeField] protected float effectDuration = 5f;
    [SerializeField] protected AudioClip pickupSound;
    
    [Header("UI")]
    [SerializeField] protected Sprite powerupIcon; 
    [SerializeField] protected string powerupType; 
    
    [Header("Animation")]
    [SerializeField] private float collectAnimationDuration = 0.3f;
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject feedbackPrefab;
    
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private Collider2D powerupCollider;
    private bool picked = false;
    
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        powerupCollider = GetComponent<Collider2D>();
        
        if (powerupIcon == null && spriteRenderer != null)
            powerupIcon = spriteRenderer.sprite;
        
        if (string.IsNullOrEmpty(powerupType))
            powerupType = GetType().Name;
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
        }
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.pitch = 0.9f + Random.Range(0f, 0.2f);
            audioSource.volume = 0.5f;
            audioSource.playOnAwake = false;
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
        if (picked) return;
        
        if (collidedObject.CompareTag("Player"))
        {
            CollectPowerup(collidedObject);
        }
    }
    
    private void CollectPowerup(GameObject player)
    {
        picked = true;
        
        if (powerupCollider != null)
            powerupCollider.enabled = false;
        
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        
        ApplyEffect(player);
        
        if (PowerupUIManager.Instance != null && powerupIcon != null)
        {
            PowerupUIManager.Instance.AddPowerupIcon(powerupType, powerupIcon, effectDuration);
        }
        
        if (feedbackPrefab != null)
        {
            GameObject feedback = Instantiate(feedbackPrefab, player.transform);
            Destroy(feedback, effectDuration);
        }
        
        AnimateCollection();
        
        if (audioSource != null && pickupSound != null)
        {
            audioSource.Play();
        }
        
        float destroyDelay = Mathf.Max(
            collectAnimationDuration,
            pickupSound != null ? pickupSound.length : 0f
        );
        
        Destroy(gameObject, destroyDelay + 0.1f);
    }
    
    private void AnimateCollection()
    {
        if (spriteRenderer == null) return;
        
        LeanTween.scale(gameObject, Vector3.one * 1.5f, collectAnimationDuration)
            .setEaseOutQuad();
        
        LeanTween.alpha(gameObject, 0f, collectAnimationDuration)
            .setEaseOutQuad();
        
        Vector3 targetPos = transform.position + Vector3.up * 0.5f;
        LeanTween.move(gameObject, targetPos, collectAnimationDuration)
            .setEaseOutQuad();
    }
    
    protected abstract void ApplyEffect(GameObject player);
    
    public void ApplyImpulse(Vector2 force)
    {
        if (rb != null && !picked)
            rb.velocity = force;
    }
}