using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons - Asignar manualmente")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backFromShopButton;
    [SerializeField] private Button backFromOptionsButton;
    
    [Header("Panels - Buscar por nombre si están en None")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject displayInfo;
    
    private void Start()
    {
        FindReferences();
        ShowMainMenu();
        SetupButtons();
        
        Debug.Log("✅ MainMenu inicializado");
    }
    
    private void OnEnable()
    {
        FindReferences();
        ShowMainMenu();
    }
    
    private void FindReferences()
    {
        // Buscar paneles si no están asignados
        if (mainPanel == null)
            mainPanel = GameObject.Find("MainPanel");
        
        if (shopPanel == null)
            shopPanel = GameObject.Find("ShopPanel");
        
        if (optionsPanel == null)
            optionsPanel = GameObject.Find("OptionsPanel");
        
        if (displayInfo == null)
            displayInfo = GameObject.Find("DisplayInfo");
        
        // Buscar botones si no están asignados
        if (playButton == null)
        {
            GameObject playObj = GameObject.Find("Play Button");
            if (playObj != null)
                playButton = playObj.GetComponent<Button>();
        }
        
        if (shopButton == null)
        {
            GameObject shopObj = GameObject.Find("Shop Button");
            if (shopObj != null)
                shopButton = shopObj.GetComponent<Button>();
        }
        
        if (optionsButton == null)
        {
            GameObject optObj = GameObject.Find("Options Button");
            if (optObj != null)
                optionsButton = optObj.GetComponent<Button>();
        }
        
        if (quitButton == null)
        {
            GameObject quitObj = GameObject.Find("Quit Button");
            if (quitObj != null)
                quitButton = quitObj.GetComponent<Button>();
        }
        
        if (backFromShopButton == null)
        {
            GameObject backShopObj = GameObject.Find("Back From Shop Button");
            if (backShopObj != null)
                backFromShopButton = backShopObj.GetComponent<Button>();
        }
        
        if (backFromOptionsButton == null)
        {
            GameObject backOptObj = GameObject.Find("Back From Options Button");
            if (backOptObj != null)
                backFromOptionsButton = backOptObj.GetComponent<Button>();
        }
    }
    
    private void SetupButtons()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(PlayGame);
        }
        
        if (shopButton != null)
        {
            shopButton.onClick.RemoveAllListeners();
            shopButton.onClick.AddListener(OpenShop);
        }
        
        if (optionsButton != null)
        {
            optionsButton.onClick.RemoveAllListeners();
            optionsButton.onClick.AddListener(OpenOptions);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(QuitGame);
        }
        
        if (backFromShopButton != null)
        {
            backFromShopButton.onClick.RemoveAllListeners();
            backFromShopButton.onClick.AddListener(CloseShop);
        }
        
        if (backFromOptionsButton != null)
        {
            backFromOptionsButton.onClick.RemoveAllListeners();
            backFromOptionsButton.onClick.AddListener(CloseOptions);
        }
    }
    
    private void ShowMainMenu()
    {
        // Asegurar que tenemos las referencias
        FindReferences();
        
        // Mostrar panel principal
        if (mainPanel != null)
            mainPanel.SetActive(true);
        else
            Debug.LogError("❌ mainPanel es NULL!");
        
        // Ocultar otros paneles
        if (shopPanel != null)
            shopPanel.SetActive(false);
        
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
        
        // Mostrar info de monedas/stamina
        if (displayInfo != null)
            displayInfo.SetActive(true);
        
        // IMPORTANTE: Habilitar todos los botones del menú principal
        if (playButton != null)
            playButton.interactable = true;
        
        if (shopButton != null)
            shopButton.interactable = true;
        
        if (optionsButton != null)
            optionsButton.interactable = true;
        
        if (quitButton != null)
            quitButton.interactable = true;
        
        Debug.Log("🏠 Menú principal mostrado");
    }
    
    private void PlayGame()
    {
        if (StaminaManager.Instance != null && StaminaManager.Instance.CanPlayLevel())
        {
            StaminaManager.Instance.UseLevelStamina();
            Debug.Log("🎮 Cargando nivel...");
            SceneManager.LoadScene("First Level");
        }
        else
        {
            Debug.LogWarning("⚠️ No hay suficiente stamina para jugar");
        }
    }
    
    private void OpenShop()
    {
        FindReferences();
        
        if (mainPanel != null)
            mainPanel.SetActive(false);
        
        if (shopPanel != null)
            shopPanel.SetActive(true);
        
        if (displayInfo != null)
            displayInfo.SetActive(true);
        
        Debug.Log("🛒 Tienda abierta");
    }
    
    private void CloseShop()
    {
        Debug.Log("🔙 Cerrando tienda");
        ShowMainMenu();
        SetupButtons();
    }
    
    private void OpenOptions()
    {
        FindReferences();
        
        if (mainPanel != null)
            mainPanel.SetActive(false);
        
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
        
        if (displayInfo != null)
            displayInfo.SetActive(false);
        
        Debug.Log("⚙️ Opciones abiertas");
    }
    
    private void CloseOptions()
    {
        Debug.Log("🔙 Cerrando opciones");
        ShowMainMenu();
        SetupButtons();
    }
    
    private void QuitGame()
    {
        Debug.Log("👋 Cerrando juego");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}