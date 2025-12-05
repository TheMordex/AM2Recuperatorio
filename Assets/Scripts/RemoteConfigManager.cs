using UnityEngine;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using System.Threading.Tasks;

public class RemoteConfigManager : MonoBehaviour
{
    public static RemoteConfigManager Instance { get; private set; }
    
    [Header("Remote Config Values")]
    // Variables obligatorias
    public int maxStamina = 5;
    public bool enablePowerUps = true;
    
    // Variables adicionales
    public float staminaRegenTime = 60f;
    public float playerMoveSpeed = 5f;
    public float coinRewardMultiplier = 1f;
    
    [Header("Status")]
    [SerializeField] private bool isConfigLoaded = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        Debug.Log("RemoteConfigManager creado");
    }
    
    private async void Start()
    {
        await InitializeRemoteConfig();
    }
    
    private async Task InitializeRemoteConfig()
    {
        try
        {
            Debug.Log("Inicializando Unity Services...");
            
            // Inicializar Unity Services
            await UnityServices.InitializeAsync();
            
            Debug.Log("Fetching Remote Config...");
            
            // Fetch configuraciones
            await RemoteConfigService.Instance.FetchConfigsAsync(
                new UserAttributes(), 
                new AppAttributes()
            );
            
            // Aplicar valores
            ApplyRemoteConfig();
            
            isConfigLoaded = true;
            Debug.Log("Remote Config cargado exitosamente!");
            
            // Aplicar a los managers
            ApplyToManagers();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al cargar Remote Config: {e.Message}");
            Debug.Log(" Usando valores por defecto");
        }
    }
    
    private void ApplyRemoteConfig()
    {
        // Obtener valores del Remote Config con valores por defecto como fallback
        maxStamina = RemoteConfigService.Instance.appConfig.GetInt("maxStamina", 5);
        enablePowerUps = RemoteConfigService.Instance.appConfig.GetBool("enablePowerUps", true);
        staminaRegenTime = RemoteConfigService.Instance.appConfig.GetFloat("staminaRegenTime", 60f);
        playerMoveSpeed = RemoteConfigService.Instance.appConfig.GetFloat("playerMoveSpeed", 5f);
        coinRewardMultiplier = RemoteConfigService.Instance.appConfig.GetFloat("coinRewardMultiplier", 1f);
        
        Debug.Log("Remote Config aplicado:");
        Debug.Log($"  • maxStamina: {maxStamina}");
        Debug.Log($"  • enablePowerUps: {enablePowerUps}");
        Debug.Log($"  • staminaRegenTime: {staminaRegenTime}s");
        Debug.Log($"  • playerMoveSpeed: {playerMoveSpeed}");
        Debug.Log($"  • coinRewardMultiplier: {coinRewardMultiplier}x");
    }
    
    private void ApplyToManagers()
    {
        // Aplicar a StaminaManager si existe
        if (StaminaManager.Instance != null)
        {
            StaminaManager.Instance.UpdateMaxStamina(maxStamina);
            Debug.Log($"Stamina settings aplicados: Max={maxStamina}, Regen={staminaRegenTime}s");
        }
    }
    
    // Método público para refrescar config manualmente
    public async void RefreshConfig()
    {
        Debug.Log("Refrescando Remote Config...");
        await InitializeRemoteConfig();
    }
    
    // Getters públicos
    public int GetMaxStamina() => maxStamina;
    public bool ArePowerUpsEnabled() => enablePowerUps;
    public float GetStaminaRegenTime() => staminaRegenTime;
    public float GetPlayerMoveSpeed() => playerMoveSpeed;
    public float GetCoinRewardMultiplier() => coinRewardMultiplier;
    public bool IsConfigLoaded() => isConfigLoaded;
    
    // Structs requeridos para Remote Config
    public struct UserAttributes { }
    public struct AppAttributes { }
}