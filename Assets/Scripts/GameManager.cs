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

    [Header("UI Menus")]
    [SerializeField] private VictoryMenu victoryMenu;
    [SerializeField] private DefeatMenu defeatMenu;

    [Header("Music")]
    public AudioClip musicaVictoria;
    public AudioClip musicaDerrota;

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

        if (!spawner) spawner = FindObjectOfType<EnemySpawner>();
        if (!playerRef) playerRef = FindObjectOfType<PlayerController>();

        player = playerRef;

        StartWave();
    }

    private void Update()
    {
        if (!levelActive) return;

        if (player && player.IsDead() && !GameState.IsDead)
        {
            GameState.IsDead = true;
            EndLevel(false);
            return;
        }

        if (spawner && spawner.GetActiveEnemyCount() == 0)
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

        if (spawner)
            spawner.SpawnWave(enemyCount, currentWave);
    }

    public void OnEnemyDefeated(int coinReward)
    {
        coinsEarned += coinReward;
    }

    public void EndLevel(bool victory)
    {
        if (!levelActive) return;

        levelActive = false;

        int finalCoins = victory
            ? Mathf.RoundToInt(coinsEarned * 1.5f)
            : Mathf.RoundToInt(coinsEarned * 0.5f);

        if (CurrencyManager.Instance)
            CurrencyManager.Instance.AddCoins(finalCoins);

        if (victory)
        {
            GameState.IsVictorious = true;

            if (musicaVictoria)
                AudioManager.Instance.FadeToVictory(musicaVictoria, 1.5f);

            if (victoryMenu)
                victoryMenu.ShowVictoryScreen(finalCoins);
        }
        else
        {
            GameState.IsDead = true;

            if (musicaDerrota)
                AudioManager.Instance.FadeToDefeat(musicaDerrota, 1.5f);

            if (defeatMenu)
                defeatMenu.ShowDefeatScreen(finalCoins, currentWave);
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
