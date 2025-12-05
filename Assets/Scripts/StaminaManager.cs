using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class StaminaManager : MonoBehaviour
{
    public static StaminaManager Instance { get; private set; }
    
    [Header("Stamina Settings")]
    [SerializeField] private int maxStamina = 5;
    [SerializeField] private int staminaCostPerLevel = 1;
    [SerializeField] private float staminaRegenTime = 60f; 
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private Image staminaBar;
    
    private int currentStamina;
    private const string STAMINA_KEY = "CurrentStamina";
    private const string LAST_STAMINA_TIME_KEY = "LastStaminaTime";
    
    // Inicialización automática
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeManager()
    {
        if (Instance == null)
        {
            GameObject managerObj = new GameObject("StaminaManager");
            managerObj.AddComponent<StaminaManager>();
            DontDestroyOnLoad(managerObj);
            Debug.Log("✅ StaminaManager creado automáticamente al inicio");
        }
    }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadStamina();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("✅ StaminaManager inicializado");
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        if (Instance == this)
        {
            Instance = null;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuScene")
        {
            FindUIReferences();
            UpdateStaminaUI();
        }
    }
    
    private void FindUIReferences()
    {
        GameObject staminaTextObj = GameObject.Find("StaminaText");
        if (staminaTextObj != null)
        {
            staminaText = staminaTextObj.GetComponent<TextMeshProUGUI>();
        }
        
        GameObject staminaBarObj = GameObject.Find("StaminaBar");
        if (staminaBarObj != null)
        {
            staminaBar = staminaBarObj.GetComponent<Image>();
        }
    }
    
    private void Start()
    {
        FindUIReferences();
        UpdateStaminaUI();
    }
    
    private void Update()
    {
        if (staminaText == null || staminaBar == null)
        {
            FindUIReferences();
        }
        
        // Verificar regeneración de stamina basada en tiempo real
        CheckStaminaRegeneration();
        
        UpdateStaminaUI();
    }
    
    private void CheckStaminaRegeneration()
    {
        if (currentStamina >= maxStamina)
            return;
        
        // Obtener el tiempo guardado de la última actualización
        string lastTimeString = PlayerPrefs.GetString(LAST_STAMINA_TIME_KEY, "");
        
        if (string.IsNullOrEmpty(lastTimeString))
        {
            // Si no hay tiempo guardado, guardar el tiempo actual
            SaveLastStaminaTime();
            return;
        }
        
        // Convertir el string a DateTime
        DateTime lastTime;
        if (!DateTime.TryParse(lastTimeString, out lastTime))
        {
            SaveLastStaminaTime();
            return;
        }
        
        // Calcular cuánto tiempo ha pasado
        TimeSpan timePassed = DateTime.Now - lastTime;
        float secondsPassed = (float)timePassed.TotalSeconds;
        
        // Calcular cuántas staminas deberían regenerarse
        int staminaToRegenerate = Mathf.FloorToInt(secondsPassed / staminaRegenTime);
        
        if (staminaToRegenerate > 0)
        {
            currentStamina = Mathf.Min(currentStamina + staminaToRegenerate, maxStamina);
            
            // Actualizar el tiempo guardado
            DateTime newLastTime = lastTime.AddSeconds(staminaToRegenerate * staminaRegenTime);
            PlayerPrefs.SetString(LAST_STAMINA_TIME_KEY, newLastTime.ToString());
            
            SaveStamina();
            
            Debug.Log($"⚡ Stamina regenerada: +{staminaToRegenerate}. Actual: {currentStamina}/{maxStamina}");
        }
    }
    
    public bool CanPlayLevel()
    {
        return currentStamina >= staminaCostPerLevel;
    }
    
    public void UseLevelStamina()
    {
        if (currentStamina >= staminaCostPerLevel)
        {
            currentStamina -= staminaCostPerLevel;
            SaveLastStaminaTime();
            SaveStamina();
            
            Debug.Log($"🎮 Stamina usada: -{staminaCostPerLevel}. Actual: {currentStamina}/{maxStamina}");
        }
    }
    
    private void SaveStamina()
    {
        PlayerPrefs.SetInt(STAMINA_KEY, currentStamina);
        PlayerPrefs.Save();
    }
    
    private void SaveLastStaminaTime()
    {
        PlayerPrefs.SetString(LAST_STAMINA_TIME_KEY, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }
    
    public void UpdateMaxStamina(int newMax)
    {
        maxStamina = newMax;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
        SaveStamina();
        UpdateStaminaUI();
        
        Debug.Log($"📈 Max Stamina actualizada a: {maxStamina}");
    }

    public void ResetStamina()
    {
        currentStamina = maxStamina;
        PlayerPrefs.DeleteKey(STAMINA_KEY);
        PlayerPrefs.DeleteKey(LAST_STAMINA_TIME_KEY);
        SaveLastStaminaTime();
        PlayerPrefs.Save();
        UpdateStaminaUI();
        
        Debug.Log("🔄 Stamina reseteada");
    }
    
    private void LoadStamina()
    {
        currentStamina = PlayerPrefs.GetInt(STAMINA_KEY, maxStamina);
        
        // Verificar regeneración al cargar
        CheckStaminaRegeneration();
        
        Debug.Log($"📂 Stamina cargada: {currentStamina}/{maxStamina}");
    }
    
    private void UpdateStaminaUI()
    {
        if (staminaText != null)
            staminaText.text = $"{currentStamina}/{maxStamina}";
        
        if (staminaBar != null)
        {
            float fillAmount = (float)currentStamina / maxStamina;
            staminaBar.fillAmount = fillAmount;
        }
    }
    
    // Métodos útiles para mostrar tiempo restante
    public float GetTimeUntilNextStamina()
    {
        if (currentStamina >= maxStamina)
            return 0f;
        
        string lastTimeString = PlayerPrefs.GetString(LAST_STAMINA_TIME_KEY, "");
        if (string.IsNullOrEmpty(lastTimeString))
            return staminaRegenTime;
        
        DateTime lastTime;
        if (!DateTime.TryParse(lastTimeString, out lastTime))
            return staminaRegenTime;
        
        TimeSpan timePassed = DateTime.Now - lastTime;
        float secondsPassed = (float)timePassed.TotalSeconds;
        float remainder = secondsPassed % staminaRegenTime;
        
        return staminaRegenTime - remainder;
    }
    
    public string GetTimeUntilNextStaminaFormatted()
    {
        float seconds = GetTimeUntilNextStamina();
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        
        return $"{minutes:00}:{secs:00}";
    }
    
    public int GetCurrentStamina() => currentStamina;
    public int GetMaxStamina() => maxStamina;
}