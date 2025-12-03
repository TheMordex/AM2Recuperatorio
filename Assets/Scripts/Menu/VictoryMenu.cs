using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;
    
    private void Start()
    {
        victoryPanel.SetActive(false);
        
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(PlayAgain);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
    }
    
    public void ShowVictoryScreen(int coinsEarned)
    {
        Time.timeScale = 0f; // Pausar el juego
        victoryPanel.SetActive(true);
        
        if (coinsText != null)
            coinsText.text = $"Monedas Obtenidas: {coinsEarned}";
        
        Debug.Log($"¡VICTORIA! Monedas ganadas: {coinsEarned}");
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