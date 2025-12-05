using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinsText;
    
    [Header("Max Health Upgrade")]
    [SerializeField] private Button maxHealthBuyButton;
    [SerializeField] private TextMeshProUGUI maxHealthLevelText;
    [SerializeField] private TextMeshProUGUI maxHealthCostText;
    
    [Header("Damage Upgrade")]
    [SerializeField] private Button damageBuyButton;
    [SerializeField] private TextMeshProUGUI damageLevelText;
    [SerializeField] private TextMeshProUGUI damageCostText;
    
    [Header("Knockback Upgrade")]
    [SerializeField] private Button knockbackBuyButton;
    [SerializeField] private TextMeshProUGUI knockbackLevelText;
    [SerializeField] private TextMeshProUGUI knockbackCostText;
    
    [Header("Stamina Upgrade")]
    [SerializeField] private Button staminaBuyButton;
    [SerializeField] private TextMeshProUGUI staminaCountText;
    [SerializeField] private TextMeshProUGUI staminaCostText;
    
    private void Start()
    {
        if (maxHealthBuyButton != null)
            maxHealthBuyButton.onClick.AddListener(() => BuyUpgrade("MaxHealth"));
        
        if (damageBuyButton != null)
            damageBuyButton.onClick.AddListener(() => BuyUpgrade("Damage"));
        
        if (knockbackBuyButton != null)
            knockbackBuyButton.onClick.AddListener(() => BuyUpgrade("Knockback"));
        
        if (staminaBuyButton != null)
            staminaBuyButton.onClick.AddListener(() => BuyUpgrade("Stamina"));
        
        UpdateUI();
    }
    
    private void Update()
    {
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (UpgradeDataManager.Instance == null || CurrencyManager.Instance == null)
            return;
        
        if (coinsText != null)
            coinsText.text = CurrencyManager.Instance.GetTotalCoins().ToString();
        
        int maxHealthLevel = UpgradeDataManager.Instance.GetMaxHealthLevel();
        if (maxHealthLevelText != null)
            maxHealthLevelText.text = $"Nivel: {maxHealthLevel}/5";
        
        int maxHealthCost = UpgradeDataManager.Instance.GetUpgradeCost("MaxHealth");
        if (maxHealthCostText != null)
            maxHealthCostText.text = maxHealthCost == -1 ? "MAXIMO" : $"${maxHealthCost}";
        
        if (maxHealthBuyButton != null)
            maxHealthBuyButton.interactable = UpgradeDataManager.Instance.CanUpgrade("MaxHealth");
        
        int damageLevel = UpgradeDataManager.Instance.GetDamageLevel();
        if (damageLevelText != null)
            damageLevelText.text = $"Nivel: {damageLevel}/5";
        
        int damageCost = UpgradeDataManager.Instance.GetUpgradeCost("Damage");
        if (damageCostText != null)
            damageCostText.text = damageCost == -1 ? "MAXIMO" : $"${damageCost}";
        
        if (damageBuyButton != null)
            damageBuyButton.interactable = UpgradeDataManager.Instance.CanUpgrade("Damage");
        
        int knockbackLevel = UpgradeDataManager.Instance.GetKnockbackLevel();
        if (knockbackLevelText != null)
            knockbackLevelText.text = $"Nivel: {knockbackLevel}/5";
        
        int knockbackCost = UpgradeDataManager.Instance.GetUpgradeCost("Knockback");
        if (knockbackCostText != null)
            knockbackCostText.text = knockbackCost == -1 ? "MAXIMO" : $"${knockbackCost}";
        
        if (knockbackBuyButton != null)
            knockbackBuyButton.interactable = UpgradeDataManager.Instance.CanUpgrade("Knockback");
        
        int staminaPurchases = UpgradeDataManager.Instance.GetStaminaPurchases();
        if (staminaCountText != null)
            staminaCountText.text = $"Compras: {staminaPurchases}";
        
        int staminaCost = UpgradeDataManager.Instance.GetUpgradeCost("Stamina");
        if (staminaCostText != null)
            staminaCostText.text = $"${staminaCost}";
        
        if (staminaBuyButton != null)
            staminaBuyButton.interactable = UpgradeDataManager.Instance.CanUpgrade("Stamina");
    }
    
    private void BuyUpgrade(string upgradeType)
    {
        if (UpgradeDataManager.Instance.BuyUpgrade(upgradeType))
        {
            UpdateUI();
        }
    }
}