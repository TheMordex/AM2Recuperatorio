using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UpgradeData
{
    public int maxHealthLevel = 0;
    public int damageLevel = 0;
    public int knockbackLevel = 0;
    public int staminaPurchases = 0;
    
    public int currentMaxHealthBonus = 0;
    public int currentDamageBonus = 0;
    public float currentKnockbackBonus = 0f;
    public int currentMaxStamina = 5;
}

public class UpgradeDataManager : MonoBehaviour
{
    public static UpgradeDataManager Instance { get; private set; }
    
    private UpgradeData upgradeData;
    private const string UPGRADE_DATA_KEY = "UpgradeData";
    
    [Header("Upgrade Costs")]
    [SerializeField] private List<int> maxHealthLevelCosts = new List<int> { 100, 200, 300, 400, 500 };
    [SerializeField] private List<int> damageLevelCosts = new List<int> { 80, 160, 240, 320, 400 };
    [SerializeField] private List<int> knockbackLevelCosts = new List<int> { 60, 120, 180, 240, 300 };
    [SerializeField] private int staminaBaseCost = 150;
    
    [Header("Upgrade Bonuses")]
    [SerializeField] private List<int> maxHealthBonuses = new List<int> { 20, 40, 60, 80, 100 };
    [SerializeField] private List<int> damageBonuses = new List<int> { 5, 10, 15, 20, 25 };
    [SerializeField] private List<float> knockbackBonuses = new List<float> { 3f, 6f, 9f, 12f, 15f };
    
    // Inicialización automática
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeManager()
    {
        if (Instance == null)
        {
            GameObject managerObj = new GameObject("UpgradeDataManager");
            managerObj.AddComponent<UpgradeDataManager>();
            DontDestroyOnLoad(managerObj);
            Debug.Log("✅ UpgradeDataManager creado automáticamente al inicio");
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
        
        LoadUpgradeData();
        Debug.Log("✅ UpgradeDataManager inicializado");
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    
    public int GetMaxHealthLevel() => upgradeData.maxHealthLevel;
    public int GetDamageLevel() => upgradeData.damageLevel;
    public int GetKnockbackLevel() => upgradeData.knockbackLevel;
    public int GetStaminaPurchases() => upgradeData.staminaPurchases;
    
    public int GetCurrentMaxHealthBonus() => upgradeData.currentMaxHealthBonus;
    public int GetCurrentDamageBonus() => upgradeData.currentDamageBonus;
    public float GetCurrentKnockbackBonus() => upgradeData.currentKnockbackBonus;
    public int GetCurrentMaxStamina() => upgradeData.currentMaxStamina;
    
    public int GetUpgradeCost(string upgradeType)
    {
        switch(upgradeType)
        {
            case "MaxHealth":
                if (upgradeData.maxHealthLevel >= maxHealthLevelCosts.Count) return -1;
                return maxHealthLevelCosts[upgradeData.maxHealthLevel];
            
            case "Damage":
                if (upgradeData.damageLevel >= damageLevelCosts.Count) return -1;
                return damageLevelCosts[upgradeData.damageLevel];
            
            case "Knockback":
                if (upgradeData.knockbackLevel >= knockbackLevelCosts.Count) return -1;
                return knockbackLevelCosts[upgradeData.knockbackLevel];
            
            case "Stamina":
                return staminaBaseCost + (upgradeData.staminaPurchases * 20);
            
            default:
                return -1;
        }
    }
    
    public bool CanUpgrade(string upgradeType)
    {
        int cost = GetUpgradeCost(upgradeType);
        if (cost == -1) return false;
        
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("❌ CurrencyManager.Instance es NULL en CanUpgrade!");
            return false;
        }
        
        return CurrencyManager.Instance.GetTotalCoins() >= cost;
    }
    
    public bool BuyUpgrade(string upgradeType)
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("❌ CurrencyManager.Instance es NULL en BuyUpgrade!");
            return false;
        }
        
        int cost = GetUpgradeCost(upgradeType);
        
        if (cost == -1 || !CurrencyManager.Instance.RemoveCoins(cost))
            return false;
        
        switch(upgradeType)
        {
            case "MaxHealth":
                upgradeData.maxHealthLevel++;
                upgradeData.currentMaxHealthBonus = maxHealthBonuses[upgradeData.maxHealthLevel - 1];
                break;
            
            case "Damage":
                upgradeData.damageLevel++;
                upgradeData.currentDamageBonus = damageBonuses[upgradeData.damageLevel - 1];
                break;
            
            case "Knockback":
                upgradeData.knockbackLevel++;
                upgradeData.currentKnockbackBonus = knockbackBonuses[upgradeData.knockbackLevel - 1];
                break;
            
            case "Stamina":
                upgradeData.staminaPurchases++;
                upgradeData.currentMaxStamina = 5 + (upgradeData.staminaPurchases * 2);
                
                if (StaminaManager.Instance != null)
                    StaminaManager.Instance.UpdateMaxStamina(upgradeData.currentMaxStamina);
                break;
        }
        
        SaveUpgradeData();
        return true;
    }
    
    private void SaveUpgradeData()
    {
        string json = JsonUtility.ToJson(upgradeData);
        PlayerPrefs.SetString(UPGRADE_DATA_KEY, json);
        PlayerPrefs.Save();
    }
    
    private void LoadUpgradeData()
    {
        if (PlayerPrefs.HasKey(UPGRADE_DATA_KEY))
        {
            string json = PlayerPrefs.GetString(UPGRADE_DATA_KEY);
            upgradeData = JsonUtility.FromJson<UpgradeData>(json);
        }
        else
        {
            upgradeData = new UpgradeData();
            SaveUpgradeData();
        }
    }
    
    public void ResetAllData()
    {
        upgradeData = new UpgradeData();
        PlayerPrefs.DeleteKey(UPGRADE_DATA_KEY);
        PlayerPrefs.Save();
        
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.ResetCoins();
        
        if (StaminaManager.Instance != null)
            StaminaManager.Instance.ResetStamina();
    }
}