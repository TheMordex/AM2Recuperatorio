using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Canvas canvas; // ✅ NUEVO
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        // ✅ Asegurar que esté desactivado
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // ✅ Configurar Canvas
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }

        // ✅ Configurar botones
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(PlayAgain);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    public void ShowVictoryScreen(int coinsEarned)
    {
        Debug.Log($"Mostrando pantalla de victoria: {coinsEarned} monedas");

        // ✅ 1. Activar panel ANTES de congelar tiempo
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        else
            Debug.LogError("VictoryPanel es NULL!");

        // ✅ 2. Actualizar texto
        if (coinsText != null)
            coinsText.text = "Monedas Obtenidas: " + coinsEarned;

        // ✅ 3. Congelar tiempo AL FINAL
        Time.timeScale = 0f;
    }

    private void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}