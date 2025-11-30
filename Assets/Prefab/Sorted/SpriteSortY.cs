using UnityEngine;

public class SpriteSortY : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int offset = 0;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (spriteRenderer == null) return;

        // Orden basado en la posición Y del objeto (estándar para juegos top-down)
        int order = offset - Mathf.RoundToInt(transform.position.y * 100f);
        spriteRenderer.sortingOrder = order;
    }
}