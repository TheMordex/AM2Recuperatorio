using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button pauseButtonMobile;
    
    private bool isPaused = false;
    private AudioSource[] allAudioSources;
    
    private void Start()
    {
        pausePanel.SetActive(false);
        allAudioSources = FindObjectsOfType<AudioSource>();
        
        if (pauseButtonMobile != null)
            pauseButtonMobile.onClick.AddListener(TogglePause);
        
        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }
    
    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        
        // Pausar todos los AudioSources sin deshabilitar
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
        
        Debug.Log("Juego pausado - Audio pausado");
    }
    
    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        
        // Reanudar todos los AudioSources
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.UnPause();
        }
        
        Debug.Log("Juego reanudado");
    }
    
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}