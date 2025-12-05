using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Vector2 attackSize = new Vector2(1.5f, 1.5f);
    [SerializeField] public int damage = 10;
    [SerializeField] private float knockbackIntensity = 10f;
    [SerializeField] private float attackCooldown = 0.5f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip swingAudio;

    [Header("Visual Debug")]
    [SerializeField] private bool showAttackRange = true;
    [SerializeField] private Color gizmoColor = Color.red;

    private PlayerController playerController;
    private float attackTimer = 0f;
    private Vector2 lastMoveDirection = Vector2.down;
    private float baseKnockback;

    public event Action OnPlayerAttack;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        baseKnockback = knockbackIntensity;
    }

    private void Start()
    {
        if (UpgradeDataManager.Instance != null)
        {
            damage += UpgradeDataManager.Instance.GetCurrentDamageBonus();
            knockbackIntensity = baseKnockback + UpgradeDataManager.Instance.GetCurrentKnockbackBonus();
        }
    }

    private void Update()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;

        UpdateMoveDirection();
    }

    private void UpdateMoveDirection()
    {
        Vector2 moveInput = playerController.GetMoveInput();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInput;
        }
    }

    public void PerformAttackFromButton()
    {
        if (attackTimer > 0f || playerController.IsDead())
            return;

        attackTimer = attackCooldown;

        OnPlayerAttack?.Invoke();

        if (swingAudio != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(swingAudio);

        PerformAttack();
    }

    private void PerformAttack()
    {
        Vector2 attackPos = (Vector2)transform.position + lastMoveDirection * (attackSize.x / 2f);

        float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, attackSize, angle);

        bool hitSomething = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;
                enemy.ApplyKnockback(knockbackDir * knockbackIntensity, 0.2f);

                hitSomething = true;
            }
        }

        if (hitSomething && hitAudio != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(hitAudio);
        }
    }

    public int GetDamage() => damage;
    public void SetDamage(int dmg) => damage = dmg;
    public float GetKnockback() => knockbackIntensity;
    public void SetKnockback(float kb) => knockbackIntensity = kb;
    public float GetCooldown() => attackCooldown;
    public void SetCooldown(float cd) => attackCooldown = cd;

    private void OnDrawGizmos()
    {
        if (!showAttackRange) return;

        Vector2 attackPos = (Vector2)transform.position + lastMoveDirection * (attackSize.x / 2f);
        
        Gizmos.color = gizmoColor;
        
        Gizmos.matrix = Matrix4x4.TRS(attackPos, Quaternion.Euler(0, 0, Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackSize);
    }
}