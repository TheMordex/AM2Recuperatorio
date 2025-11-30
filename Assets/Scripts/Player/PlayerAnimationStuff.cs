using UnityEngine;

public class PlayerAnimationStuff : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource audioSource;

    // Sólo para animación
    private Vector2 moveVector;
    private Vector2 lastMoveVector = Vector2.down; // dirección inicial

    private void Update()
    {
        HandleMovementAnimation();
        HandleAttackAnimation();
    }

    private void HandleMovementAnimation()
    {
        // Leemos input SOLO para animación
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(x, y);

        moveVector = input.normalized;

        // Guardamos última dirección válida
        if (input.sqrMagnitude > 0.01f)
        {
            lastMoveVector = moveVector;
        }

        if (playerAnimator == null) return;

        playerAnimator.SetFloat("Horizontal", moveVector.x);
        playerAnimator.SetFloat("Vertical", moveVector.y);
        playerAnimator.SetFloat("Speed", moveVector.magnitude);
        playerAnimator.SetFloat("LastHorizontal", lastMoveVector.x);
        playerAnimator.SetFloat("LastVertical", lastMoveVector.y);
    }

    private void HandleAttackAnimation()
    {
        if (playerAnimator == null) return;

        // Misma tecla/botón que uses para atacar en PlayerController
        if (Input.GetButtonDown("Fire1"))
        {
            playerAnimator.SetTrigger("Attack");
        }
    }

    // Llamado desde un Animation Event en la anim de caminar
    public void PlayFootstepsSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}