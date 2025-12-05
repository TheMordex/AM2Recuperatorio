using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("Confirmation Panel")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button resetDataButton;
    [SerializeField] private Button confirmDeleteButton;
    [SerializeField] private Button closeConfirmationButton; 
    [SerializeField] private Button backButton;
    
    private void Start()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
        
        if (resetDataButton != null)
            resetDataButton.onClick.AddListener(OpenConfirmationPanel);
        
        if (confirmDeleteButton != null)
            confirmDeleteButton.onClick.AddListener(ConfirmResetData);
        
        if (closeConfirmationButton != null)
            closeConfirmationButton.onClick.AddListener(CloseConfirmationPanel);
    }
    
    private void OpenConfirmationPanel()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(true);
        
        if (backButton != null)
            backButton.interactable = false;
    }
    
    private void CloseConfirmationPanel()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
        
        if (backButton != null)
            backButton.interactable = true;
    }
    
    private void ConfirmResetData()
    {
        if (UpgradeDataManager.Instance != null)
        {
            UpgradeDataManager.Instance.ResetAllData();
        }
        
        CloseConfirmationPanel();
    }
}