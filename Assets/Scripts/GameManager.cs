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
        // ✅ Reiniciar GameState al inicio
        GameState.IsPaused = false;
        GameState.IsDead = false;
        GameState.IsVictorious = false;
        
        // ✅ Buscar referencias si no están asignadas
        if (spawner == null)
            spawner = FindObjectOfType<EnemySpawner>();
        
        if (playerRef == null)
            playerRef = FindObjectOfType<PlayerController>();
        
        player = playerRef;
        
        // ✅ Validaciones con mensajes claros
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
        
        // ✅ Verificar si el jugador murió (solo una vez)
        if (player != null && player.IsDead() && !GameState.IsDead)
        {
            GameState.IsDead = true;
            Debug.Log("🔴 Jugador murió - Llamando EndLevel(false)");
            EndLevel(false);
            return;
        }
        
        // ✅ Verificar victoria o siguiente oleada
        if (spawner != null && spawner.GetActiveEnemyCount() == 0)
        {
            // ✅ Si completamos todas las oleadas = VICTORIA
            if (currentWave >= waveCount && !GameState.IsVictorious)
            {
                Debug.Log($"🏆 Todas las oleadas completadas ({currentWave}/{waveCount}) - Llamando EndLevel(true)");
                EndLevel(true);
                return;
            }
            
            // ✅ Si no, esperar para la siguiente oleada
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
            Debug.Log($"📍 Oleada {currentWave}/{waveCount} iniciada con {enemyCount} enemigos");
        }
    }
    
    public void OnEnemyDefeated(int coinReward)
    {
        coinsEarned += coinReward;
        Debug.Log($"💀 Enemigo derrotado. Monedas ganadas: +{coinReward} (Total: {coinsEarned})");
    }
    
    public void EndLevel(bool victory)
    {
        if (!levelActive) 
        {
            Debug.Log("⚠️ EndLevel llamado pero levelActive ya es false");
            return;
        }
        
        levelActive = false;
        Debug.Log($"=== NIVEL TERMINADO === Victoria: {victory}, Monedas: {coinsEarned}");
    
        if (victory)
        {
            GameState.IsVictorious = true;
            coinsEarned = Mathf.RoundToInt(coinsEarned * 1.5f);
            Debug.Log($"🏆 Victoria - Mostrando VictoryMenu con {coinsEarned} monedas");
            
            if (victoryMenu != null)
            {
                victoryMenu.ShowVictoryScreen(coinsEarned);
            }
            else
            {
                Debug.LogError("❌ VictoryMenu es NULL - Asígnalo en el Inspector del GameManager!");
            }
        }
        else
        {
            GameState.IsDead = true;
            coinsEarned = Mathf.RoundToInt(coinsEarned * 0.5f);
            Debug.Log($"💀 Derrota - Mostrando DefeatMenu con {coinsEarned} monedas, oleada {currentWave}");
            
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
        Debug.Log($"💰 Monedas: +{amount} (Total: {coinsEarned})");
    }
    
    public bool IsLevelActive() => levelActive;
    public int GetCoinsEarned() => coinsEarned;
    public int GetCurrentWave() => currentWave;
    public int GetTotalWaves() => waveCount;
}