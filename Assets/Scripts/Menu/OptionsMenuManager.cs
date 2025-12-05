using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("Buscar por nombre si están en None")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button resetDataButton;
    [SerializeField] private Button confirmDeleteButton;
    [SerializeField] private Button closeConfirmationButton; 
    [SerializeField] private Button backButton;
    
    private void Start()
    {
        FindReferences();
        InitializeState();
        SetupButtons();
        
        Debug.Log("✅ OptionsMenuManager inicializado");
    }
    
    private void OnEnable()
    {
        FindReferences();
        InitializeState();
        SetupButtons();
    }
    
    private void FindReferences()
    {
        // Buscar confirmation panel si no está asignado
        if (confirmationPanel == null)
        {
            // Buscar en hijos de este GameObject
            Transform confirmTransform = transform.Find("ConfirmationPanel");
            if (confirmTransform == null)
                confirmTransform = transform.Find("Confirmation Panel");
            if (confirmTransform == null)
                confirmTransform = transform.Find("¿Seguro?");
            
            if (confirmTransform != null)
                confirmationPanel = confirmTransform.gameObject;
        }
        
        // Buscar botones si no están asignados
        if (resetDataButton == null)
        {
            GameObject resetObj = GameObject.Find("Reset Data Button");
            if (resetObj != null)
                resetDataButton = resetObj.GetComponent<Button>();
        }
        
        if (confirmDeleteButton == null)
        {
            GameObject confirmObj = GameObject.Find("Confirm Delete Button");
            if (confirmObj != null)
                confirmDeleteButton = confirmObj.GetComponent<Button>();
        }
        
        if (closeConfirmationButton == null)
        {
            GameObject closeObj = GameObject.Find("Close Confirmation Button");
            if (closeObj != null)
                closeConfirmationButton = closeObj.GetComponent<Button>();
        }
        
        if (backButton == null)
        {
            GameObject backObj = GameObject.Find("Back From Options Button");
            if (backObj != null)
                backButton = backObj.GetComponent<Button>();
        }
    }
    
    private void InitializeState()
    {
        // ASEGURAR que el panel de confirmación esté oculto al inicio
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
        
        // ASEGURAR que el botón de regresar esté habilitado
        if (backButton != null)
            backButton.interactable = true;
        
        // ASEGURAR que el botón de reset esté habilitado
        if (resetDataButton != null)
            resetDataButton.interactable = true;
    }
    
    private void SetupButtons()
    {
        if (resetDataButton != null)
        {
            resetDataButton.onClick.RemoveAllListeners();
            resetDataButton.onClick.AddListener(OpenConfirmationPanel);
        }
        
        if (confirmDeleteButton != null)
        {
            confirmDeleteButton.onClick.RemoveAllListeners();
            confirmDeleteButton.onClick.AddListener(ConfirmResetData);
        }
        
        if (closeConfirmationButton != null)
        {
            closeConfirmationButton.onClick.RemoveAllListeners();
            closeConfirmationButton.onClick.AddListener(CloseConfirmationPanel);
        }
    }
    
    private void OpenConfirmationPanel()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true);
            Debug.Log("📋 Panel de confirmación abierto");
        }
        else
        {
            Debug.LogError("❌ confirmationPanel es NULL!");
        }
        
        if (resetDataButton != null)
            resetDataButton.interactable = false;
        
        if (backButton != null)
            backButton.interactable = false;
    }
    
    private void CloseConfirmationPanel()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
            Debug.Log("❌ Panel de confirmación cerrado");
        }
        
        if (resetDataButton != null)
            resetDataButton.interactable = true;
        
        if (backButton != null)
            backButton.interactable = true;
    }
    
    private void ConfirmResetData()
    {
        Debug.Log("🗑️ Reseteando datos...");
        
        if (UpgradeDataManager.Instance != null)
        {
            UpgradeDataManager.Instance.ResetAllData();
            Debug.Log("✅ Datos reseteados correctamente");
        }
        else
        {
            Debug.LogError("❌ UpgradeDataManager.Instance es NULL!");
        }
        
        CloseConfirmationPanel();
    }
}