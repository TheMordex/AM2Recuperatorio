using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource footstepsAudio;

    private Vector2 lastDirection = Vector2.down;

    private void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        footstepsAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Vector2 velocity = rb.velocity;

        // --- Movimiento ---
        animator.SetFloat("Speed", velocity.magnitude);

        if (velocity.sqrMagnitude > 0.01f)
        {
            animator.SetFloat("Horizontal", velocity.x);
            animator.SetFloat("Vertical", velocity.y);
            lastDirection = velocity.normalized;
        }
        else
        {
            // última dirección para IDLE direccional
            animator.SetFloat("LastHorizontal", lastDirection.x);
            animator.SetFloat("LastVertical", lastDirection.y);
        }
    }

    // --- ATAQUE (se llama desde Enemy o EnemyMage) ---
    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayFootstep()
    {
        if (footstepsAudio != null)
            footstepsAudio.Play();
    }

    public void PlayFootstepsSound()   // <- ESTE ES EL QUE PIDE EL ANIMATION EVENT
    {
        if (footstepsAudio != null)
            footstepsAudio.Play();
    }
}