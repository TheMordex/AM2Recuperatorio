using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI References - Buscar por nombre si están en None")]
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
        FindReferences();
        SetupButtons();
        UpdateUI();
    }
    
    private void OnEnable()
    {
        FindReferences();
        SetupButtons();
        UpdateUI();
    }
    
    private void Update()
    {
        UpdateUI();
    }
    
    private void FindReferences()
    {
        // Buscar CoinsText
        if (coinsText == null)
        {
            GameObject coinsObj = GameObject.Find("CoinsText");
            if (coinsObj != null)
                coinsText = coinsObj.GetComponent<TextMeshProUGUI>();
        }
        
        // Buscar botones de Max Health
        if (maxHealthBuyButton == null)
        {
            GameObject btnObj = GameObject.Find("Max Health Buy Button");
            if (btnObj != null)
                maxHealthBuyButton = btnObj.GetComponent<Button>();
        }
        
        if (maxHealthLevelText == null)
        {
            GameObject textObj = GameObject.Find("Max Health Level Text");
            if (textObj != null)
                maxHealthLevelText = textObj.GetComponent<TextMeshProUGUI>();
        }
        
        if (maxHealthCostText == null)
        {
            GameObject textObj = GameObject.Find("Max Health Cost Text");
            if (textObj != null)
                maxHealthCostText = textObj.GetComponent<TextMeshProUGUI>();
        }
        
        // Buscar botones de Damage
        if (damageBuyButton == null)
        {
            GameObject btnObj = GameObject.Find("Damage Buy Button");
            if (btnObj != null)
                damageBuyButton = btnObj.GetComponent<Button>();
        }
        
        if (damageLevelText == null)
        {
            GameObject textObj = GameObject.Find("Damage Level Text");
            if (textObj != null)
                damageLevelText = textObj.GetComponent<TextMeshProUGUI>();
        }
        
        if (damageCostText == null)
        {
            GameObject textObj = GameObject.Find("Damage Cost Text");
            if (textObj != null)
                damageCostText = textObj.GetComponent<TextMeshProUGUI>();
        }
        
        // Buscar botones de Knockback
        if (knockbackBuyButton == null)
        {
            GameObject btnObj = GameObject.Find("Knockback Buy Button");
            if (btnObj != null)
                knockbackBuyButton = btnObj.GetComponent<Button>();
        }
        
        if (knockbackLevelText == null)
        {
            GameObject textObj = GameObject.Find("Knockback Level Text");
            if (textObj != null)
                knockbackLevelText = textObj.GetComponent<TextMeshProUGUI>();
        }
        
        if (knockbackCostText == null)
        {
            GameObject textObj = GameObject.Find("Knockback Cost Text");
            if (textObj != null)
                knockbackCostText = textObj.GetComponent<TextMeshProUGUI>();
        }
        
        // Buscar botones de Stamina
        if (staminaBuyButton == null)
        {
            GameObject btnObj = GameObject.Find("Stamina Buy Button");
            if (btnObj != null)
                staminaBuyButton = btnObj.GetComponent<Button>();
        }
        
        if (staminaCountText == null)
        {
            GameObject textObj = GameObject.Find("Stamina Count Text");
            if (textObj != null)
                staminaCountText = textObj.GetComponent<TextMeshProUGUI>();
        }
        
        if (staminaCostText == null)
        {
            GameObject textObj = GameObject.Find("Stamina Cost Text");
            if (textObj != null)
                staminaCostText = textObj.GetComponent<TextMeshProUGUI>();
        }
    }
    
    private void SetupButtons()
    {
        if (maxHealthBuyButton != null)
        {
            maxHealthBuyButton.onClick.RemoveAllListeners();
            maxHealthBuyButton.onClick.AddListener(() => BuyUpgrade("MaxHealth"));
        }
        
        if (damageBuyButton != null)
        {
            damageBuyButton.onClick.RemoveAllListeners();
            damageBuyButton.onClick.AddListener(() => BuyUpgrade("Damage"));
        }
        
        if (knockbackBuyButton != null)
        {
            knockbackBuyButton.onClick.RemoveAllListeners();
            knockbackBuyButton.onClick.AddListener(() => BuyUpgrade("Knockback"));
        }
        
        if (staminaBuyButton != null)
        {
            staminaBuyButton.onClick.RemoveAllListeners();
            staminaBuyButton.onClick.AddListener(() => BuyUpgrade("Stamina"));
        }
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