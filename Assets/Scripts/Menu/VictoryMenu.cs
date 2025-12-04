using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Canvas canvas; 
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
        
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }
        
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(PlayAgain);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    public void ShowVictoryScreen(int coinsEarned)
    {
        
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        else
        
        
        if (coinsText != null)
            coinsText.text = "Monedas Obtenidas: " + coinsEarned;
        
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