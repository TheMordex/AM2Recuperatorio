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
    
    [Header("UI Menus - ASIGNAR MANUALMENTE")]
    [SerializeField] private VictoryMenu victoryMenu;
    [SerializeField] private DefeatMenu defeatMenu;
    
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
        GameState.IsPaused = false;
        GameState.IsDead = false;
        GameState.IsVictorious = false;
        
        if (spawner == null)
            spawner = FindObjectOfType<EnemySpawner>();
        
        if (playerRef == null)
            playerRef = FindObjectOfType<PlayerController>();
        
        player = playerRef;
        
        if (spawner == null)
            Debug.LogError("GameManager: No se encontró EnemySpawner");
        
        if (player == null)
            Debug.LogError("GameManager: No se encontró PlayerController");
        
        if (victoryMenu == null)
            Debug.LogError("GameManager: VictoryMenu NO asignado en el Inspector!");
        
        if (defeatMenu == null)
            Debug.LogError("GameManager: DefeatMenu NO asignado en el Inspector!");
        
        StartWave();
    }
    
    private void Update()
    {
        if (!levelActive) return;
        
        if (player != null && player.IsDead() && !GameState.IsDead)
        {
            GameState.IsDead = true;
            EndLevel(false);
            return;
        }
        
        if (spawner != null && spawner.GetActiveEnemyCount() == 0)
        {
            if (currentWave >= waveCount && !GameState.IsVictorious)
            {
                EndLevel(true);
                return;
            }
            
            if (currentWave < waveCount)
            {
                waveTimer -= Time.deltaTime;
                
                if (waveTimer <= 0)
                {
                    StartWave();
                }
            }
        }
    }
    
    private void StartWave()
    {
        currentWave++;
        waveTimer = timeBetweenWaves;
        int enemyCount = Random.Range(3, 6);
        
        if (spawner != null)
        {
            spawner.SpawnWave(enemyCount, currentWave);
        }
    }
    
    public void OnEnemyDefeated(int coinReward)
    {
        coinsEarned += coinReward;
    }
    
    public void EndLevel(bool victory)
    {
        if (!levelActive) 
        {
            return;
        }
        
        levelActive = false;
    
        if (victory)
        {
            GameState.IsVictorious = true;
            coinsEarned = Mathf.RoundToInt(coinsEarned * 1.5f);
            
            if (CurrencyManager.Instance != null)
                CurrencyManager.Instance.AddCoins(coinsEarned);
            
            if (victoryMenu != null)
            {
                victoryMenu.ShowVictoryScreen(coinsEarned);
            }
        }
        else
        {
            GameState.IsDead = true;
            coinsEarned = Mathf.RoundToInt(coinsEarned * 0.5f);
            
            if (CurrencyManager.Instance != null)
                CurrencyManager.Instance.AddCoins(coinsEarned);
            
            if (defeatMenu != null)
            {
                defeatMenu.ShowDefeatScreen(coinsEarned, currentWave);
            }
        }
    }
    
    public void AddCoins(int amount)
    {
        coinsEarned += amount;
    }
    
    public bool IsLevelActive() => levelActive;
    public int GetCoinsEarned() => coinsEarned;
    public int GetCurrentWave() => currentWave;
    public int GetTotalWaves() => waveCount;
}