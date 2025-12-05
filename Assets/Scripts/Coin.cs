using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private float collectAnimationDuration = 0.3f;
    
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private Collider2D coinCollider;
    private bool picked = false;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coinCollider = GetComponent<Collider2D>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
        }
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        if (coinSound != null)
        {
            audioSource.clip = coinSound;
            audioSource.pitch = 0.9f + Random.Range(0f, 0.2f);
            audioSource.volume = 0.4f + Random.Range(0f, 0.2f);
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
            CollectCoin();
        }
    }
    
    private void CollectCoin()
    {
        picked = true;
        
        if (coinCollider != null)
            coinCollider.enabled = false;
        
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoins(value);
        }
        
        AnimateCollection();
        
        if (audioSource != null && coinSound != null)
        {
            audioSource.Play();
        }
        
        float destroyDelay = Mathf.Max(
            collectAnimationDuration,
            coinSound != null ? coinSound.length : 0f
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
    
    public void ApplyImpulse(Vector2 force)
    {
        if (rb != null && !picked)
            rb.velocity = force;
    }
    
    public void SetValue(int newValue)
    {
        value = newValue;
    }
    
    public int GetValue() => value;
}