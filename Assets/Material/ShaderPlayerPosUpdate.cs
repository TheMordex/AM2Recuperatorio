using UnityEngine;

public class ShaderPlayerPosUpdate : MonoBehaviour
{
    private PlayerController player;

    private void Awake()
    {
        // Buscamos una sola vez al jugador
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (player == null) return;

        // Mandamos la posición global al shader
        Shader.SetGlobalVector("_PlayerPos", player.transform.position);
    }
}
