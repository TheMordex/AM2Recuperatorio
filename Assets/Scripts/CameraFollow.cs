using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;           // Jugador
    [SerializeField] private new GameObject camera;      // Cámara que queremos mover

    [Header("Offsets")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
    [SerializeField] private float snappiness = 5f;
    [SerializeField] private float lookAheadIntensityStill = 1f;
    [SerializeField] private float lookAheadIntensityMove = 1f;

    private Vector2 lastMoveVector = Vector2.down;       // Última dirección en la que se movió el jugador

    private void Awake()
    {
        // Si no se asignó nada en el inspector, tratamos de autocompletar
        if (camera == null && Camera.main != null)
            camera = Camera.main.gameObject;

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (camera == null || target == null) return;

        // Leemos el input para saber la dirección actual de movimiento
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 moveVector = new Vector2(x, y).normalized;

        // Guardamos la última dirección válida
        if (moveVector.sqrMagnitude > 0.01f)
        {
            lastMoveVector = moveVector;
        }

        // Look ahead similar al script viejo: un poco hacia donde se movió y hacia donde se mueve
        Vector3 lookAhead =
            (Vector3)(lastMoveVector * lookAheadIntensityStill) +
            (Vector3)(moveVector * lookAheadIntensityMove);

        Vector3 desiredPosition = target.position + offset + lookAhead;

        camera.transform.position = Vector3.Lerp(
            camera.transform.position,
            desiredPosition,
            snappiness * Time.deltaTime
        );
    }
}
