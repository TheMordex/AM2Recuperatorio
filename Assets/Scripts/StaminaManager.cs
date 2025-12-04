using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StaminaManager : MonoBehaviour
{
    public static StaminaManager Instance { get; private set; }
    
    [Header("Stamina Settings")]
    [SerializeField] private int maxStamina = 5;
    [SerializeField] private int staminaCostPerLevel = 1;
    [SerializeField] private float staminaRegenTime = 300f;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private Image staminaBar;
    
    private int currentStamina;
    private float staminaRegenTimer = 0f;
    private const string STAMINA_KEY = "CurrentStamina";
    private const string STAMINA_TIMER_KEY = "StaminaRegenTimer";
    
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
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        UpdateStaminaUI();
    }
    
    private void Update()
    {
        if (currentStamina < maxStamina)
        {
            staminaRegenTimer -= Time.deltaTime;
            
            if (staminaRegenTimer <= 0f)
            {
                RegenerateStamina();
            }
        }
        
        UpdateStaminaUI();
    }
    
    public bool CanPlayLevel()
    {
        if (currentStamina >= staminaCostPerLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void UseLevelStamina()
    {
        if (currentStamina >= staminaCostPerLevel)
        {
            currentStamina -= staminaCostPerLevel;
            staminaRegenTimer = staminaRegenTime;
            SaveStamina();
        }
    }
    
    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina++;
            staminaRegenTimer = staminaRegenTime;
            SaveStamina();
        }
    }
    
    private void SaveStamina()
    {
        PlayerPrefs.SetInt(STAMINA_KEY, currentStamina);
        PlayerPrefs.SetFloat(STAMINA_TIMER_KEY, staminaRegenTimer);
        PlayerPrefs.Save();
    }
    
    private void LoadStamina()
    {
        currentStamina = PlayerPrefs.GetInt(STAMINA_KEY, maxStamina);
        staminaRegenTimer = PlayerPrefs.GetFloat(STAMINA_TIMER_KEY, 0f);
    }
    
    private void UpdateStaminaUI()
    {
        if (staminaText != null)
            staminaText.text = $"⚡ {currentStamina}/{maxStamina}";
        
        if (staminaBar != null)
        {
            float fillAmount = (float)currentStamina / maxStamina;
            staminaBar.fillAmount = fillAmount;
        }
    }
    
    public int GetCurrentStamina() => currentStamina;
    public int GetMaxStamina() => maxStamina;
    public float GetStaminaRegenTime() => staminaRegenTimer;
}