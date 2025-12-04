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
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }
        
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }
        
        if (tryAgainButton != null)
            tryAgainButton.onClick.AddListener(TryAgain);
        else
        

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    public void ShowDefeatScreen(int coins, int wave)
    {
        if (defeatPanel == null)
        {
            return;
        }
        defeatPanel.SetActive(true);
        
        if (coinsText != null)
        {
            coinsText.text = "Monedas Obtenidas: " + coins;
        }
     
        if (waveText != null)
        {
            waveText.text = "Oleada Alcanzada: " + wave;
        }
        
        Time.timeScale = 0f;
    }

    private void TryAgain()
    {
        Time.timeScale = 1f;
        
        GameState.IsDead = false;
        GameState.IsPaused = false;
        GameState.IsVictorious = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void BackToMainMenu()
    {
        Time.timeScale = 1f;
        
        GameState.IsDead = false;
        GameState.IsPaused = false;
        GameState.IsVictorious = false;
        
        SceneManager.LoadScene("MenuScene");
    }
}