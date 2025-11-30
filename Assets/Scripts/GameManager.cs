using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Wave Settings")]
    [SerializeField] private int waveCount = 5;
    [SerializeField] private float timeBetweenWaves = 3f;

    [Header("References")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private PlayerController playerRef;

    private int currentWave = 0;
    private float waveTimer = 0f;
    private int coinsEarned = 0;
    private bool levelActive = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (spawner == null) spawner = FindObjectOfType<EnemySpawner>();
        if (playerRef == null) playerRef = FindObjectOfType<PlayerController>();

        StartWave();
    }

    private void Update()
    {
        if (!levelActive) return;

        // Jugador muerto → derrota
        if (playerRef.IsDead())
        {
            EndLevel(false);
            return;
        }

        // Si no quedan enemigos
        if (spawner.GetActiveEnemyCount() == 0)
        {
            waveTimer -= Time.deltaTime;

            // Si NO quedan más oleadas → VICTORIA
            if (currentWave >= waveCount && waveTimer <= 0f)
            {
                EndLevel(true);
                return;
            }

            // Nueva oleada
            if (waveTimer <= 0f && currentWave < waveCount)
            {
                StartWave();
            }
        }
    }

    private void StartWave()
    {
        currentWave++;
        waveTimer = timeBetweenWaves;

        int enemyCount = Random.Range(3, 6);

        spawner.SpawnWave(enemyCount, currentWave);

        Debug.Log($"OLEADA {currentWave}/{waveCount} SPAWNEADA ({enemyCount} enemigos)");
    }

    public void OnEnemyDefeated(int coins)
    {
        coinsEarned += coins;
    }

    public void EndLevel(bool victory)
    {
        if (!levelActive) return;

        levelActive = false;

        if (victory)
        {
            coinsEarned = Mathf.RoundToInt(coinsEarned * 1.5f);
            Debug.Log($"VICTORIA! Monedas obtenidas: {coinsEarned}");
        }
        else
        {
            coinsEarned = Mathf.RoundToInt(coinsEarned * 0.5f);
            Debug.Log($"DERROTA! Monedas obtenidas: {coinsEarned}");
        }
    }

    public int GetCurrentWave() => currentWave;
    public int GetTotalWaves() => waveCount;
    public bool IsLevelActive() => levelActive;
    public int GetCoinsEarned() => coinsEarned;
}
