using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        // ✅ Asegurar que esté desactivado al inicio
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
            Debug.Log("✅ DefeatPanel desactivado al inicio");
        }
        else
        {
            Debug.LogError("❌ DefeatPanel NO asignado en DefeatMenu!");
        }

        // ✅ Configurar Canvas
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            Debug.Log($"✅ Canvas configurado: {canvas.name}");
        }
        else
        {
            Debug.LogError("❌ Canvas no encontrado en DefeatMenu!");
        }

        // ✅ Configurar botones
        if (tryAgainButton != null)
            tryAgainButton.onClick.AddListener(TryAgain);
        else
            Debug.LogWarning("⚠️ TryAgainButton no asignado");

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        else
            Debug.LogWarning("⚠️ MainMenuButton no asignado");
    }

    public void ShowDefeatScreen(int coins, int wave)
    {
        Debug.Log($"🔴 === ShowDefeatScreen LLAMADO ===");
        Debug.Log($"🔴 Monedas: {coins}, Oleada: {wave}");
        Debug.Log($"🔴 DefeatPanel es null? {defeatPanel == null}");
        
        if (defeatPanel == null)
        {
            Debug.LogError("❌ CRÍTICO: DefeatPanel es NULL! Asígnalo en el Inspector.");
            return;
        }

        Debug.Log($"🔴 DefeatPanel antes de activar: {defeatPanel.activeSelf}");

        // ✅ 1. Activar panel ANTES de congelar tiempo
        defeatPanel.SetActive(true);
        
        Debug.Log($"🔴 DefeatPanel después de activar: {defeatPanel.activeSelf}");

        // ✅ 2. Actualizar textos
        if (coinsText != null)
        {
            coinsText.text = "Monedas Obtenidas: " + coins;
            Debug.Log($"✅ Texto de monedas actualizado: {coinsText.text}");
        }
        else
        {
            Debug.LogWarning("⚠️ CoinsText no asignado");
        }

        if (waveText != null)
        {
            waveText.text = "Oleada Alcanzada: " + wave;
            Debug.Log($"✅ Texto de oleada actualizado: {waveText.text}");
        }
        else
        {
            Debug.LogWarning("⚠️ WaveText no asignado");
        }

        // ✅ 3. Congelar tiempo AL FINAL
        Time.timeScale = 0f;
        Debug.Log("✅ Tiempo congelado (timeScale = 0)");
        
        // ✅ Verificación final
        Debug.Log($"🔴 VERIFICACIÓN FINAL - Panel activo: {defeatPanel.activeSelf}");
    }

    private void TryAgain()
    {
        Debug.Log("🔄 Reiniciando nivel...");
        Time.timeScale = 1f;
        
        // ✅ Resetear GameState
        GameState.IsDead = false;
        GameState.IsPaused = false;
        GameState.IsVictorious = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void BackToMainMenu()
    {
        Debug.Log("🏠 Volviendo al menú principal...");
        Time.timeScale = 1f;
        
        // ✅ Resetear GameState
        GameState.IsDead = false;
        GameState.IsPaused = false;
        GameState.IsVictorious = false;
        
        SceneManager.LoadScene("MenuScene");
    }
}