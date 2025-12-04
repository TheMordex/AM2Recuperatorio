using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    private int totalCoins = 0;
    private const string COINS_KEY = "TotalCoins";
    
    [SerializeField] private TextMeshProUGUI coinsDisplayUI;
    
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
        UpdateUI();
        
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
            UpdateUI();
        }
    }
    
    private void FindUIReferences()
    {
        GameObject coinsUI = GameObject.Find("CoinsText");
        if (coinsUI != null)
        {
            coinsDisplayUI = coinsUI.GetComponent<TextMeshProUGUI>();
        }
    }
    
    private void Start()
    {
        UpdateUI();
    }
    
    public void AddCoins(int amount)
    {
        totalCoins += amount;
        SaveCoins();
        UpdateUI();
    }
    
    public bool RemoveCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            SaveCoins();
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public int GetTotalCoins() => totalCoins;
    
    public bool HasEnoughCoins(int amount) => totalCoins >= amount;
    
    private void SaveCoins()
    {
        PlayerPrefs.SetInt(COINS_KEY, totalCoins);
        PlayerPrefs.Save();
    }
    
    private void LoadCoins()
    {
        totalCoins = PlayerPrefs.GetInt(COINS_KEY, 0);
    }
    
    private void UpdateUI()
    {
        if (coinsDisplayUI != null)
            coinsDisplayUI.text = $"💰 {totalCoins}";
    }
    
    public void ResetCoins()
    {
        totalCoins = 0;
        PlayerPrefs.DeleteKey(COINS_KEY);
        PlayerPrefs.Save();
        UpdateUI();
        Debug.Log("🔄 Monedas reseteadas");
    }
}