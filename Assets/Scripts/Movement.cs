using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float snappiness = 0.01f;
    
    private Rigidbody2D rb;
    private Vector2 movementVector = Vector2.zero;
    private Vector2 prevDirection = Vector2.zero;
    private Vector2 pushDirection = Vector2.zero;
    private Vector2 prevPushDirection = Vector2.zero;
    private float pushSnappiness = 0.1f;
    private Vector2 lastMovementDirection = Vector2.zero;
    private bool isMoving;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        prevDirection = Vector2.Lerp(prevDirection, movementVector, snappiness);
        
        if (movementVector != Vector2.zero)
        {
            isMoving = true;
            lastMovementDirection = movementVector;
            movementVector = Vector2.zero;
        }
        else
        {
            isMoving = false;
        }
        
        if (pushDirection != Vector2.zero)
        {
            prevPushDirection = pushDirection;
            pushDirection = Vector2.zero;
        }
        
        prevPushDirection = Vector2.Lerp(prevPushDirection, Vector2.zero, pushSnappiness);

        Vector3 finalMovement = (prevDirection * speed + prevPushDirection) * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + finalMovement);
    }

    public void Impulse(Vector2 direction)
    {
        pushDirection = direction;
        pushSnappiness = 0.1f;
    }

    public void MoveNormalized(Vector2 direction)
    {
        movementVector = direction.normalized;
    }

    public void Move(Vector2 direction)
    {
        movementVector = direction;
    }

    public Vector2 GetDirectionVector()
    {
        return prevDirection;
    }

    public Vector2 GetMoveVector()
    {
        return GetDirectionVector() * speed;
    }

    public Vector2 GetLastMoveVector()
    {
        return lastMovementDirection.normalized;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}