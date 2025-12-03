using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerController player { get; private set; }
    
    [Header("Wave Settings")]
    [SerializeField] private int waveCount = 5;
    [SerializeField] private float timeBetweenWaves = 3f;
    
    [Header("References")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private PlayerController playerRef;
    
    private int currentWave = 0;
    private float waveTimer = 0f;
    private bool levelActive = true;
    private int coinsEarned = 0;
    
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
        if (spawner == null)
            spawner = FindObjectOfType<EnemySpawner>();
        
        if (playerRef == null)
            playerRef = FindObjectOfType<PlayerController>();
        
        player = playerRef;
        
        if (spawner == null)
            Debug.LogError("GameManager: No se encontró EnemySpawner");
        
        if (player == null)
            Debug.LogError("GameManager: No se encontró PlayerController");
        
        StartWave();
    }
    
    private void Update()
    {
        if (!levelActive) return;
        
        if (spawner.GetActiveEnemyCount() == 0 && currentWave < waveCount)
        {
            waveTimer -= Time.deltaTime;
            
            if (waveTimer <= 0)
            {
                StartWave();
            }
        }
        
        if (player != null && player.IsDead())
        {
            EndLevel(false);
        }
    }
    
    private void StartWave()
    {
        currentWave++;
        waveTimer = timeBetweenWaves;
        int enemyCount = Random.Range(3, 6);
        spawner.SpawnWave(enemyCount, currentWave);
        Debug.Log($"Ola {currentWave}/{waveCount} iniciada con {enemyCount} enemigos");
    }
    
    public void OnEnemyDefeated(int coinReward)
    {
        coinsEarned += coinReward;
    }
    
    public void EndLevel(bool victory)
    {
        levelActive = false;
    
        if (victory)
        {
            coinsEarned = Mathf.RoundToInt(coinsEarned * 1.5f);
            VictoryMenu victoryMenu = FindObjectOfType<VictoryMenu>();
            if (victoryMenu != null)
                victoryMenu.ShowVictoryScreen(coinsEarned);
        }
    }
    
    public void AddCoins(int amount)
    {
        coinsEarned += amount;
        Debug.Log($"Monedas: +{amount} (Total: {coinsEarned})");
    }
    
    public bool IsLevelActive() => levelActive;
    public int GetCoinsEarned() => coinsEarned;
    public int GetCurrentWave() => currentWave;
    public int GetTotalWaves() => waveCount;
}