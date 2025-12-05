using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    private int totalCoins = 0;
    private const string COINS_KEY = "TotalCoins";
    
    [SerializeField] private TextMeshProUGUI coinsDisplayUI;
    
    // Método para asegurar que la instancia exista
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeManager()
    {
        if (Instance == null)
        {
            GameObject managerObj = new GameObject("CurrencyManager");
            managerObj.AddComponent<CurrencyManager>();
            DontDestroyOnLoad(managerObj);
            Debug.Log("✅ CurrencyManager creado automáticamente al inicio");
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
        
        LoadCoins();
        
        Debug.Log($"💰 CurrencyManager inicializado. Monedas cargadas: {totalCoins}");
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        // Solo remover el listener si esta es la instancia principal
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"🔄 Escena cargada: {scene.name}");
        
        // Buscar UI en cualquier escena que la necesite
        FindUIReferences();
        UpdateUI();
    }
    
    private void FindUIReferences()
    {
        // Buscar por nombre
        GameObject coinsUI = GameObject.Find("CoinsText");
        if (coinsUI != null)
        {
            coinsDisplayUI = coinsUI.GetComponent<TextMeshProUGUI>();
            Debug.Log("✅ CoinsText UI encontrado");
        }
        else
        {
            // Si no lo encuentra por nombre, buscar en displayInfo
            GameObject displayInfo = GameObject.Find("DisplayInfo");
            if (displayInfo != null)
            {
                TextMeshProUGUI[] texts = displayInfo.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var text in texts)
                {
                    if (text.name == "CoinsText")
                    {
                        coinsDisplayUI = text;
                        Debug.Log("✅ CoinsText UI encontrado en DisplayInfo");
                        break;
                    }
                }
            }
        }
    }
    
    private void Start()
    {
        FindUIReferences();
        UpdateUI();
    }
    
    private void Update()
    {
        // Verificar UI cada frame (solo si es null)
        if (coinsDisplayUI == null)
        {
            FindUIReferences();
        }
    }
    
    public void AddCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log($"💰 +{amount} monedas. Total: {totalCoins}");
        SaveCoins();
        UpdateUI();
    }
    
    public bool RemoveCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            Debug.Log($"💸 -{amount} monedas. Total: {totalCoins}");
            SaveCoins();
            UpdateUI();
            return true;
        }
        Debug.LogWarning($"⚠️ No hay suficientes monedas. Requerido: {amount}, Actual: {totalCoins}");
        return false;
    }
    
    public int GetTotalCoins() => totalCoins;
    
    public bool HasEnoughCoins(int amount) => totalCoins >= amount;
    
    private void SaveCoins()
    {
        PlayerPrefs.SetInt(COINS_KEY, totalCoins);
        PlayerPrefs.Save();
        Debug.Log($"💾 Monedas guardadas: {totalCoins}");
    }
    
    private void LoadCoins()
    {
        totalCoins = PlayerPrefs.GetInt(COINS_KEY, 0);
        Debug.Log($"📂 Monedas cargadas desde PlayerPrefs: {totalCoins}");
    }
    
    private void UpdateUI()
    {
        if (coinsDisplayUI != null)
        {
            coinsDisplayUI.text = totalCoins.ToString();
        }
    }
    
    public void ResetCoins()
    {
        totalCoins = 0;
        PlayerPrefs.DeleteKey(COINS_KEY);
        PlayerPrefs.Save();
        Debug.Log("🔄 Monedas reseteadas a 0");
        UpdateUI();
    }
}