using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.0f;
    
    [Header("Avoidance")]
    [SerializeField] private float avoidanceRadius = 1.5f;
    [SerializeField] private float avoidanceForce = 1.5f;
    [SerializeField] private LayerMask obstacleLayer;
    
    private PlayerController player;
    private Rigidbody2D rb;
    private Vector2 avoidanceDirection = Vector2.zero;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void Initialize(PlayerController targetPlayer)
    {
        player = targetPlayer;
    }
    
    public Vector2 CalculateMovement()
    {
        if (player == null)
            return Vector2.zero;
        
        // Dirección hacia el jugador
        Vector2 dirToPlayer = (player.transform.position - transform.position).normalized;
        
        // Detectar obstáculos cercanos
        Collider2D[] obstaclesNearby = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, obstacleLayer);
        
        Vector2 avoidance = Vector2.zero;
        
        // Calcular fuerza de evasión
        foreach (Collider2D obstacle in obstaclesNearby)
        {
            if (obstacle.gameObject == gameObject) continue;
            
            Vector2 dirAwayFromObstacle = (Vector2)(transform.position - obstacle.transform.position).normalized;
            float distance = Vector2.Distance(transform.position, obstacle.transform.position);
            float force = 1f - (distance / avoidanceRadius);
            
            avoidance += dirAwayFromObstacle * force;
        }
        
        avoidance = avoidance.normalized * avoidanceForce;
        
        // Combinar dirección al jugador con evasión
        Vector2 finalDirection = (dirToPlayer + avoidance).normalized;
        
        return finalDirection * moveSpeed;
    }
}