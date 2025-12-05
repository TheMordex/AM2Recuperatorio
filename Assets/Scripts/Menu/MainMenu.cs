using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject displayInfo;
    [SerializeField] private Button backFromShopButton;
    [SerializeField] private Button backFromOptionsButton;
    
    private void Start()
    {
        ShowMainMenu();
        
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);
        
        if (shopButton != null)
            shopButton.onClick.AddListener(OpenShop);
        
        if (optionsButton != null)
            optionsButton.onClick.AddListener(OpenOptions);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        if (backFromShopButton != null)
            backFromShopButton.onClick.AddListener(CloseShop);
        
        if (backFromOptionsButton != null)
            backFromOptionsButton.onClick.AddListener(CloseOptions);
    }
    
    private void ShowMainMenu()
    {
        if (mainPanel != null)
            mainPanel.SetActive(true);
        if (shopPanel != null)
            shopPanel.SetActive(false);
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
        if (displayInfo != null)
            displayInfo.SetActive(true);
    }
    
    private void PlayGame()
    {
        if (StaminaManager.Instance != null && StaminaManager.Instance.CanPlayLevel())
        {
            StaminaManager.Instance.UseLevelStamina();
            SceneManager.LoadScene("First Level");
        }
    }
    
    private void OpenShop()
    {
        if (mainPanel != null)
            mainPanel.SetActive(false);
        if (shopPanel != null)
            shopPanel.SetActive(true);
        if (displayInfo != null)
            displayInfo.SetActive(true);
    }
    
    private void CloseShop()
    {
        ShowMainMenu();
    }
    
    private void OpenOptions()
    {
        if (mainPanel != null)
            mainPanel.SetActive(false);
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
        if (displayInfo != null)
            displayInfo.SetActive(false);
    }
    
    private void CloseOptions()
    {
        ShowMainMenu();
    }
    
    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}