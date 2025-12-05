using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationStuff : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button attackButton;

    private PlayerController playerController;
    private Vector2 moveVector;
    private Vector2 lastMoveVector = Vector2.down;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        
        // Conectar botón de ataque
        if (attackButton != null)
            attackButton.onClick.AddListener(OnAttackButtonPressed);
    }

    private void Update()
    {
        HandleMovementAnimation();
    }

    private void HandleMovementAnimation()
    {
        if (playerController == null) return;

        // Obtener el input del joystick desde PlayerController
        moveVector = playerController.GetMoveInput();

        // Guardar última dirección válida
        if (moveVector.sqrMagnitude > 0.01f)
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

    private void OnAttackButtonPressed()
    {
        if (playerAnimator == null) return;
        
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.PerformAttackFromButton();
        }

        playerAnimator.SetTrigger("Attack");
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